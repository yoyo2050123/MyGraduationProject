using System.Diagnostics;
using JapaneseLearnSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace JapaneseLearnSystem.Controllers
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


//�qgithub��U�Ӫ��{���X �O�o�ק�dbJapaneseLearnSystemContext.cs��Program.cs���s�u�r��
//�N�O"Data Source=KAZE;Database=dbJapaneseLearnSystem;TrustServerCertificate=True;User ID=abc;Password=123"�o��
//�O�o�H��n�ק�Program.cs���� �]�n�P�B�ק�o�䪺�s�u�r��A�N�O"dbJapaneseLearnSystemConnection"�A�o�ӭn��appsettings.json���s�u�r��W�٤@��

//_ViewStart.cshtml�n��bViews��Ƨ��̭����h�A���n��i�hShared��Ƨ��̭�
//��Ƥ�����檺�R�W�W�h�O��Ӹ�ƪ�W�٤����Ω��u�s���_�ӡA�M���Ӹ�ƪ�W�ٳ��n�νƼƧΦ�
//��Ƥ�����nScaffold���ܭn�`�N�A�]���L�S����k��ʥh�ק�A�ҥH�n�h�إߤ@�Ӫťժ����������ܦ�����A�o��Scaffold�~�|�����ʾާ@�o�Ӫ��