using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;



namespace InstructorWebClient.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _service;
        private readonly ISessionManagerService _sessionManagerService;
        private readonly IStudentService _studentService;

        public SessionController(ISessionService service, ISessionManagerService sessionManagerService, IStudentService studentService)
        {
            _service = service;
            _sessionManagerService = sessionManagerService;
            _studentService = studentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Domain.SessionLister());
        }

        [HttpPost]
        public IActionResult List(Domain.SessionLister mLister)
        {
            try
            {
                Thread.Sleep(1000);

                var mLoginUser = _sessionManagerService.GetLoginUser();

                ViewBag.ShowInstructor = false;
                if (mLoginUser.UserType == EnumHelper.GetEnumString(Enums.UserType.Admin))
                    ViewBag.ShowInstructor = true;
                else
                    mLister.SearchCriteria.UserId = mLoginUser.Id;

                mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ViewBag.Pagination = new List<SelectListItem>
                {
                    new SelectListItem { Text = "5", Value = "5" },
                    new SelectListItem { Text = "10", Value = "10" },
                    new SelectListItem { Text = "20", Value = "20" },
                    new SelectListItem { Text = "50", Value = "50" },
                    new SelectListItem { Text = "100", Value = "100" },
                    new SelectListItem { Text = "All", Value = "-1" }
                };

                ViewBag.Students = _studentService.GetActiveStudent().GetAwaiter().GetResult();
            }
            return View("_List", mLister);
        }

        public IActionResult Create()
        {
            var mSession = new Domain.Session();
            return PartialView("_Create", mSession);
        }

        [HttpPost]
        public JsonResult Create([FromBody] Domain.Session mSession)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                Thread.Sleep(1000);
                if (mSession != null)
                {
                    mSession = _service.UpsertAsync(mSession).GetAwaiter().GetResult();

                    if (mSession.Id > default(int))
                    {
                        resultData = new { NotificationType = EnumHelper.GetEnumString(Enums.Result.Success).ToLower(), NotificationMsg = string.Format(Constant.SuccessMessage, EnumHelper.GetEnumString(Enums.Operation.Session)) };
                    }
                    else
                    {
                        resultData = new { NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower(), NotificationMsg = string.Format(Constant.ErrorMessage, EnumHelper.GetEnumString(Enums.Operation.Session), "saving") };
                    }
                }
                else
                {
                    resultData = new { NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower(), NotificationMsg = string.Format(Constant.ErrorMessage, EnumHelper.GetEnumString(Enums.Operation.Session), "saving") };
                }
            }
            catch (Exception)
            {
                resultData = new { NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower(), NotificationMsg = Constant.InternalServerError };
            }
            finally
            {
                ViewBag.Students = _studentService.GetActiveStudent().GetAwaiter().GetResult();
            }
            return Json(resultData);
        }

        [HttpGet]
        public IActionResult Get(int id, int classType)
        {
            var mSession = new Domain.Session();
            try
            {
                mSession.Id = id;
                mSession.ClassType = classType;
                mSession.UserId = _sessionManagerService.GetLoginUser().Id;
            }
            catch
            {
                ViewBag.NotificationType = "error";
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            finally
            {
                ViewBag.Students = _studentService.GetActiveStudent().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mSession);
        }

        public IActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();

            try
            {
                Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { Type = EnumHelper.GetEnumString(Enums.Result.Success).StrToUpper(), Msg = "Record has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { Type = EnumHelper.GetEnumString(Enums.Result.Error).StrToUpper(), Msg = "Error occured while deleting the record!" };
            }

            return Json(resultData);
        }

        [HttpGet]
        public ActionResult GetModuleRowPartial(int index)
        {
            ViewBag.ModuleIndex = index;
            return PartialView("_ModuleRowPartial", new Domain.Module());
        }

        [HttpGet]
        public ActionResult ClassOnePartial()
        {
            Thread.Sleep(1000);
            var mSession = new Domain.Session();
            mSession.UserId = _sessionManagerService.GetLoginUser().Id;
            mSession.ClassType = 1;
            ViewBag.Students = _studentService.GetActiveStudent().GetAwaiter().GetResult();
            return PartialView("_ClassOne", mSession);
        }

        [HttpGet]
        public ActionResult ClassThreePartial()
        {
            Thread.Sleep(1000);
            var mSession = new Domain.Session();
            mSession.UserId = _sessionManagerService.GetLoginUser().Id;
            mSession.ClassType = 3;
            ViewBag.Students = _studentService.GetActiveStudent().GetAwaiter().GetResult();
            return PartialView("_ClassThree", mSession);
        }
    }
}
