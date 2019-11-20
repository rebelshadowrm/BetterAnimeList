using BetterAnimeList.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterAnimeList.Classes
{
    [ViewComponent(Name = "Register")]
    public class RegisterViewComponent : ViewComponent
    {
        RegisterViewModel _registerViewModel;
        public RegisterViewComponent(RegisterViewModel registerViewModel)
        {
            _registerViewModel = registerViewModel;
        }

        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            return View("_register", _registerViewModel);
        }
    }
}
