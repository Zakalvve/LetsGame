using BlazorApp.Components;
using BlazorApp.Extensions;
using BlazorApp.Services;
using LetsGame.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddRazorPages();

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGet("/logout", async (HttpContext context, LogOutService logout) =>
{
    await logout.LogOutAsync(context);
});

app.MapGet("/login", async (HttpContext context, string? returnUrl) =>
{
    var ReturnUrl = String.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl; 
    var url = new Uri(ReturnUrl, UriKind.RelativeOrAbsolute);
    context.Response.Redirect(ReturnUrl);

}).RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
