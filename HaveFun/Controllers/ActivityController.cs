using HaveFun.Models;
using HaveFun.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Policy;

namespace HaveFun.Controllers
{
	public class ActivityController : Controller
	{
		private readonly HaveFunDbContext _context;

        private int _userId;


        public ActivityController(HaveFunDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);

			//檢查Cookie 是否存在並嘗試獲取其值
			if (Request.Cookies.TryGetValue("userId", out string userIdString) && int.TryParse(userIdString, out int userId))
			{
				_userId = userId;
			}
			else
			{
				_userId = -1; //默認值或其他處理
			}
		}

		[Authorize(AuthenticationSchemes = "Bearer,Cookies")]
		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult Index()
		{
			ViewBag.UserId = _userId;
			return View();
		}

		public IActionResult Create()
		{
			var activityTypes = _context.ActivityTypes;
            var model = new ActivityViewModel
            {
                Activity = new Activity()
            };
			model.ActivityTypes = activityTypes;
            return View(model);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        //設定上傳檔案限制, 位元組為單位
        [RequestFormLimits(MultipartBodyLengthLimit = 4096000)]
        [RequestSizeLimit(4096000)]
        public async Task<IActionResult> Create(ActivityViewModel model)
		{
            model.Activity.UserId = _userId;
            if (ModelState.IsValid)
			{
                //if (model.UploadedPicture != null && model.UploadedPicture.Length > 0)
                if (Request.Form.Files["UploadedPicture"] != null)
                {
                    ReadUploadImage(model);
                }

                _context.Activities.Add(model.Activity);
                await _context.SaveChangesAsync();

				int newActivityId = model.Activity.Id;
				var newActivityParticipant = new ActivityParticipant { UserId = _userId, ActivityId = newActivityId };
				_context.ActivityParticipantes.Add(newActivityParticipant);
				await _context.SaveChangesAsync();

				TempData["SuccessMessage"] = "新增活動成功！";

                return RedirectToAction(nameof(Index));
			}
            model.ActivityTypes = _context.ActivityTypes;
            return View(model);
		}

        private void ReadUploadImage(ActivityViewModel model)
        {
            //讀取上傳檔案的内容
            using (BinaryReader br = new BinaryReader(Request.Form.Files["UploadedPicture"].OpenReadStream()))
            {
                model.Activity.Picture = br.ReadBytes((int)Request.Form.Files["UploadedPicture"].Length);
            }
        }

		[HttpGet("Activity/Detail/{id}")]
		public async Task<IActionResult> Detail(int? id)
		{
			ViewBag.LoginUserId = _userId;
			if (id == null)
			{
				return NotFound();
			}
			var activity = await _context.Activities
				.Include(a => a.User)
				.Include(a => a.ActivityParticipants)
					.ThenInclude(ap => ap.User)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (activity == null)
			{
				return NotFound();
			}

			foreach (var participant in activity.ActivityParticipants)
			{
				if (participant.User != null && !string.IsNullOrEmpty(participant.User.ProfilePicture))
				{
					participant.User.ProfilePicture = participant.User.ProfilePicture.Replace(@"\", "/");
					var index = participant.User.ProfilePicture.LastIndexOf("images/");
					if (index != -1)
					{
						participant.User.ProfilePicture = participant.User.ProfilePicture.Substring(index);
					}
				}
			}

			return View(activity);
		}
    }
}
