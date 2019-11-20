using BetterAnimeList.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterAnimeList.Classes
{
    [ViewComponent(Name = "Login")]
    public class LoginViewComponent : ViewComponent
    {
        LoginViewModel _loginViewModel;
        public LoginViewComponent(LoginViewModel loginViewModel)
        {
            _loginViewModel = loginViewModel;
        }

        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            return View("_login", _loginViewModel);
        }

    }
}
