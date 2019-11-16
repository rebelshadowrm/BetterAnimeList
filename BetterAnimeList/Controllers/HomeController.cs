using BetterAnimeList.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BetterAnimeList.Controllers
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

        public IActionResult UserList()
        {

            AnimeDBContext context = HttpContext.RequestServices.GetService(typeof(AnimeDBContext)) as AnimeDBContext;

            var model = new UserListViewModel(context.GetAllMembers());


            return View(model);
        }

        /// capi.net/Home/AnimeList?UserID=2
        /// capi.net/Home/AnimeList?Username=rebelshadowrm
        [HttpGet]
        public IActionResult AnimeList(int? UserID, string? Username)
        {
            AnimeDBContext context = HttpContext.RequestServices.GetService(typeof(AnimeDBContext)) as AnimeDBContext;

            var SentUserID = UserID.GetValueOrDefault();
            
            var model = new AnimeListViewModel();
            if (UserID.HasValue)
            {
                model = context.GetUserAnimelist(SentUserID);

                return View(model);
            }
            
            if (!string.IsNullOrEmpty(Username))
            {
                model = context.GetUserAnimelistByUsername(Username);

                return View(model);
            }

            var LoggedInUser = HttpContext.Session.GetString("username");

            if (!string.IsNullOrEmpty(LoggedInUser))
            {
                model = context.GetUserAnimelistByUsername(LoggedInUser);

                return View(model);
            }

            return View(model);

        }





        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }




        public IActionResult Login()
        {
            var model = new LoginViewModel();

            return View(model);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return RedirectToAction("Privacy", "Home");
        }

        public IActionResult UploadTest()
        {
            return View();
        }
    }
}
