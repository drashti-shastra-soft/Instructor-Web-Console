using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace InstructorWebClient.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index() => View();
    }
}
