using InstructorWebClient.Helper;
using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Infrastructure.Utils;
using InstructorWebClient.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace InstructorWebClient.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        #region Declaration
        private readonly IStudentService _service;
        private readonly IStudentSessionService _studentSessionService;
        private readonly IStudentCourseService _studentCourseService;
        private readonly IAddressService _addressService;

        public StudentController(IStudentService service, IStudentSessionService studentSessionService, IStudentCourseService studentCourseService, IAddressService addressService)
        {
            _service = service;
            _studentSessionService = studentSessionService;
            _studentCourseService = studentCourseService;
            _addressService = addressService;
        }
        #endregion

        #region Student
        public IActionResult Index()
        {
            return View(new Domain.StudentLister());
        }

        [HttpPost]
        public IActionResult List(Domain.StudentLister mLister)
        {
            try
            {
                Thread.Sleep(1000);
                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ViewBag.Pagination = PaginationHelper.GetPageSize();
                mLister.Pagination.PaginationSummary = PaginationHelper.GetPaginationSummary(mLister.Pagination);
            }
            return View("_List", mLister);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("_Create", new Domain.Student());
        }

        [HttpPost]
        public IActionResult Create(Domain.Student mStudent)
        {
            try
            {
                Thread.Sleep(1000);
                mStudent = _service.UpsertAsync(mStudent).GetAwaiter().GetResult();
                if (mStudent != null && mStudent.Id > default(int))
                {
                    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Success).ToLower();
                    ViewBag.NotificationMsg = string.Format(Constant.SuccessMessage, EnumHelper.GetEnumString(Enums.Operation.Student));
                }
                else
                {
                    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower();
                    ViewBag.NotificationMsg = string.Format(Constant.ErrorMessage, EnumHelper.GetEnumString(Enums.Operation.Student), "saving");
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            return View("_Create", mStudent);
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var mStudent = new Domain.Student();
            try
            {
                Thread.Sleep(1000);
                mStudent = _service.GetByAsync(id).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            return PartialView("_Create", mStudent);
        }

        public async Task<ActionResult> Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                Thread.Sleep(1000);
                await _service.DeleteAsync(id);
                resultData = new
                {
                    Type = EnumHelper.GetEnumString(Enums.Result.Success),
                    Msg = "Record has been deleted!"
                };
            }
            catch (Exception)
            {
                resultData = new
                {
                    Type = EnumHelper.GetEnumString(Enums.Result.Error),
                    Msg = "Error occurred while deleting the record!"
                };
            }
            return Json(resultData);
        }
        #endregion

        #region Address 
        [HttpGet]
        public IActionResult GetStudentAddress(int id)
        {
            var mAddress = new Domain.Address();
            try
            {
                mAddress = _addressService.GetByAsync(id).GetAwaiter().GetResult();
                mAddress = mAddress ?? new Domain.Address();
                mAddress.StudentId = id;
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
           
            return PartialView("_Address", mAddress);
        }

        [HttpPost]
        public IActionResult SaveStudentAddress(Domain.Address mAddress)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
                mAddress = _addressService.UpsertAsync(mAddress).GetAwaiter().GetResult();

                if (mAddress != null && mAddress.Id > 0)
                {
                    ViewBag.NotificationType = "success";
                    ViewBag.NotificationMsg = "Address has been saved successfully!";
                }
                else
                {
                    ViewBag.NotificationType = "error";
                    ViewBag.NotificationMsg = "Error occurred while saving Address!";
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = "Internal server error!";
            }
            return PartialView("_Address", mAddress);
        }
        #endregion
    }
}
