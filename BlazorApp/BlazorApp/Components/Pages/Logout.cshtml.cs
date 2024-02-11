using BlazorApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorApp.Components.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly LogOutService _logOutService;

        public LogoutModel(LogOutService logOutService)
        {
            _logOutService = logOutService;
        }

        public async Task OnGetLogoutAsync()
        {
            await _logOutService.LogOutAsync(HttpContext);
        }
    }
}
