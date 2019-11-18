using BetterAnimeList.Classes;
using BetterAnimeList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace BetterAnimeList.Controllers
{
    public class FormController : Controller
    {

        [HttpPost]
        public IActionResult Register(RegisterViewModel ModelSent)
        {
            if (ModelSent.Username != null && ModelState.IsValid)
            {
                AnimeDBContext context = HttpContext.RequestServices.GetService(typeof(AnimeDBContext)) as AnimeDBContext;

                var member = context.register(ModelSent.Username, ModelSent.Password);


                return RedirectToAction("Login", "Home");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        //[HttpPost]
        //public IActionResult Register(string username, string password, string repeatpassword)
        //{
        //    if (username.Length >= 3 && username.Length <= 24 && password = repeatpassword && password.length > 5 && username.regex)
        //    {

        //    }
        //    AnimeDBContext context = HttpContext.RequestServices.GetService(typeof(AnimeDBContext)) as AnimeDBContext;

        //    if (password == repeatpassword)
        //    {
        //        var model = context.register(username, password);
        //    }
        //    else
        //    {
        //        return Redirect(Request.Headers["Referer"].ToString());
        //    }

        //    return RedirectToAction("Index", "Home");
        //}

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            AnimeDBContext context = HttpContext.RequestServices.GetService(typeof(AnimeDBContext)) as AnimeDBContext;

            var member = context.login(username, password);
            if (member != null)
            {
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetInt32("userid", member.Userid);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }



            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public JsonResult AnimeUpload(string Filename)
        {
            return Json("hello");
        }

        private bool ConvertXmlToCsv(string FilePath)
        {
            try
            {
                var CSVPath = Path.GetDirectoryName(FilePath) + '\\' + Path.GetFileNameWithoutExtension(FilePath) + ".csv";
                var XMLPath = Path.GetDirectoryName(FilePath) + '\\' + Path.GetFileNameWithoutExtension(FilePath) + ".tmp";
                var Tag = "anime";

                XMLtoCSV.Convert(XMLPath, CSVPath, Tag, XMLtoCSV.DataArrange.Element, XMLtoCSV.RowDelimit.NewLine, XMLtoCSV.ColumnDelimit.OrSymbol);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void AddColumn(string FilePath, string ColumnName, string Value)
        {
            var csv = System.IO.File.ReadLines(FilePath) // not AllLines
              .Select((line, index) => index == 0
                 ? line + ColumnName + "|"
                 : line + Value + "|")
              .ToList(); // we should write into the same file, that´s why we materialize

            System.IO.File.WriteAllLines(FilePath, csv);
        }

        [HttpPost("/Form/OnPostUploadAsync")]
        public JsonResult OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            string filePath = "";
            var SuccessMsg = "";

            foreach (var formFile in files)
            {
                if (formFile.Length > 0 && formFile.ContentType == "text/xml")
                {
                    filePath = Path.GetTempFileName();

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        formFile.CopyTo(stream);
                    }

                    SuccessMsg = ConvertXmlToCsv(filePath) ? "Horray!" : "Conversion Failed.";

                    var CSVPath = Path.GetDirectoryName(filePath) + '\\' + Path.GetFileNameWithoutExtension(filePath) + ".csv";

                    AddColumn(CSVPath, "user_id", Convert.ToString(HttpContext.Session.GetInt32("userid").GetValueOrDefault()));
                    AddColumn(CSVPath, "username", HttpContext.Session.GetString("username"));

                    AnimeDBContext context = HttpContext.RequestServices.GetService(typeof(BetterAnimeList.Models.AnimeDBContext)) as AnimeDBContext;

                    context.UploadCSV(CSVPath);
                }
                else
                {
                    SuccessMsg = "file not the correct format";
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Json(SuccessMsg);
        }
    }
}