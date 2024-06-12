using HaveFun.Models;

namespace HaveFun.Service
{
    public class FriendService : IFriendService
    {
        private readonly List<Friend> _friends;
        private readonly List<Friend> _blackList;

        public FriendService()
        {
            _friends = new List<Friend>
        {
            new Friend { Id = 1, Name = "LeBron James", ImageUrl = "assets/images/member/home2/01.jpg" },
            new Friend { Id = 2, Name = "周子魚", ImageUrl = "assets/images/member/home2/02.jpg" },
            new Friend { Id = 3, Name = "Toyz", ImageUrl = "assets/images/member/home2/03.jpg" }
        };

            _blackList = new List<Friend>();
        }

        public List<Friend> GetFriends()
        {
            return _friends;
        }

        public List<Friend> GetBlockedFriends()
        {
            return _blackList;
        }

        public void BlockFriend(int friendId)
        {
            var friend = _friends.FirstOrDefault(f => f.Id == friendId);
            if (friend != null)
            {
                _friends.Remove(friend);
                _blackList.Add(friend);
            }
        }

        public void UnblockFriend(int friendId)
        {
            var friend = _blackList.FirstOrDefault(f => f.Id == friendId);
            if (friend != null)
            {
                _blackList.Remove(friend);
                _friends.Add(friend);
            }
        }
    }
}
