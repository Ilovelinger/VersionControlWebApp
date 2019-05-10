using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    /// <summary>
    /// This is the home controller of the application,for managing homepage actions.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// HttpGet method for homepage
        /// </summary>
        /// <returns>View</returns>
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// HttpGet method for about page
        /// </summary>
        /// <returns>View</returns>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// HttpGet method for contact
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Contact()
        {

            return View();
        }

        /// <summary>
        /// HttpGet method for privacy page
        /// </summary>
        /// <returns>View</returns>
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
