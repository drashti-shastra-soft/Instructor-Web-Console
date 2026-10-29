using InstructorWebClient.Helper;
using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Infrastructure.Utils;
using InstructorWebClient.Service;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace InstructorWebClient.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _service;

        public CourseController(ICourseService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(new Domain.CourseLister());
        }

        [HttpPost]
        public IActionResult List(Domain.CourseLister mLister)
        {
            try
            {
                System.Threading.Thread.Sleep(1000);
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
        public ActionResult Create()
        {
            return PartialView("_Create", new Domain.Course());
        }

        [HttpPost]
        public ActionResult Create(Domain.Course mCourse)
        {
            try
            {
                Thread.Sleep(1000);
                mCourse = _service.UpsertAsync(mCourse).GetAwaiter().GetResult();
                if (mCourse != null && mCourse.Id > default(int))
                {
                    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Success).StrToLower();
                    ViewBag.NotificationMsg = string.Format(Constant.SuccessMessage, EnumHelper.GetEnumString(Enums.Operation.Course));
                }
                else
                {
                    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                    ViewBag.NotificationMsg = string.Format(Constant.ErrorMessage, EnumHelper.GetEnumString(Enums.Operation.Course), "saving");
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            
            return PartialView("_Create", mCourse);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mUser = new Domain.Course();
            try
            {
                Thread.Sleep(1000);
                mUser = _service.GetByAsync(id).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            return PartialView("_Create", mUser);
        }

        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
                resultData = new { Type = EnumHelper.GetEnumString(Enums.Result.Success).StrToLower(), Msg = "CourseS has been deleted!" };
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                resultData = new { Type = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower(), Msg = "Error occured while deleting the record!" };
            }
            return Json(resultData);
        }
    }
}
