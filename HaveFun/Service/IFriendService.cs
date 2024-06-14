using HaveFun.DTOs;

namespace HaveFun.Service
{
    public interface IFriendService
    {
        List<FriendDTO> GetFriends();
        List<FriendDTO> GetBlockedFriends();
        void BlockFriend(int friendId);
        void UnblockFriend(int friendId);
    }
}

