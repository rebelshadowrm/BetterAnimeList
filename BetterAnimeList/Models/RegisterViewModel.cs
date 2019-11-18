using BetterAnimeList.Classes;
using System.ComponentModel.DataAnnotations;

namespace BetterAnimeList.Models
{
    public class RegisterViewModel
    {


        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Username")]
        [StringLength(24, MinimumLength = 3)]
        [RegularExpression(@"^(?=.{3,24}$)(?:[a-zA-Z\d]+(?:(?:\.|-|_)[a-zA-Z\d])*)+$", ErrorMessage = "Length 3-24, can contain _.- non repetitively")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Length must be {1} or greater.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Repeat Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again!")]
        public string RepeatPassword { get; set; }

       
    }
}
