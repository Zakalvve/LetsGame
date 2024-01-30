using Identity.API.Components;
using Identity.API.Data;
using Identity.API.Extensions;
using LetsGame.ServiceDefaults;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllersWithViews();

//// Add services to the container.
//builder.Services.AddRazorComponents()
//    .AddInteractiveServerComponents();

builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        context.Database.EnsureCreated();
    }
} 
else
{ 
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.UseAntiforgery();

app.MapDefaultControllerRoute();

//app.MapRazorComponents<App>()
//    .AddInteractiveServerRenderMode();

app.Run();
