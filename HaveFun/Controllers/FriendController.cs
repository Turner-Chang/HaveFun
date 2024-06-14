using HaveFun.Models;
using HaveFun.Service;
using Microsoft.AspNetCore.Mvc;

namespace HaveFun.Controllers
{
    public class FriendController : Controller
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        public IActionResult FriendList()
        {
            var viewModel = new FriendList
            {
                Friends = _friendService.GetFriends(),
                BlackList = _friendService.GetBlockedFriends()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult BlockFriend(int friendId)
        {
            _friendService.BlockFriend(friendId);
            return RedirectToAction("FriendList");
        }

        [HttpPost]
        public IActionResult UnblockFriend(int friendId)
        {
            _friendService.UnblockFriend(friendId);
            return RedirectToAction("FriendList");
        }

        [HttpGet]
        public ActionResult<List<Friend>> GetFriends()
        {
            return _friendService.GetFriends();
        }

        [HttpGet("blocked")]
        public ActionResult<List<Friend>> GetBlockedFriends()
        {
            return _friendService.GetBlockedFriends();
        }
    }
}
