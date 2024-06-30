using HaveFun.Common;
using HaveFun.DTOs;
using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HaveFun.Controllers
{
    public class MemberLevelController : Controller
    {
        private HaveFunDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private BankInfoModelDTO _bankInfoModel;

        public MemberLevelController(HaveFunDbContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            // 初始化 BankInfoModelDTO 物件
            _bankInfoModel = new BankInfoModelDTO
            {
                MerchantID = "MS152946101",
                HashKey = "jEtYKs90ytwsg99AHNHjrRQigebWnI1k",
                HashIV = "CWzEtUYpjfCzEOmP",
                ReturnURL = @$"{_configuration["ServerHost"]}/MemberLevel/ReturnCheck",
                NotifyURL = "xxx",
                CustomerURL = "xxx",
                AuthUrl = "https://ccore.newebpay.com/MPG/mpg_gateway",
                CloseUrl = "https://core.newebpay.com/API/CreditCard/Close"
            };
        }

        public IActionResult Index()
        {
            return View();
        }
 
        public IActionResult Pay()
        {

            int userId = User.GetUserId();
            string ordernumber = $"{DateTime.Now.Ticks}_{userId}"; 

            // 目前時間轉換 +08:00, 防止傳入時間或 Server 時間時區不同造成錯誤
            DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));
            TradeInfoDTO tradeInfo = new TradeInfoDTO
            {
                // * 商店代號
                MerchantID = _bankInfoModel.MerchantID,
                // * 回傳格式
                RespondType = "JSON",
                // * TimeStamp
                TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                // * 串接程式版本
                Version = "2.0",
                // * 商店訂單編號
                MerchantOrderNo = ordernumber,
                // * 訂單金額
                Amt = 300,
                // * 商品資訊
                ItemDesc = "會員等級",
                // 支付完成 返回商店網址
                ReturnURL = _bankInfoModel.ReturnURL,
                // 信用卡 一次付清啟用 (1=啟用、0 或者未有此參數=不啟用)
                CREDIT = 1
            };

            var inputModel = new SpgatewayInputDTO
            {
                MerchantID = _bankInfoModel.MerchantID,
                Version = tradeInfo.Version,
                AuthUrl = _bankInfoModel.AuthUrl,
            };

            // 將 model 轉換為 List<KeyValuePair<string, string>>，null 值不轉
            List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfoDTO>(tradeInfo);
            // 將 List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
            var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            inputModel.TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, _bankInfoModel.HashKey, _bankInfoModel.HashIV);
            // SHA256 加密
            inputModel.TradeSha = CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{inputModel.TradeInfo}&HashIV={_bankInfoModel.HashIV}");

            return View(inputModel);
        }




        public IActionResult TransactionsError()
        {
            return View();
        }

        public async Task<IActionResult> TransactionsSuccess()
        {
            Transaction transaction = new Transaction
            {
                UserId = User.GetUserId(),
                Amount = 300,
                Product = 0,
                Method = 0,
                Date = DateTime.Now,
                Status = 0
            };
            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();
            return View();
        }
        

        public IActionResult ReturnCheck([FromForm] ReturnTransactionsDTO returnTransactions)
        {

            int userID = User.GetUserId();
            // 把回傳的字串解密
            // string result = CryptoUtil.DecryptAESHex(returnTransactions.TradeInfo, _bankInfoModel.HashKey, _bankInfoModel.HashIV);
            
            // 如果正確，新增訂單到紀錄中並導到成功頁面，失敗則直接導到失敗頁面
            if (returnTransactions.Status == "SUCCESS")
            {
               
                return RedirectToAction("TransactionsSuccess");
            }
            else
            {
                return RedirectToAction("TransactionsError");
            }
        }
    }
}
