using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(ApplicationContext context, IConfiguration config) : ControllerBase
    {
        [HttpGet("profile-pictures")]
        public async Task<ActionResult<List<string>>> Get()
        {
            var root = config.GetRequiredValue("IdentityOptions:PicBaseUrl");
            var profilePictures = await context.ProfilePictures.Select(picture => $"{root}/{picture.imagePath}").ToListAsync();
            
            return profilePictures;
        }


    }
}