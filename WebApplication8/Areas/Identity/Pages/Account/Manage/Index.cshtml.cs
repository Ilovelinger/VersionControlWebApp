using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication8.Models;

namespace WebApplication8.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            //[Phone]
            [Display(Name = "Phone number")]
            public int PhoneNumber { get; set; }

            [Required]
            public string Firstname { get; set; }

            [Required]
            public string Surname { get; set; }

            [Required]
            public string NickName { get; set; }

            [Required]
            public string FullName { get; set; }

            public int KitNumber { get; set; }

            public string Position { get; set; }

            public IFormFile AvatarImage { get; set; }

        }

        /// <summary>
        /// HttpGet method for updating user data
        /// </summary>
        /// <returns>Page</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            if (user.AvatarImage != null) {
                string temp_inBase64 = Convert.ToBase64String(user.AvatarImage);
                ViewData["MyPic"] = String.Format("data:image/jpeg;base64,{0}", temp_inBase64);
            }


            Input = new InputModel
            {
                Firstname = user.Firstname,
                Surname = user.Surname,
                NickName = user.Nickname,
                KitNumber = user.KitNumber,
                Position = user.Position,
                FullName = user.FullName,
                Email = email,
                PhoneNumber = user.MobilePhoneNumber,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        /// <summary>
        /// HttpPost method for updating user data.
        /// </summary>
        /// <returns>Redirct to page</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.Surname != user.Surname)
            {
                user.Surname = Input.Surname;
            }

            if (Input.Firstname != user.Firstname)
            {
                user.Firstname = Input.Firstname;
            }

            if (Input.NickName != user.Nickname)
            {
                user.Nickname = Input.NickName;
            }

            if (Input.FullName != user.FullName)
            {
                user.FullName = Input.FullName;
            }

            if (Input.KitNumber != user.KitNumber)
            {
                user.KitNumber = Input.KitNumber;
            }

            if (Input.Position != user.Position)
            {
                user.Position = Input.Position;
            }

            if (Input.PhoneNumber != user.MobilePhoneNumber)
            {
                user.MobilePhoneNumber = Input.PhoneNumber;
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        var userId = await _userManager.GetUserIdAsync(user);
            //        throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
            //    }
            //}

            //using (var memoryStream = new MemoryStream())
            //{
            //    await uploadfile.CopyToAsync(memoryStream);
            //    user.AvatarImage = memoryStream.ToArray();
            //}
        }
    }
}
