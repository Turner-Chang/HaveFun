new Vue({
  el: '#appc',
  data() {
    return {
      users: [],
      activeUser: {},
      messages: [],
      newMessage: '',
      searchQuery: '',
      connection: null // SignalR connection
    }
  },
  computed: {
    filteredUsers() {
      return this.users.filter(user =>
        user.name.toLowerCase().includes(this.searchQuery.toLowerCase())
      );
    }
  },
  filters: {
    formatTime(value) {
      const date = new Date(value);
      return date.toLocaleTimeString();
    }
  },
  mounted() {
    // Fetch initial data from backend
    axios.get('/chatrooms/users')
      .then(response => {
        this.users = response.data;
        if (this.users.length > 0) {
          this.activeUser = this.users[0];
          this.fetchMessages(this.activeUser.id);
        }
      })
      .catch(error => {
        console.error('Error fetching users:', error);
      });

    // Establish SignalR connection
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl("/chathub")
      .build();

    this.connection.on("ReceiveMessage", (message) => {
      this.messages.push(message);
    });

    this.connection.start()
      .then(() => console.log('SignalR connected'))
      .catch(err => console.error('Error connecting to SignalR:', err));
  },
  methods: {
    fetchMessages(userId) {
      axios.get(`/chatrooms/messages/${userId}`)
        .then(response => {
          this.messages = response.data;
        })
        .catch(error => {
          console.error('Error fetching messages:', error);
        });
    },
    viewInfo(user) {
      this.activeUser = user;
      this.fetchMessages(user.id);
    },
    openCamera() {
      // Open camera logic
      console.log('Camera opened');
    },
    openImageUpload() {
      // Open image upload dialog
      console.log('Image upload opened');
    },
    openSettings() {
      // Open settings dialog
      console.log('Settings opened');
    },
    openHelp() {
      // Open help dialog
      console.log('Help opened');
    },
    sendMessage() {
      if (this.newMessage.trim() === '') {
        return;
      }
      const message = {
        sender: this.activeUser.id,
        text: this.newMessage,
        time: new Date()
      };
      axios.post('/chatrooms/send', message)
        .then(response => {
          this.messages.push(response.data);
          this.newMessage = '';
        })
        .catch(error => {
          console.error('Error sending message:', error);
        });
    }
  }
});
