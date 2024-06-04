using HaveFun.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;

namespace HaveFun.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //public IActionResult SendMail(string content)
        //{
        //    try
        //    {
        //        var MailList = new List<string>(){
        //        "danielchen0810@gmail.com",
        //        "winny14752@gmail.com",
        //        "ang498751@gmail.com"
        //        };
            

        //        MailMessage msg = new MailMessage();
        //        //收件者，以逗號分隔不同收件者 ex "test@gmail.com,test2@gmail.com"
        //        msg.To.Add(string.Join(",", MailList.ToArray()));
        //        msg.From = new MailAddress("sio2.nh2o.creative@gmail.com", "蛋白石小姐", System.Text.Encoding.UTF8);
        //        //郵件標題 
        //        msg.Subject = "TIM101-測試寄信" + DateTime.Now.ToString();
        //        //郵件標題編碼  
        //        msg.SubjectEncoding = System.Text.Encoding.UTF8;
        //        //郵件內容
        //        msg.Body = $@"<h1>恭喜中獎!!</h1>
        //                  <div>您得到Iphone18乙支，請點擊以下連結</div>
        //                  <div><a href='https://www.google.com'>按我!!</a></div>
        //                  <div>{content}</div>";
        //        msg.IsBodyHtml = true;
        //        msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
        //        msg.Priority = MailPriority.Normal;//郵件優先級 
        //                                           //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
        //        #region 其它 Host
        //        /*
        //         *  outlook.com smtp.live.com port:25
        //         *  yahoo smtp.mail.yahoo.com.tw port:465
        //        */
        //        #endregion
        //        SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
        //        //設定你的帳號密碼
        //        MySmtp.Credentials = new System.Net.NetworkCredential("sio2.nh2o.creative@gmail.com", "zlez nmvy leck tayg");
        //        //Gmial 的 smtp 使用 SSL
        //        MySmtp.EnableSsl = true;
        //        MySmtp.Send(msg);
        //        return Ok("成功"); 
        //    }
        //    catch (Exception)
        //    {
        //        return Ok("失敗");
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
