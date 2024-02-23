using Newtonsoft.Json;
using System.Net;
namespace LetsGame.Common.Clients;


public class ClientTest : BaseClient
{
    public ClientTest(HttpClient client) : base(client) { }

    public async Task<LGEvent[]> GetEvents()
    {
        var http = await MakeRequest<LGEvent[]>()
            .BuildRequest(HttpMethod.Get, "events", uriKind: UriKind.Relative)
            .SendRequest()
            .DeserializeJsonResponse()
            .Run();

        return http.Result() ?? [];
    }

    public async Task PostEvent()
    {
        var http = await MakeRequest<object>()
            .BuildRequest(HttpMethod.Post, "events", uriKind: UriKind.Relative)
            .SendRequest()
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

    public HttpRequestBuilder<T> MakeRequest<T>() where T : class
    {
        return new HttpRequestBuilder<T>(_client);
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

public class HttpRequestBuilder<T> : IDisposable where T : class
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
        _result = null;
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

    public HttpRequestBuilder<T> SendRequest<TInput>(TInput? data = null) where TInput : class
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
    private async Task SendRequestAsync<TInput>(TInput? data = null) where TInput : class
    {
        if (_request == default) throw new InvalidOperationException("Cannot send request before request is built");

        string? jsonString = null;

        if (data != null)
        {
            jsonString = JsonConvert.SerializeObject(data);
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

    public T? Result() => _result;

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
        DeserializationError
    }
}
