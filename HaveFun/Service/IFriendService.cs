using HaveFun.Models;

namespace HaveFun.Service
{
    public interface IFriendService
    {
        List<Friend> GetFriends();
        List<Friend> GetBlockedFriends();
        void BlockFriend(int friendId);
        void UnblockFriend(int friendId);
    }
}

