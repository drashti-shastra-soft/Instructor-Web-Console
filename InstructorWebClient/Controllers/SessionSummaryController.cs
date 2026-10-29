using InstructorWebClient.Helper;
using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace InstructorWebClient.Controllers
{
    public class SessionSummaryController : Controller
    {
        private readonly  IUserService _userService;
        private readonly ISummaryService _summaryService;

        public SessionSummaryController(IUserService userService, ISummaryService summaryService)
        {
            _userService = userService;
            _summaryService = summaryService;
        }


        public IActionResult Index(int id)
        {
            if (id <= default(int))
                return RedirectToAction("Index", "User");

            var mLister = new Domain.SessionSummaryLister();
            mLister.SearchCriteria.UserId = id;
            return View(mLister);
        }

        [HttpPost]
        public IActionResult List(Domain.SessionSummaryLister mLister)
        {
            try
            {
                Thread.Sleep(1000);
                //mLister = _service.GetAllAsync(mLister).GetAwaiter().GetResult();
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
            return PartialView("_List", mLister);
        }

        [HttpGet]

        public IActionResult Create()
        {
            var mSessionSummary = new Domain.SessionSummary();
            //ViewBag.CourseNames = _courseService.GetActiveCourses().GetAwaiter().GetResult();
            mSessionSummary.UserId = TempData.Peek("UserId") != null ? Convert.ToInt32(TempData.Peek("UserId")) : 0;
            return View("_Create", new Domain.SessionSummary());
        }

        [HttpPost]
        public IActionResult Create(Domain.SessionSummary mSessionSummary)
        {
            try
            {
                Thread.Sleep(1000);
                //mStudentCourse = _service.UpsertAsync(mStudentCourse).GetAwaiter().GetResult();
                //if (mSessionSummary != null && mSessionSummary.Id > default(int))
                //{
                //    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Success).ToLower();
                //    ViewBag.NotificationMsg = string.Format(Constant.SuccessMessage, EnumHelper.GetEnumString(Enums.Operation.StudentCourse));
                //}
                //else
                //{
                //    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower();
                //    ViewBag.NotificationMsg = string.Format(Constant.ErrorMessage, EnumHelper.GetEnumString(Enums.Operation.StudentCourse), "saving");
                //}
            }
            catch (Exception)
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            return PartialView("_Create", mSessionSummary);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mSessionSummary = new Domain.SessionSummary();
            try
            {
                Thread.Sleep(1000);
                //mStudentCourse = _service.GetByAsync(id).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            return View("_Create", mSessionSummary);
        }

        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                Thread.Sleep(1000);
                //_service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { Type = EnumHelper.GetEnumString(Enums.Result.Success).StrToLower(), Msg = "Student Course has been deleted!" };
            }
            catch (Exception)
            {
                resultData = new { Type = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower(), Msg = "Error occured while deleting the record!" };
            }
            return Json(resultData);
        }
    }
}
