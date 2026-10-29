using InstructorWebClient.Infrastructure.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstructorWebClient.Controllers
{
    [Authorize]
    public class LogoutController : Controller
    {
        private readonly ISessionManagerService _sessionHelperService;

        public LogoutController(ISessionManagerService sessionHelperService)
        {
            _sessionHelperService = sessionHelperService;
        }

        public async Task<IActionResult> Index()
        {
            // Clear the login user info
            _sessionHelperService.ClearLoginUser();

            // Clear the remember me cookie
            HttpContext.Response.Cookies.Delete("LoginUser");

            // Sign out of the cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }
    }
}
