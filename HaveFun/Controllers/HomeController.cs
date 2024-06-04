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
        //        //����̡A�H�r�����j���P����� ex "test@gmail.com,test2@gmail.com"
        //        msg.To.Add(string.Join(",", MailList.ToArray()));
        //        msg.From = new MailAddress("sio2.nh2o.creative@gmail.com", "�J�եۤp�j", System.Text.Encoding.UTF8);
        //        //�l����D 
        //        msg.Subject = "TIM101-���ձH�H" + DateTime.Now.ToString();
        //        //�l����D�s�X  
        //        msg.SubjectEncoding = System.Text.Encoding.UTF8;
        //        //�l�󤺮e
        //        msg.Body = $@"<h1>���ߤ���!!</h1>
        //                  <div>�z�o��Iphone18�A��A���I���H�U�s��</div>
        //                  <div><a href='https://www.google.com'>����!!</a></div>
        //                  <div>{content}</div>";
        //        msg.IsBodyHtml = true;
        //        msg.BodyEncoding = System.Text.Encoding.UTF8;//�l�󤺮e�s�X 
        //        msg.Priority = MailPriority.Normal;//�l���u���� 
        //                                           //�إ� SmtpClient ���� �ó]�w Gmail��smtp�D����Port 
        //        #region �䥦 Host
        //        /*
        //         *  outlook.com smtp.live.com port:25
        //         *  yahoo smtp.mail.yahoo.com.tw port:465
        //        */
        //        #endregion
        //        SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
        //        //�]�w�A���b���K�X
        //        MySmtp.Credentials = new System.Net.NetworkCredential("sio2.nh2o.creative@gmail.com", "zlez nmvy leck tayg");
        //        //Gmial �� smtp �ϥ� SSL
        //        MySmtp.EnableSsl = true;
        //        MySmtp.Send(msg);
        //        return Ok("���\"); 
        //    }
        //    catch (Exception)
        //    {
        //        return Ok("����");
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
