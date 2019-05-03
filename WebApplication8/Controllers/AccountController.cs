using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Data;
using WebApplication8.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication5.Controllers
{   
    /// <summary>
    /// This is the Account controller of the application, for managing user login and register.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signinManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register a user, store the information to the database and assign he/she a role.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm,IFormFile file)
        {
            //ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                //Take the email address input as user name and email
                var user = new ApplicationUser { UserName = vm.Email, Email = vm.Email,Nickname = vm.NickName, Firstname = vm.Firstname, Surname = vm.Surname,FullName = vm.FullName,Position = vm.Position,MobilePhoneNumber = vm.MobilePhoneNumber,KitNumber = vm.KitNumber };
                user.isRegistered = "No";


                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    user.AvatarImage = memoryStream.ToArray();

                }

                //Save the password input as the password of the user's account 
                var result = await _userManager.CreateAsync(user, vm.Password);

                if (result.Succeeded)
                {
                    if (vm.UserRole == "Player")
                    {
                        //Assign a role to the user
                        await _userManager.AddToRoleAsync(user,"Player");
                    }
                    else if (vm.UserRole == "CommonUser")
                    {
                        //Assign a role to the user
                        await _userManager.AddToRoleAsync(user, "CommonUser");
                    }
                    await _signinManager.SignInAsync(user, false);
                    return RedirectToAction("DoWork", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Compare login information with user information from database, if matches then login.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //Check if the login information input matches with information in the database
                var result = await _signinManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid Login Attemt");
                return View(vm);

            }
            return View(vm);
        }

        public async Task<ActionResult> DoWork() 

        {

            if (User.Identity.IsAuthenticated)
            {
                var userid = _userManager.GetUserId(HttpContext.User);
                ApplicationUser user = _userManager.FindByIdAsync(userid).Result;

                HttpContext.Session.SetString("tempUserName", user.Nickname);

                HttpContext.Session.SetString("isRegistered", user.isRegistered);

                HttpContext.Session.SetString("tempUserId", user.Id);


            }
            //_logger.LogInformation("User logged in.");
            return RedirectToAction("Index", "Home");


        }
    }
}
