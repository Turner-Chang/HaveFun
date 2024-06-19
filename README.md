# HaveFun
HaveFunDev
# �ϥΪ̥\��
1. **���U/�n�J**
  * �@��ϥΪ̵n�J�]�i�ĤT��n�J�^
  * �ѰO�K�X

2. **�ӤH�D��**
  * �ӤH���
    - �����
    - ��K�X
    - ��j�Y�K
    - ��ӤH���
  * �ֳ��w�ڡ]�@��|���ҽk��ܡ^
  * �ӤH�K��
    - �I�g�B�^�СB����
    - ���|�B�R��
    - �i�W�ǹϤ�
  * �n�ͲM��G����/�ѫ�
  * ���ʺ޲z
    - �ѻP������
    - �o�_������
  * �I�ڬ���
    - �d��
    - �h�ڡ]�̫�A���^

3. **�t��**
  * �z��
  * ��/�k��
  * �u������
  * ���|

4. **�ӤH��ѫ�**
  * �@��@���
  * ���D�^��

5. **���ʴ���**
  * ���W�\��

6. **�|������**
  * �ʶR�ժ��|��

# ��O�\��
1. **�ϥΪ̺޲z**
  * ����/�ѫ�
  * �H�c����

2. **�K��f��**

3. **���ʼf��**

4. **�Τ�f��**

5. **���ұ���**
  * �s�W���һP���Ҹs��
  * �R�����һP���Ҹs��

6. **���y����**�]�̫�A���^
  * �|���I�ڬ���
  * �ק����

7. **�Ϫ����R**�]�̫�A���^

8. **���i�޲z**�]�̫�A���^

9. **���D�^��**
  * ��ѫǤ覡�e�{

10. **�޲z�̫ݿ�ƶ�����**
11. @*  <script>
  var chatHistory = [
  ];

  // Function to update chat history
  function updateChatHistory() {
      var chatHistoryContainer = document.querySelector('.chat-history ul');
      chatHistoryContainer.innerHTML = '';
      chatHistory.forEach(chat => {
          var chatItem = document.createElement('li');
          chatItem.className = 'clearfix';
          var chatContent = `
              <div class="message-data ${chat.type === 'other' ? 'text-right' : ''}">
                  <span class="message-data-time">${chat.time}</span>
                  ${chat.img ? `<img src="${chat.img}" alt="avatar">` : ''}
              </div>
              <div class="message ${chat.type === 'other' ? 'other-message float-right' : 'my-message'}">
                  ${chat.message}
              </div>
          `;
          chatItem.innerHTML = chatContent;
          chatHistoryContainer.appendChild(chatItem);
      });
  }

  //user2 對  user1的聊天紀錄

      fetch('/api/ChatRoom/ChatRoomsApi/GetByUser1IdAndUser2Id/2/1')
          .then(response => response.json())
          .then(data => {
              data.forEach(message => {
                  const newChatMessage = {
                      time: moment(message.createTime).format("h:mm A, MMMM D"),
                      img: `/api/UserInfo/GetPicture/${message.user1Id}`,
                      message: message.messageText,
                      type: message.user1Id === 2 ? "my" : "other"
                  };
                  chatHistory.push(newChatMessage);
              });
              updateChatHistory();
          })
          .catch(error => console.error('Error fetching chat history:', error));
   // user1 to user2

      fetch('/api/ChatRoom/ChatRoomsApi/GetByUser1IdAndUser2Id/1/2')
          .then(response => response.json())
          .then(data => {
              data.forEach(message => {
                  const newChatMessage = {
                      time: moment(message.createTime).format("h:mm A, MMMM D"),
                      img: `/api/UserInfo/GetPicture/${message.user1Id}`,
                      message: message.messageText,
                      type: message.user1Id === 1? "my" : "other"
                  };
                  chatHistory.push(newChatMessage);
              });
              updateChatHistory();
          })
          .catch(error => console.error('Error fetching chat history:', error));
 

  // Initial call to update chat history
  updateChatHistory();

  // SignalR connection setup
  const connection = new signalR.HubConnectionBuilder()
      .withUrl("/chatHub")
      .build();

  connection.on("ReceiveMessage", function (user, message) {
      const msg =  message;
      const chat = {
          time: moment().format("h:mm A, MMMM D"),
              img:"/api/UserInfo/GetPicture/1",
          message: msg,
          type: "other"
      };
      chatHistory.push(chat);
      updateChatHistory();
  });

 
      connection.on("SomeOneOnline", (connid) => {
          console.log(`有人連線了,Id:${connid}`)
      });
      connection.on("SomeOneOffline", (connid) => {
          console.log(`有人離線了,Id:${connid}`)
      });
      connection.start().catch(err => console.error(err.toString()));

   //    document.getElementById("sendButton").addEventListener("click", event => {
   //        const user = ""; //select UserInfos.Name where (UserInfos.Id==this.user1Id);
   // ; // Replace with the dynamic user data if needed
   //        const message = document.getElementById("messageInput").value;
   //        connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
   //        document.getElementById("messageInput").value = '';
   //        event.preventDefault();
   //    });
      document.getElementById("sendButton").addEventListener("click", event => {
          const user1Id = 2; // 替換為當前使用者的 ID
          const user2Id = 1; // 替換為收件者的 ID
          const messageText = document.getElementById("messageInput").value;
          const user = "";
          const message = document.getElementById("messageInput").value;
          connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
           document.getElementById("messageInput").value = '';
          // 建立 POST 請求的資料物件
          const data = {
              User1Id: user1Id,
              User2Id: user2Id,
              MessageText: messageText,
              CreateTime: new Date(),
              IsRead: false
          };

          // 送出 POST 請求到伺服器
          fetch('/api/ChatRoom/ChatRoomsApi', {
              method: 'POST',
              headers: {
                  'Content-Type': 'application/json'
              },
              body: JSON.stringify(data)
          })
              .then(response => {
                  if (response.ok) {
                      return response.text();
                  } else {
                      throw new Error('Network response was not ok.');
                  }
              })
              .then(responseText => {
                  console.log(responseText); // 處理伺服器回應（如果需要）
                  document.getElementById("messageInput").value = ''; // 清空輸入欄位
              })
              .catch(error => {
                  console.error('Error saving message:', error);
              });
          event.preventDefault();
      });
  </script> *@