using SmgAlumni.App.Models;
using SmgAlumni.Utils.EmailQuerer;
using SmgAlumni.Utils.EmailQuerer.Serialization;
using SmgAlumni.Utils.EmailQuerer.Templates;
using SmgAlumni.Utils.Membership;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmgAlumni.EF.Models;
using AutoMapper;

namespace SmgAlumni.App.Api
{
    public class AccountController : BaseApiController
    {
        private readonly EFUserManager _userManager;
        private readonly NotificationSender _sender;

        public AccountController(EFUserManager userManager, NotificationSender sender,Logger logger):base(logger)
        {
            _userManager = userManager;
            VerifyNotNull(_userManager);
            _sender = sender;
            VerifyNotNull(_sender);
        }

        [Route("api/account/register")]
        public IHttpActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invlid Model");
            }
            try
            {
                var getUserByEmail = _userManager.GetUserByEmail(model.Email);
                if (getUserByEmail != null)
                {
                    return BadRequest("User Already Exists");
                }

                var user = Mapper.Map<User>(model);

                _userManager.CreateUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/account/forgotpassword")]
        public IHttpActionResult ForgotPassword(AccountForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Входните данни не са валидни");
            }
            //check that user is valid
            var user = _userManager.GetUserByEmail(model.Email);
            if (user == null)
            {
                return Ok("Ако сте въвели правилен и-мейл трябва да получите съобщение с инструкции как да възстановите паролата си");
            }
            try
            {
                GenerateResetPassRequest(user);
                return Ok("Ако сте въвели правилен и-мейл трябва да получите съобщение с инструкции как да възстановите паролата си");
            }
            catch (Exception ex)
            {
                return BadRequest("Възникна грешка. Моля опитайте отново.");
            }
        }

        private void GenerateResetPassRequest(User user)
        {
            var guid = Guid.NewGuid();
            if (_userManager.AddResetPassRequest(user, guid))
            {
                var urlBuilder = new UriBuilder(Request.RequestUri.AbsoluteUri)
                {
                    Path = Url.Link("ResetPassword", "Account")
                };
                string ff = Request.RequestUri.AbsoluteUri;
                string path = urlBuilder.ToString();
                string link = path + "/#/account/resetpassword" + "?guid=" + guid + "&email=" + user.Email;
                WriteResetPassLinkToMsmq(link, user.Email, user.UserName);
            }
        }

        private void WriteResetPassLinkToMsmq(string link, string email, string username)
        {
            if (!string.IsNullOrEmpty(email))
            {
                _sender.SendEmailNotification(new EmailNotificationOptions
                {
                    To = email,
                    Template = new ResetPasswordTemplate
                    {
                        UserName = username,
                        Link = link
                    }
                });
            }
        }

        [Route("api/account/resetpassword")]
        public IHttpActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var user = _userManager.GetUserByEmail(model.Email);

            if (!_userManager.IsPasswordResetTokenValid(user, model.Token))
            {
                return BadRequest("Искането за възстановяване на парола е невалидно или изгубило давност. Моля опитайте отново.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Невалидни входни данни.");
            }

            try
            {
                var passwordReset = new PasswordReset()
                {
                    Guid = model.Token

                };
                _userManager.ResetPasswordBasedOnToken(user, passwordReset, model.Password);
                return Ok("Вашата парола беше сменена успешно. Сега може да се автентикирате с новите credentials.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest("Възникна грешка. Моля опитайте отново.");
            }
        }

        [HttpGet]
        [Route("api/account/checkguid")]
        public IHttpActionResult CheckGuid(string guid, string email)
        {
            if ((guid != null) && (email != null))
            {
                var user = _userManager.GetUserByEmail(email);
                //check validity
                var g = new Guid(guid);
                if (!_userManager.IsPasswordResetTokenValid(user, g))
                {
                    return BadRequest("Искането за възстановяване на парола е невалидно или изгубило давност. Моля опитайте отново.");
                }
            }
            return Ok();
        }
        
        //[Route("api/manage/data")]
        //public IHttpActionResult GetManageData()
        //{
        //    if (CurrentUser == null) return Ok();
        //    var model = new Models.AccountManageViewModel
        //    {
        //        Email = CurrentUser.Email,
        //        Username = CurrentUser.UserName
        //    };
        //    return Ok(model);
        //}

        //[HttpPost]
        //[Route("api/account/saveaccountmanage")]
        //public IHttpActionResult SaveAccountManage(Models.AccountManageViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("error", "Entered incorrect data.");
        //        return BadRequest(ModelState);
        //    }
        //    try
        //    {
        //        var user = MongoSession.Users.Where(x => x.Email.Equals(model.Email)).SingleOrDefault();
                
        //        if(user!=null)
        //        {
        //            user.Email = model.Email;
        //            user.UserName = model.Username;
        //            try
        //            {
        //                MongoSession.Users.Update(user);
        //            }
        //            catch (Exception e)
        //            {
        //                return BadRequest("Could not update user");
        //            }
        //            return Ok(model);
        //        }
        //        return BadRequest("User was not found");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("error", "There was an error attempting to reset your password (" + ex.Message + ").");
        //        return BadRequest(ModelState);
        //    }
        //}

        [Route("api/account/changepassword")]
        public IHttpActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Невалидни входни данни");
            }
            if (!_userManager.ValidatePassword(CurrentUser, model.OldPassword))
            {
                return BadRequest("Старата парола е грешна");
            }
            try
            {
                _userManager.ChangePassword(CurrentUser.UserName, model.NewPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Възникна грешка при смяна на Вашата парола. Моля опитайте отново");
            }
        }

        [HttpGet]
        [Route("api/acocunt/useraccount")]
        public IHttpActionResult GetUserAccount()
        {
            var id = CurrentUser.Id;
            var user = _userManager.GetUserById(id);
            var vm = Mapper.Map<UserAccountViewModel>(user);
            return Ok(vm);
        }
    }
}
