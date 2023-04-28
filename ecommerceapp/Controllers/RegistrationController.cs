using ecommerceapp.Data;
using ecommerceapp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using ecommerceapp.ViewModels.Registration;
using Org.BouncyCastle.Crypto.Generators;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ecommerceapp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegistrationController> _logger;

        private object returnUrl;

        private readonly RegistrationViewModel _viewModel;

        public RegistrationController(
            ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegistrationController> logger

            )
        {
            _viewModel = new RegistrationViewModel(dbContext);

            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _logger = logger;
        }



        public IActionResult Index()
        {
            return View();
        }

        //Index is login page and newIndexAsync is post Data of login page 
        [HttpPost]
        public async Task<IActionResult> newIndexAsync(Register obj)
        {
            // Find user by email
            //var user = _viewModel.GetRegisterByEmail(obj.Email);

            //if (user != null)
            //{
            //if (user.Email == "imran.chughtai77@gmail.com")
            //{
            //    var hasher2 = new PasswordHasher<Register>();
            //    var result2 = hasher2.VerifyHashedPassword(user, user.PasswordHash, obj.Password);

            //    if (result2 == PasswordVerificationResult.Success)
            //    {
            //        // Set session values
            //        HttpContext.Session.SetString("AdminLoggedIn", "true");
            //        HttpContext.Session.SetString("UserEmail", user.Email);
            //        HttpContext.Session.SetString("UserName", user.Name);
            //        HttpContext.Session.SetString("UserAddress", user.Address);
            //        HttpContext.Session.SetString("UserPhone", user.PhoneNumber);

            //        var userId = await _userManager.GetUserIdAsync(user);
            //        HttpContext.Session.SetString("UserId", userId);

            //        return RedirectToAction("Index", "View");
            //    }
            //}
            // Verify password

            // Find user by email
            var user = _viewModel.GetRegisterByEmail(obj.Email);

            if (user != null)
            {
                //Login As an Admin
                if (user.Email == "imran.chughtai77@gmail.com")
                {
                    var hasher2 = new PasswordHasher<Register>();
                    var result2 = hasher2.VerifyHashedPassword(user, user.PasswordHash, obj.Password);

                    if (result2 == PasswordVerificationResult.Success)
                    {
                        // Set session values
                        HttpContext.Session.SetString("AdminLoggedIn", "true");
                        HttpContext.Session.SetString("UserEmail", user.Email);
                        HttpContext.Session.SetString("UserName", user.Name);
                        HttpContext.Session.SetString("UserAddress", user.Address);
                        HttpContext.Session.SetString("UserPhone", user.PhoneNumber);

                        var userId = await _userManager.GetUserIdAsync(user);
                        HttpContext.Session.SetString("UserId", userId);

                        return RedirectToAction("Index", "View");
                    }
                }

//Login user 
                var hasher = new PasswordHasher<Register>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, obj.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    // Set session values
                    HttpContext.Session.SetString("IsLoggedIn", "true");
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserAddress", user.Address);
                    HttpContext.Session.SetString("UserPhone", user.PhoneNumber);

                    var userId = await _userManager.GetUserIdAsync(user);
                    HttpContext.Session.SetString("UserId", userId);

                    return RedirectToAction("Index", "View");
                }
            }
            TempData["erroremail"] = "Enter Valid Email";
            TempData["errorpass"] = "Enter Valid Password";

            return RedirectToAction("Index");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(Register obj)
        {
            //if (obj.Name == null) {
            //    TempData["erroremail"] = "Email is Null";

            //}

            if (obj.Email == null || obj.PasswordHash == null)
            {
                TempData["erroremail"] = "Please Enter Email & Password";
                return View(obj);
            }

            if (obj.Id == 0)
                {
                //_db.Users.Add(obj);
                //_db.SaveChanges();
                var user = _viewModel.GetFindByPhoneno(obj.PhoneNumber);

                if (user == null)
                {
                    var hasher = new PasswordHasher<Register>();
                    obj.PasswordHash = hasher.HashPassword(obj, obj.PasswordHash);

                    await _viewModel.AddRegisterAsync(obj);
                    return RedirectToAction("Index");

                }
                TempData["errorexist"] = "User Already Exist";
                return View();
            }
            //TempData["errorpass"] = "Password Is Null";
            return View(obj);
        }

        public IActionResult Forgotpassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(Register obj)
        {
            //var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.Equals(obj.Email));
            var user = await _viewModel.GetUserByEmailAsync(obj.Email);

            var userId = await _userManager.GetUserIdAsync(user);
            
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Action(
           "Newpassword",
           "Registration",
           new { user = user.Email,  userId = userId, code = code, returnUrl = Url.Content("~/") },
           protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(obj.Email, "Change Pasword Email",
                $"<strong>Please <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>click here</a> to reset your password.</strong>");

            return RedirectToAction("Index");
        }
    
        public IActionResult Newpassword(string user, string userId, string code, string returnUrl)
        {
            var model = new Register { Email = user };
            return View(model);
        }

        public RegistrationViewModel Get_viewModel()
        {
            return _viewModel;
        }

        [HttpPost]
        public IActionResult Newpassword(Register obj)
        {
            //var user = _db.Registers.FirstOrDefault(e => e.Email == obj.Email);
            var user = _viewModel.GetRegisterByEmail(obj.Email);

            var hasher = new PasswordHasher<Register>();
            obj.PasswordHash = hasher.HashPassword(obj, obj.PasswordHash);

            if (user != null)
            {
                user.PasswordHash = obj.PasswordHash;
                // _db.SaveChanges();
                _viewModel.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Registration");
        }
    }

}
