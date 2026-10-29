using InstructorWebClient.Helper;
using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Infrastructure.Utils;
using InstructorWebClient.Service;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace InstructorWebClient.Controllers
{
    public class StudentCourseController : Controller
    {
        private readonly IStudentCourseService _service;
        private readonly ICourseService _courseService;

        public StudentCourseController(IStudentCourseService service, ICourseService courseService)
        {
            _service = service;
            _courseService = courseService;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            if (id <= default(int))
                return RedirectToAction("Index", "Student");

            var mLister = new Domain.StudentCourseLister();
            mLister.SearchCriteria.StudentId = id;
            return View(mLister);
        }

        [HttpPost]
        public IActionResult List(Domain.StudentCourseLister mLister)
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
                ViewBag.CourseNames = _courseService.GetActiveCourses().GetAwaiter().GetResult();
            }
            return PartialView("_List", mLister);
        }

        [HttpGet]
        
        public IActionResult Create()
        {
            var mStudentCourse = new Domain.StudentCourse();
            ViewBag.CourseNames = _courseService.GetActiveCourses().GetAwaiter().GetResult();
            mStudentCourse.StudentId = TempData.Peek("StudentId") != null ? Convert.ToInt32(TempData.Peek("StudentId")) : 0;
            return View("_Create", new Domain.StudentCourse());
        }

        [HttpPost]
        public IActionResult Create(Domain.StudentCourse mStudentCourse)
        {
            try
            {
                Thread.Sleep(1000);
                mStudentCourse = _service.UpsertAsync(mStudentCourse).GetAwaiter().GetResult();
                if (mStudentCourse != null && mStudentCourse.Id > default(int))
                {
                    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Success).ToLower();
                    ViewBag.NotificationMsg = string.Format(Constant.SuccessMessage, EnumHelper.GetEnumString(Enums.Operation.StudentCourse));
                }
                else
                {
                    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower();
                    ViewBag.NotificationMsg = string.Format(Constant.ErrorMessage, EnumHelper.GetEnumString(Enums.Operation.StudentCourse), "saving");
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).ToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            finally
            {
                ViewBag.CourseNames = _courseService.GetActiveCourses().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mStudentCourse);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mStudentCourse = new Domain.StudentCourse();
            try
            {
                Thread.Sleep(1000);
                mStudentCourse = _service.GetByAsync(id).GetAwaiter().GetResult();
            }
            catch
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            finally
            {
                ViewBag.CourseNames = _courseService.GetActiveCourses().GetAwaiter().GetResult();
            }
            return View("_Create", mStudentCourse);
        }

        public ActionResult Delete(int id)
        {
            dynamic resultData = new ExpandoObject();
            try
            {
                Thread.Sleep(1000);
                _service.DeleteAsync(id).GetAwaiter().GetResult();
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
