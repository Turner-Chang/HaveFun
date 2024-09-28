# HaveFun
HaveFunDev

ER diagram show with react
https://codesandbox.io/p/sandbox/happy-tess-9v3nyy?file=%2Fsrc%2Findex.js%3A13%2C3





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

4. **個人聊天室**     [林晉賢](https://github.com/linjinhsien/)
  * 一對一聊天

### class
[chatroom](https://github.com/Turner-Chang/HaveFun/blob/main/HaveFun/Models/ChatRoom.cs)
[ConId&UserId](https://github.com/Turner-Chang/HaveFun/blob/main/HaveFun/Models/ConId%26UserId.cs)
[ChatHub](https://github.com/Turner-Chang/HaveFun/blob/main/HaveFun/ChatHub.cs)
![image](https://github.com/user-attachments/assets/666126cc-4d12-442d-a641-df7a0097f6e8)


### controller
  [ChatRoomsController](https://github.com/Turner-Chang/HaveFun/blob/main/HaveFun/Controllers/ChatRoomsController.cs)
  [ChatRoomsApiController](https://github.com/Turner-Chang/HaveFun/blob/main/HaveFun/Controllers/APIs/ChatRoomsApiController.cs)
  [ConId_UserController](https://github.com/Turner-Chang/HaveFun/blob/main/HaveFun/Controllers/APIs/ConId_UserController.cs)
 ### view
 [chatroomview](https://github.com/Turner-Chang/HaveFun/blob/main/HaveFun/Views/ChatRooms/Main.cshtml)
 ![image](https://github.com/user-attachments/assets/16a38cdf-30f7-41da-b90f-36b2c5dfe0da)


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
```mermaid
%%{init: {'theme': 'default', 'themeVariables': { 'primaryColor': '#ff79c6', 'secondaryColor': '#bd93f9', 'tertiaryColor': '#f8f8f2', 'mainBkg': '#ffffff', 'nodeBorder': '#ff79c6', 'clusterBkg': '#f8f8f2', 'clusterBorder': '#bd93f9', 'lineColor': '#ff79c6', 'fontFamily': 'arial', 'fontSize': '16px' }}}%%

graph TD
    subgraph 用戶流程
        A[1. 註冊] --> B[2. 登錄]
        B --> C[3. 個人主頁]
        C --> D[4. 匹配]
        D --> E[5. 個人檔案]
        E --> F[6. 聊天室]
        F --> G[7. 活動]
        
        C --> H[個人貼文]
        C --> I[個人資料]
        C --> J[活動管理]
        C --> K[好友清單]
        C --> L[誰喜歡我]
        C --> M[付款紀錄]
    end
    
    subgraph 後台管理
        N[管理員登錄] --> O[後台主頁]
        O --> P[用戶管理]
        O --> Q[內容審核]
        O --> R[活動管理]
        O --> S[系統設置]
        O --> T[數據分析]
    end

style A fill:#ff79c6,stroke:#ff79c6,color:#ffffff
style B fill:#ff79c6,stroke:#ff79c6,color:#ffffff
style C fill:#ff79c6,stroke:#ff79c6,color:#ffffff
style D fill:#ff79c6,stroke:#ff79c6,color:#ffffff
style E fill:#ff79c6,stroke:#ff79c6,color:#ffffff
style F fill:#ff79c6,stroke:#ff79c6,color:#ffffff
style G fill:#ff79c6,stroke:#ff79c6,color:#ffffff

style H fill:#bd93f9,stroke:#bd93f9,color:#ffffff
style I fill:#bd93f9,stroke:#bd93f9,color:#ffffff
style J fill:#bd93f9,stroke:#bd93f9,color:#ffffff
style K fill:#bd93f9,stroke:#bd93f9,color:#ffffff
style L fill:#bd93f9,stroke:#bd93f9,color:#ffffff
style M fill:#bd93f9,stroke:#bd93f9,color:#ffffff

style N fill:#50fa7b,stroke:#50fa7b,color:#ffffff
style O fill:#50fa7b,stroke:#50fa7b,color:#ffffff
style P fill:#8be9fd,stroke:#8be9fd,color:#ffffff
style Q fill:#8be9fd,stroke:#8be9fd,color:#ffffff
style R fill:#8be9fd,stroke:#8be9fd,color:#ffffff
style S fill:#8be9fd,stroke:#8be9fd,color:#ffffff
style T fill:#8be9fd,stroke:#8be9fd,color:#ffffff

classDef default fill:#f8f8f2,stroke:#ff79c6,color:#282a36;
classDef cluster fill:#ffffff,stroke:#bd93f9,color:#282a36;
```
