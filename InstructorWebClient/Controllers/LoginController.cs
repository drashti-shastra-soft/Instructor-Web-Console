using InstructorWebClient.Infrastructure.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InstructorWebClient.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly Infrastructure.IService.IAuthenticationService _service;
        private readonly Infrastructure.IService.ISessionManagerService _sessionManagerService;

        public LoginController(Infrastructure.IService.IAuthenticationService service, Infrastructure.IService.ISessionManagerService sessionManagerService)
        {
            _service = service;
            _sessionManagerService = sessionManagerService;
        }

        public ActionResult Index()
        {
            var mCredential = new Domain.Credential();

            // Only try to read cookie if RememberMe is supported in your system
            string cookieValue = HttpContext.Request.Cookies["LoginUser"];
            if (cookieValue.IsNotNullOrEmpty())
                mCredential = JsonSerializer.Deserialize<Domain.Credential>(cookieValue, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(mCredential);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Domain.Credential mCredential)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", mCredential);
            }

            var mUser = await _service.ValidateAsync(mCredential);

            if (mUser == null || mUser.Id == default(int))
            {
                ViewData["NotificationType"] = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewData["NotificationMsg"] = "Invalid username or password.";
                return View("Index", mCredential);
            }

            if (!mUser.IsActive)
            {
                ViewData["NotificationType"] = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewData["NotificationMsg"] = "Your account has been temporarily suspended. Please contact the administrator for assistance.";
                return View("Index", mCredential);
            }

            _sessionManagerService.SetLoginUser(mUser);

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, mUser.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{mUser.FirstName} {mUser.LastName}"),
                new Claim(ClaimTypes.Role, mUser.UserType),
            };

            // Create claims identity and principal
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                // Allow refresh for 1 hour
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                IsPersistent = mCredential.RememberMe,
            };

            // Sign in the user with cookie authentication
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Handle "Remember Me" functionality
            if (mCredential.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    HttpOnly = true,
                    Secure = true, // Ensure this is true in production (requires HTTPS)
                    SameSite = SameSiteMode.Strict
                };

                HttpContext.Response.Cookies.Append("LoginUser", JsonSerializer.Serialize(mCredential, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }), cookieOptions);
            }
            else
            {
                // Clear the remember me cookie if not checked
                HttpContext.Response.Cookies.Delete("LoginUser");
            }

            return RedirectToAction("Index", "Dashboard");
        }
    }
}
