using InstructorWebClient.Helper;
using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;

namespace InstructorWebClient.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly ISummaryService _summaryService;
        private readonly IUserTypeService _userTypeService;

        public UserController(IUserService service, ISummaryService summaryService, IUserTypeService userTypeService)
        {
            _service = service;
            _summaryService = summaryService;
            _userTypeService = userTypeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Domain.UserLister());
        }

        [HttpPost]
        public IActionResult List(Domain.UserLister mLister)
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
        public ActionResult Create()
        {
            Thread.Sleep(1000);
            ViewBag.UserTypes = _userTypeService.GetAllAsync().GetAwaiter().GetResult();
            return PartialView("_Create", new Domain.User());
        }

        [HttpPost]
        public ActionResult Create(Domain.User mUser)
        {
            try
            {
                Thread.Sleep(1000);
                mUser = _service.UpsertAsync(mUser).GetAwaiter().GetResult();
                if (mUser != null && mUser.Id > default(int))
                {
                    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Success).StrToLower();
                    ViewBag.NotificationMsg = string.Format(Constant.SuccessMessage, EnumHelper.GetEnumString(Enums.Operation.User));
                }
                else
                {
                    ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                    ViewBag.NotificationMsg = string.Format(Constant.ErrorMessage, EnumHelper.GetEnumString(Enums.Operation.User), "saving");
                }
            }
            catch (Exception)
            {
                ViewBag.NotificationType = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower();
                ViewBag.NotificationMsg = Constant.InternalServerError;
            }
            finally
            {
                ViewBag.UserTypes = _userTypeService.GetAllAsync().GetAwaiter().GetResult();
            }
            return PartialView("_Create", mUser);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var mUser = new Domain.User();
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
            finally
            {
                ViewBag.UserTypes = _userTypeService.GetAllAsync().GetAwaiter().GetResult();
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
                resultData = new { Type = EnumHelper.GetEnumString(Enums.Result.Success).StrToLower(), Msg = "User has been deleted!" };
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                resultData = new { Type = EnumHelper.GetEnumString(Enums.Result.Error).StrToLower(), Msg = "Error occured while deleting the record!" };
            }
            return Json(resultData);
        }


        [HttpGet, Route("User/GetSessionHistoryView/{id}/{name}")]
        public IActionResult GetSessionHistoryView(int id, string name, int year = default(int), int month = default(int))
        {
            Thread.Sleep(1000);
            var mModel = _summaryService.GetSessionSummaryAsync(id, year, month).GetAwaiter().GetResult();

            mModel.Username = name;
            mModel.UserId = id;

            ViewBag.Years = mModel.Years.Select(item => new SelectListItem
            {
                Text = item.Text,
                Value = item.Value,
                Selected = item.Selected,
            }).ToList();

            ViewBag.Months = mModel.Months.Select(item => new SelectListItem
            {
                Text = item.Text,
                Value = item.Value,
                Selected = item.Selected,
            }).ToList();

            return PartialView("_SessionHistory", mModel);
        }

        [HttpGet]
        public IActionResult GetSessionHistory(int id, int year, int month)
        {
            Thread.Sleep(1000);
            var mModel = _summaryService.GetSessionSummaryAsync(id, year, month).GetAwaiter().GetResult();
            return Json(mModel);
        }
    }
}
