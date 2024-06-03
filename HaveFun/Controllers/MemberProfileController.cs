using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HaveFun.Controllers
{
	public class MemberProfileController : Controller
	{
		// GET: 
		public IActionResult Index()
		{
			return View();
		}
	}
	
	

	// POST: Profile/Edit
	//[HttpPost]
	//public ActionResult Edit(MemberProfile profile)
	//{
		
	//	// 更新資料庫中的資料
	//}
}
