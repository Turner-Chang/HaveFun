
const currentUserId = 1; // 假設當前用戶ID為1，實際使用時應該從服務器或session中獲取

new Vue({
    el: '#friendListApp',
    data: {
        friends: [],
        searchQuery: '',
        activeFriendId: null
    },
    computed: {
        filteredFriends() {
            return this.friends.filter(friend =>
                friend.name.toLowerCase().includes(this.searchQuery.toLowerCase())
            );
        }
    },
    methods: {
        selectFriend(friend) {
            this.activeFriendId = friend.id;
            // 這裡可以觸發聊天窗口的更新或其他相關操作
            console.log('Selected friend:', friend);
        },
        fetchFriends() {
            axios.get(`/api/FriendApi/GetAllFriends/${currentUserId}`)
                .then(response => {
                    this.friends = response.data.map(friend => ({
                        id: friend.id,
                        name: friend.name,
                        avatarUrl: friend.profilePicture ? `/api/UserInfo/GetPicture/${friend.id}` : '/img/default-avatar.png',
                        isOnline: false,
                        status: 'offline'
                    }));
                })
                .catch(error => {
                    console.error('獲取好友列表失敗:', error);
                });
        },
        updateFriendStatus(friendId, isOnline) {
            const friend = this.friends.find(f => f.id === friendId);
            if (friend) {
                friend.isOnline = isOnline;
                friend.status = isOnline ? 'online' : 'offline';
            }
        },
        addFriend(newFriend) {
            if (!this.friends.some(f => f.id === newFriend.id)) {
                this.friends.push({
                    id: newFriend.id,
                    name: newFriend.name,
                    avatarUrl: newFriend.profilePicture ? `/api/UserInfo/GetPicture/${newFriend.id}` : '/img/default-avatar.png',
                    isOnline: false,
                    status: 'offline'
                });
            }
        },
        removeFriend(friendId) {
            this.friends = this.friends.filter(f => f.id !== friendId);
        }
    },
    mounted() {
        this.fetchFriends();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/chatHub")
            .build();

        connection.on("UserOnline", (userId) => {
            this.updateFriendStatus(userId, true);
        });

        connection.on("UserOffline", (userId) => {
            this.updateFriendStatus(userId, false);
        });

        connection.on("NewFriend", (friendData) => {
            this.addFriend(friendData);
        });

        connection.on("FriendRemoved", (friendId) => {
            this.removeFriend(friendId);
        });

        connection.start().catch(err => console.error(err.toString()));
    }
});