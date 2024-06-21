# HaveFun
HaveFunDev
# 使用者功能
1. **註冊/登入**
  * 一般使用者登入（可第三方登入）
  * 忘記密碼

2. **個人主頁**
  * 個人資料
    - 改標籤
    - 改密碼
    - 改大頭貼
    - 改個人資料
  * 誰喜歡我（一般會員模糊顯示）
  * 個人貼文
    - 點讚、回覆、分享
    - 檢舉、刪除
    - 可上傳圖片
  * 好友清單：封鎖/解封
  * 活動管理
    - 參與的活動
    - 發起的活動
  * 付款紀錄
    - 查詢
    - 退款（最後再做）

3. **配對**
  * 篩選
  * 左/右滑
  * 優先推薦
  * 檢舉

4. **個人聊天室**
  * 一對一聊天
  * 問題回報

5. **活動揪團**
  * 報名功能

6. **會員等級**
  * 購買白金會員

# 後臺功能
1. **使用者管理**
  * 封鎖/解封
  * 信箱驗證

2. **貼文審核**

3. **活動審核**

4. **用戶審核**

5. **標籤控管**
  * 新增標籤與標籤群組
  * 刪除標籤與標籤群組

6. **金流控管**（最後再做）
  * 會員付款紀錄
  * 修改價格

7. **圖表分析**（最後再做）

8. **公告管理**（最後再做）

9. **問題回報**
  * 聊天室方式呈現

10. **管理者待辦事項提醒**

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
