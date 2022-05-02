using CalendarManagementApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Net.Mail;
using System.Net;
using Dapper;
using System.Drawing;
using MovieApp.Repositories;
using MovieApp.Models;

namespace IdentityDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //checking the state of model passed as parameter
            if (ModelState.IsValid)
            {
                //check to see if the account exists
                var user = _repository.LoginUser(model);

                if (user != null && user.Password == model.Password)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(
                                              CookieAuthenticationDefaults.AuthenticationScheme,
                                              new ClaimsPrincipal(identity),
                                              new AuthenticationProperties
                                              {
                                                  IsPersistent = false   //remember me
                                              });

                    //if user is valid & present in database redirecting it to welcome page
                    return RedirectToAction("Index", "Home");
                }
            }
            //if model state is not valid, the model with error message is returned to the view
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<UserModel>();
                //create the user object
                var user = new UserModel
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = hasher.HashPassword(null, " "),
                    Role = model.Role
                };

                // create the User
                var result = _repository.RegisterUser(user);

                //If it's successful we redirect to sucessfully created profile
                if (result)
                {
                    return View("CreatedProfile");
                }
                else              //if model state is not valid, the model with error message is returned to the view
                {
                    return View(new ErrorViewModel { });
                }
            }
            return View(model);
        }

        public ActionResult SignOutAsync()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            ViewData["errormessage"] = "You are not allowed to access this page!";

            return View();
        }

        // GET: /Account/UpdateUser
        public IActionResult UpdateUser()
        {
            return View();
        }

        //POST: /Account/UpdateUser
        [HttpPost]
        public ActionResult UpdateUser(UserModel model)
        {
            var message = "";

            var claimsIdentity = User?.Identity as ClaimsIdentity;

            model.Email = claimsIdentity?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

            var update = _repository.UpdateUser(model);

            if (update != null)
            {
                //message = "Profile updated successfully";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //message = "Something failed";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                Token = token,
                UserId = userId
            };

            return View();
        }

        public ActionResult ResetPassword(ResetPasswordModel model, string email, string password, string confirmNewPassword)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var user = _repository.ResetPassword(model);

                if (user != null)
                {
                    message = "New password updated successfully";
                }
            }
            else
            {
                message = "Somehting failed";
            }
            ViewBag.Message = message;
            return View(model);
        }

        // GET: /Manage/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model, string currentPassword)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User?.Identity as ClaimsIdentity;

                model.Email = claimsIdentity?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;

                var user = _repository.ChangePassword(model);

                //user.Parameters.AddWithValue("@CurrentPassword", ChangePassword1.CurrentPassword);

                return View("ChangePasswordConfirmation");
            }
            return View(model);
        }

        [HttpPost]
        public void SendVerificationLinkEmail(string email, ForgotPasswordModel model)
        {
            string token = Guid.NewGuid().ToString();
            var fromEmail = new MailAddress("*******", "Calendar Application");
            var toEmail = new MailAddress(email);
            var link = "http://localhost:52284/Account/ResetPassword?token=" + token;
            var fromEmailPassword = "*******";

            string subject = "Reset Password";
            string body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                             "<br/><br/><a href=" + link + ">Reset Password link</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp-mail.outlook.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            smtp.Send(message);

        }
    }
}