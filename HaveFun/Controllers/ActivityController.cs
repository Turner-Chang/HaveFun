using HaveFun.Models;
using HaveFun.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
	public class ActivityController : Controller
	{
		private readonly HaveFunDbContext _context;

		public ActivityController(HaveFunDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			ViewBag.UserId = 2;
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
            model.Activity.UserId = 2;
            if (ModelState.IsValid)
			{
                //if (model.UploadedPicture != null && model.UploadedPicture.Length > 0)
                if (Request.Form.Files["UploadedPicture"] != null)
                {
                    ReadUploadImage(model);
                }

                _context.Activities.Add(model.Activity);
                _context.SaveChanges();

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
    }
}
