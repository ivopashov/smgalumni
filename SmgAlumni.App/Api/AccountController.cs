using System;
using System.Web.Http;
using AutoMapper;
using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.Helpers;
using SmgAlumni.Utils.Membership;

namespace SmgAlumni.App.Api
{
    public class AccountController : BaseApiController
    {
        private readonly EFUserManager _userManager;
        private readonly NotificationEnqueuer _sender;


        public AccountController(EFUserManager userManager, NotificationEnqueuer sender, Logger logger)
            : base(logger)
        {
            _userManager = userManager;
            VerifyNotNull(_userManager);
            _sender = sender;
            VerifyNotNull(_sender);
        }

        [AllowAnonymous]
        [Route("api/account/register")]
        public IHttpActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Входните данни не са валидни");
            }
            try
            {
                var getUserByEmail = _userManager.GetUserByEmail(model.Email);
                if (getUserByEmail != null)
                {
                    return BadRequest("Потребител с такъв имейл съществува");
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

        [AllowAnonymous]
        [Route("api/account/forgotpassword")]
        public IHttpActionResult ForgotPassword(AccountForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Входните данни не са валидни");
            }
            //check that user is valid
            try
            {
                DomainEvents.Raise<ForgotPasswordEvent>(new ForgotPasswordEvent() 
                {
                    RequestAuthority=Request.RequestUri.Authority,
                    RequestScheme=Request.RequestUri.Scheme,
                    Email=model.Email
                });
                return Ok("Ако сте въвели правилен и-мейл трябва да получите съобщение с инструкции как да възстановите паролата си");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest("Възникна грешка. Моля опитайте отново.");
            }
        }

        [AllowAnonymous]
        [Route("api/account/resetpassword")]
        public IHttpActionResult ResetPassword(ResetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Невалидни входни данни.");
            }

            var user = _userManager.GetUserByEmail(model.Email);

            if (!_userManager.IsPasswordResetTokenValid(user, model.Token))
            {
                return BadRequest("Искането за възстановяване на парола е невалидно или изгубило давност. Моля опитайте отново.");
            }

            try
            {
                _userManager.ResetPasswordBasedOnToken(user, model.Token, model.Password);
                return Ok("Вашата парола беше сменена успешно. Сега може да се автентикирате с новите credentials.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return BadRequest("Възникна грешка. Моля опитайте отново.");
            }
        }

        [HttpGet]
        [AllowAnonymous]
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
                    return Ok(false);
                }
                return Ok(true);
            }
            return BadRequest("Невалидни входни данни");
        }
        
        
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
        [Route("api/account/useraccount")]
        public IHttpActionResult GetUserAccount()
        {
            var id = CurrentUser.Id;
            var user = _userManager.GetUserById(id);
            if (user == null) return BadRequest("Потребителят не беше намерен. Моля опитайте отново");
            
            var vm = Mapper.Map<UserAccountViewModel>(user);
            return Ok(vm);
        }

        [Authorize(Roles="Admin, MasterAdmin")]
        [HttpGet,Route("api/account/delete")]
        public IHttpActionResult DeleteUser([FromUri] string username)
        {
            var user = _userManager.GetUserByUserName(username);
            if (user == null) return BadRequest("Потребителят не беше намерен");
            try
            {
                Users.Delete(user);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Възникна грешка. Моля опитайте отново");
            }
        }

        [Authorize(Roles = "Admin, MasterAdmin")]
        [HttpPost, Route("api/account/resetuserpass")]
        public IHttpActionResult ResetUserPass(ResetUserPassViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невярни входни данни");

            var user = _userManager.GetUserById(vm.Id);
            if (user == null) return BadRequest("Потребителят не беше намерен");

            var salt = Password.CreateSalt();
            user.Password = Password.HashPassword(vm.Password + salt);
            user.PasswordSalt = salt;
            
            try
            {
                Users.Update(user);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Възникна грешка. Моля опитайте отново");
            }
        }

        [HttpPost]
        [Route("api/account/updateuser")]
        public IHttpActionResult UpdateUser(UserAccountViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");
            try
            {
                CurrentUser.UserName = model.UserName;
                CurrentUser.FirstName = model.FirstName;
                CurrentUser.MiddleName = model.MiddleName;
                CurrentUser.LastName = model.LastName;
                CurrentUser.Email = model.Email;
                CurrentUser.Company = model.Company;
                CurrentUser.Description = model.Description;
                CurrentUser.DwellingCountry = model.DwellingCountry;
                CurrentUser.City = model.City;
                CurrentUser.Division = model.Division;
                CurrentUser.YearOfGraduation = model.YearOfGraduation;
                CurrentUser.UniversityGraduated = model.UniversityGraduated;
                CurrentUser.Profession = model.Profession;
                CurrentUser.PhoneNumber = model.PhoneNumber;

                Users.Update(CurrentUser);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Потребителските данни не можаха да бъдат обновени. Моля опитайте отново");
            }
        }
    }
}
