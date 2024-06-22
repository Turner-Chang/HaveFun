
const { createApp } = Vue;
const currentUserId = 7;
createApp({
    data() {
        return {
            friends: [],
            searchQuery: '',
            activeFriendId: null
        };
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
            if (!friend.isBlocked) {
                this.activeFriendId = friend.id;
                console.log('選擇的朋友:', friend);
            } else {
                console.log('無法選擇被封鎖的朋友');
            }
        },
        fetchFriends() {
            axios.get(`/api/FriendApi/GetFriend/${currentUserId}`)
                .then(response => {
                    this.friends = response.data.map(friend => ({
                        id: friend.id,
                        name: friend.name,
                        avatarUrl: friend.profilePicture ?
                            (friend.profilePicture.startsWith('http') ? friend.profilePicture : `/api/UserInfo/GetPicture/${friend.id}`) :
                            '/img/default-avatar.png',
                        isOnline: false,
                        status: 'offline',
                        isBlocked: friend.isBlocked
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
                    avatarUrl: newFriend.profilePicture || '/img/default-avatar.png',
                    isOnline: false,
                    status: 'offline',
                    isBlocked: false
                });
            }
        },
        removeFriend(friendId) {
            this.friends = this.friends.filter(f => f.id !== friendId);
        },
        blockFriend(friendId) {
            axios.post('/api/FriendApi/BlockUser', {
                userId: currentUserId,
                friendId: friendId
            })
                .then(() => {
                    const friend = this.friends.find(f => f.id === friendId);
                    if (friend) {
                        friend.isBlocked = true;
                    }
                    console.log('用戶已被封鎖');
                })
                .catch(error => {
                    console.error('封鎖用戶失敗:', error);
                });
        },
        unblockFriend(friendId) {
            axios.post('/api/FriendApi/UnblockUser', {
                userId: currentUserId,
                friendId: friendId
            })
                .then(() => {
                    const friend = this.friends.find(f => f.id === friendId);
                    if (friend) {
                        friend.isBlocked = false;
                    }
                    console.log('用戶已被解除封鎖');
                })
                .catch(error => {
                    console.error('解除封鎖失敗:', error);
                });
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
}).mount('#friendListApp');
