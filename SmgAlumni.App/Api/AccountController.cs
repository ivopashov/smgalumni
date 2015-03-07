using SmgAlumni.App.Models;
using Core.EmailQuerer;
using Core.EmailQuerer.Serialization;
using Core.EmailQuerer.Templates;
using Core.Membership;
using Mongo.Data;
using Mongo.Data.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class AccountController : BaseApiController
    {
        private readonly MongoUserManager _membership;
        private readonly MongoContext _context;
        private readonly NotificationSender _sender;

        public AccountController(MongoUserManager surveyMasterMembership, MongoContext context, NotificationSender sender,Logger logger):base(logger)
        {
            _membership = surveyMasterMembership;
            VerifyNotNull(_membership);
            _context = context;
            VerifyNotNull(_context);
            _sender = sender;
            VerifyNotNull(_sender);
        }

        [Route("api/account/register")]
        public IHttpActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Entered incorrect data.");
                return BadRequest(ModelState);
            }
            try
            {
                var getUserByEmail = _membership.GetUserByEmail(model.Email);
                if (getUserByEmail != null)
                {
                    ModelState.AddModelError("error", "User exists.");
                    return BadRequest(ModelState);
                }
                _membership.CreateUser(model.Username, model.Password, model.Email);
                return Ok("User was stored.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpGet]
        [Route("api/account/checkguid")]
        public IHttpActionResult CheckGuid(string guid, string email)
        {
            if ((guid != null) && (email != null))
            {
                var user = _membership.GetUserByEmail(email);
                //check validity
                var g = new Guid(guid);
                if (!_membership.IsPasswordResetTokenValid(user, g))
                {
                    ModelState.AddModelError("error", "Your password reset token is invalid or expired.");
                    return BadRequest(ModelState);
                }
            }
            return Ok();
        }
        [Route("api/account/forgotpassword")]
        public IHttpActionResult ForgotPassword(AccountForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Entered incorrect data.");
                return BadRequest(ModelState);
            }
            //check that user is valid
            var user = _membership.GetUserByEmail(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("error", "That email is not registered.");
                return BadRequest(ModelState);
            }
            try
            {
                GenerateResetPassRequest(user);
                return Ok("Please check your email and follow the instructions in order to reset your password.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", "An error occured while sending out your password reset email (" + ex.Message + ")");
                return BadRequest(ModelState);
            }
        }
        private void GenerateResetPassRequest(User user)
        {
            var guid = Guid.NewGuid();
            if (_membership.AddResetPassRequest(user, guid))
            {
                var urlBuilder = new UriBuilder(Request.RequestUri.AbsoluteUri)
                {
                    Path = Url.Link("ResetPassword", "Account")
                };
                string ff = Request.RequestUri.AbsoluteUri;
                string path = urlBuilder.ToString();
                string link = path+"/#/account/resetpassword" + "?guid=" + guid + "&email=" + user.Email;
                WriteResetPassLinkToMsmq(link, user.Email, user.UserName);
            }
        }

        [Route("api/account/resetpassword")]
        public IHttpActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var s = SmUser;
            var user = _membership.GetUserByEmail(model.Email);
            
            if (!_membership.IsPasswordResetTokenValid(user, model.Token))
            {
                ModelState.AddModelError("error", "Your password reset token is invalid or expired.");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Entered incorrect data.");
                return BadRequest(ModelState);
            }

            try
            {
                var passwordReset = new PasswordReset()
                {
                    Guid = model.Token

                };
                _membership.ResetPasswordBasedOnToken(user, passwordReset, model.Password);
                return Ok("Your password was successfully reset. You may now login using your credentials.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", "There was an error attempting to reset your password (" + ex.Message + ").");
                return BadRequest(ModelState);
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
        
        [Route("api/manage/data")]
        public IHttpActionResult GetManageData()
        {
            if (SmUser == null) return Ok();
            var model = new Models.AccountManageViewModel
            {
                Email = SmUser.Email,
                Username = SmUser.UserName
            };
            return Ok(model);
        }

        [HttpPost]
        [Route("api/account/saveaccountmanage")]
        public IHttpActionResult SaveAccountManage(Models.AccountManageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Entered incorrect data.");
                return BadRequest(ModelState);
            }
            try
            {
                var user = MongoSession.Users.Where(x => x.Email.Equals(model.Email)).SingleOrDefault();
                
                if(user!=null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Username;
                    try
                    {
                        MongoSession.Users.Update(user);
                    }
                    catch (Exception e)
                    {
                        return BadRequest("Could not update user");
                    }
                    return Ok(model);
                }
                return BadRequest("User was not found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", "There was an error attempting to reset your password (" + ex.Message + ").");
                return BadRequest(ModelState);
            }
        }

        [Route("api/account/changepassword")]
        public IHttpActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Entered incorrect data.");
                return BadRequest(ModelState);
            }
            if (!_membership.ValidatePassword(SmUser, model.OldPassword))
            {
                ModelState.AddModelError("error", "Your password was wrong.");
                return BadRequest(ModelState);
            }
            try
            {
                _membership.ChangePassword(SmUser.Id, model.NewPassword);
                return Ok("Your password was changed successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", "There was an error while changing your password (" + ex.Message + ").");
                return BadRequest(ModelState);
            }
        }
    }
}
