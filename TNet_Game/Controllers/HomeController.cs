using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TNet.Runtime;
using TNet_Web.Models;

namespace TNet.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet()]
        public IActionResult PT()
        {
            var wr = string.Format(GameZone.char_d,
                     Assembly.GetExecutingAssembly().GetName().FullName.Split(',')[0],
                       Environment.Is64BitProcess ? 64 : 86,
                     Environment.OSVersion.Platform,
                     Assembly.GetExecutingAssembly().GetName().FullName.Split(',')[1],
                     5317,
                     2017);
            
            return Content(wr);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/{ab}")]
        public IActionResult Lx(string ab)
        {
           
         //var item=   Request.Query["a"];
            //            ViewData["Message"] = "Your application description page.";
            //

            return Json(ab);
            //return new ViewResult(item);
           // return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
