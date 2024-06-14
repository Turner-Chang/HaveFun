using HaveFun.DTOs;
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
            //var viewModel = new FriendList
            //{
            //    Friends = _friendService.GetFriends(),
            //    BlackList = _friendService.GetBlockedFriends()
            //};
            var viewModel = new List<FriendDTO>();

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
        public ActionResult<List<FriendDTO>> GetFriends()
        {
            return _friendService.GetFriends();
        }

        [HttpGet("blocked")]
        public ActionResult<List<FriendDTO>> GetBlockedFriends()
        {
            return _friendService.GetBlockedFriends();
        }
    }
}
