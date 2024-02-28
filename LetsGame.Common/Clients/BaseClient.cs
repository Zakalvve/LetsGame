using Newtonsoft.Json;
using System.Net;
namespace LetsGame.Common.Clients;


public class ClientTest : BaseClient
{
    public ClientTest(HttpClient client) : base(client) { }

    public async Task<LGEvent[]> GetEvents()
    {
        var http = await BuildRequest<LGEvent[]>(HttpMethod.Get, "events", uriKind: UriKind.Relative)
            .SendRequest()
            .DeserializeJsonResponse()
            .Run();

        return http.Result() ?? [];
    }

    public async Task PostEvent(int ev)
    {
        var http = await BuildRequest(HttpMethod.Post, "events", uriKind: UriKind.Relative)
            .SendRequest(ev)
            .DeserializeJsonResponse()
            .Run();

        if (http.IsSuccess)
        {
            // Success
        } else
        {
            // Failure
        }
    }

    public class LGEvent
    {
        public string Name { get; set; }
    }
}

public abstract class BaseClient : IDisposable
{
    private readonly HttpClient _client;

    public BaseClient(HttpClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Make a request to an external API or service. Does not return any data.
    /// </summary>
    /// <param name="method">The http method to use for this request</param>
    /// <param name="endpoint">The url of the API endpoint</param>
    /// <param name="headers">Any additional headers that will be added to this request</param>
    /// <param name="uriKind">The Uri kind to use for this request. If the Uri kind is relative, the given endpoint will be relative to the HttpClient's base address</param>
    /// <returns>A http request builder to facilitate the construction of a http request pipeline.</returns>
    public HttpRequestBuilder<object> BuildRequest(HttpMethod method, string endpoint, List<KeyValuePair<string, string>>? headers = null, UriKind uriKind = UriKind.Absolute)
    {
        return BuildRequest<object>(method, endpoint, headers, uriKind);
    }

    /// <summary>
    /// Make a request to an external API or service.
    /// </summary>
    /// <typeparam name="TResult">The type returned by API endpoint</typeparam>
    /// <param name="method">The http method to use for this request</param>
    /// <param name="endpoint">The url of the API endpoint</param>
    /// <param name="headers">Any additional headers that will be added to this request</param>
    /// <param name="uriKind">The Uri kind to use for this request. If the Uri kind is relative, the given endpoint will be relative to the HttpClient's base address</param>
    /// <returns>A http request builder to facilitate the construction of a http request pipeline.</returns>
    public HttpRequestBuilder<TResult> BuildRequest<TResult>(HttpMethod method, string endpoint, List<KeyValuePair<string, string>>? headers = null, UriKind uriKind = UriKind.Absolute)
    {
        return new HttpRequestBuilder<TResult>(_client).BuildRequest(method, endpoint, headers, uriKind);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    ~BaseClient()
    {
        Dispose();
    }
}

public class HttpRequestBuilder<T> : IDisposable
{
    private readonly HttpClient _client;
    private HttpRequestBuilderStatus _status;
    private HttpRequestMessage? _request = null;
    private HttpResponseMessage? _response = null;
    private Queue<Func<Task>> _queue = new Queue<Func<Task>>();
    private T? _result;

    public HttpRequestBuilder(HttpClient client)
    {
        _client = client;
        _result = default(T);
    }

    public HttpStatusCode ResponseStatus => _response == null ? HttpStatusCode.PreconditionFailed : _response.StatusCode;
    public string? ReponseMessage => _response == null ? null : _response.ReasonPhrase;
    public bool IsSuccess => _status == HttpRequestBuilderStatus.ReponseSuccess && (_response?.IsSuccessStatusCode ?? false);

    public HttpRequestBuilder<T> BuildRequest(HttpMethod method, string endpoint, List<KeyValuePair<string, string>>? headers = null, UriKind uriKind = UriKind.Absolute)
    {
        var request = new HttpRequestMessage(method, new Uri(endpoint, uriKind));

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        _request = request;

        return this;
    }

    public HttpRequestBuilder<T> SendRequest<TInput>(TInput data)
    {
        _queue.Enqueue(() => SendRequestAsync(data));
        return this;
    }
    public HttpRequestBuilder<T> SendRequest(string? jsonString = null)
    {
        _queue.Enqueue(() => SendRequestAsync(jsonString));
        return this;
    }
    public HttpRequestBuilder<T> SendRequest()
    {
        _queue.Enqueue(() => SendRequestAsync());
        return this;
    }

    private async Task SendRequestAsync<TInput>(TInput data)
    {
        if (_request == default) throw new InvalidOperationException("Cannot send request before request is built");

        string? jsonString = null;

        try
        {
            jsonString = JsonConvert.SerializeObject(data);
        } catch
        {
            _status = HttpRequestBuilderStatus.SerializationError;
            return;
        }

        await SendRequestAsync(jsonString);
    }
    private async Task SendRequestAsync(string? jsonString = null)
    {
        if (_request == default) throw new InvalidOperationException("Cannot send request before request is built");

        if (jsonString != null)
        {
            var body = new StringContent(jsonString, System.Text.Encoding.UTF8, "application.json");
            _request.Content = body;
        }

        await SendRequestAsync();
    }
    private async Task SendRequestAsync()
    {
        if (_request == default) throw new InvalidOperationException("Cannot send request before request is built");

        _response = await _client.SendAsync(_request);

        if (_response.IsSuccessStatusCode) _status = HttpRequestBuilderStatus.ReponseSuccess;
        else _status = HttpRequestBuilderStatus.ResponseFailure;

    }

    public HttpRequestBuilder<T> DeserializeJsonResponse()
    {
        _queue.Enqueue(() => DeserializeJsonResponseAsync());

        return this;
    }
    private async Task DeserializeJsonResponseAsync()
    {
        if (_response == default) throw new InvalidOperationException("Cannot attempt response deserialization before request has been made");

        try
        {
            var jsonString = await _response.Content.ReadAsStringAsync();
            _result = JsonConvert.DeserializeObject<T>(jsonString);
        } 
        catch 
        {
            _status = HttpRequestBuilderStatus.DeserializationError;
        }
    }

    public Task<HttpRequestBuilder<T>> Run()
    {
        return RunRecursive(_queue.Dequeue());
    }
    private async Task<HttpRequestBuilder<T>> RunRecursive(Func<Task> next)
    {
        await next();

        if (_queue.Count > 0)
        {
            await RunRecursive(_queue.Dequeue());
        }

        return this;
    }

    public T? Result()
    {
        object result = _result;

        if (!(result is DBNull))
        {
            return (T)result;
        }

        return default(T);
    }

    public void Dispose()
    {
        _request?.Dispose();
        _response?.Dispose();
    }

    ~HttpRequestBuilder()
    {
        Dispose();
    }

    public enum HttpRequestBuilderStatus
    {
        ReponseSuccess,
        ResponseFailure,
        SerializationError,
        DeserializationError
    }
}
