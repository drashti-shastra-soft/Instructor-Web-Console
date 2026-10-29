using InstructorWebClient.Infrastructure.IService;
using Microsoft.AspNetCore.Mvc;

namespace InstructorWebClient.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressService _service;

        public AddressController(IAddressService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Domain.Address());
        }


    }
}
