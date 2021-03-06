﻿using AutoMapper;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.ServiceLayer;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.Utils;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Models;
using SmgAlumni.Utils.EfEmailQuerer;
using SmgAlumni.Utils.Helpers;
using System;
using System.Web;
using System.Linq;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        public AccountController(IAccountService accountService, ILogger logger, IUserRepository userRepository, IUserService userService)
            : base(logger, userRepository)
        {
            _accountService = accountService;
            VerifyNotNull(_accountService);
            _userService = userService;
            VerifyNotNull(_userService);
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
                var getUserByEmail = _userRepository.UsersByEmail(model.Email).SingleOrDefault();
                if (getUserByEmail != null)
                {
                    return BadRequest("Потребител с такъв имейл съществува");
                }

                model.FirstName = HttpContext.Current.Server.HtmlEncode(model.FirstName);
                model.MiddleName = HttpContext.Current.Server.HtmlEncode(model.MiddleName);
                model.LastName= HttpContext.Current.Server.HtmlEncode(model.LastName);
                model.Username= HttpContext.Current.Server.HtmlEncode(model.Username);
                model.Email = HttpContext.Current.Server.HtmlEncode(model.Email);

                var user = Mapper.Map<User>(model);

                _userService.CreateUser(user);
                DomainEvents.Raise(new RegisterUserDomainEvent() { User = user, RegisteredOn = DateTime.Now });

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
                DomainEvents.Raise(new ForgotPasswordEvent()
                {
                    RequestAuthority = Request.RequestUri.Authority,
                    RequestScheme = Request.RequestUri.Scheme,
                    Email = model.Email
                });
                return Ok("Ако сте въвели правилен и-мейл трябва да получите съобщение с инструкции как да възстановите паролата си. Това обикновено става в рамките на 5 мин.");
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

            var user = _userRepository.UsersByEmail(model.Email).SingleOrDefault();
            if (user == null)
            {
                throw new NullReferenceException("User with email: " + model.Email + " could not be found");
            }

            if (!_accountService.IsPasswordResetTokenValid(user, model.Token))
            {
                return BadRequest("Искането за възстановяване на парола е невалидно или изгубило давност. Моля опитайте отново.");
            }

            try
            {
                _accountService.ResetPasswordBasedOnToken(user, model.Token, model.Password);
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
                var user = _userRepository.UsersByEmail(email).SingleOrDefault();
                if (user == null)
                {
                    throw new NullReferenceException("User with email: " + email + " could not be found");
                }

                //check validity
                var g = new Guid(guid);
                if (!_accountService.IsPasswordResetTokenValid(user, g))
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
            if (!_accountService.ValidatePassword(CurrentUser, model.OldPassword))
            {
                return BadRequest("Старата парола е грешна");
            }
            try
            {
                _accountService.ChangePassword(CurrentUser.UserName, model.NewPassword);
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
            var user = _userRepository.GetById(id);
            if (user == null)
            {
                return BadRequest("Потребителят не беше намерен. Моля опитайте отново");
            }

            var vm = Mapper.Map<UserAccountViewModel>(user);
            return Ok(vm);
        }

        [Authorize(Roles = "Admin, MasterAdmin")]
        [HttpGet, Route("api/account/delete")]
        public IHttpActionResult DeleteUser([FromUri] string username)
        {
            var user = _userRepository.UsersByUserName(username).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("Потребителят не беше намерен");
            }

            try
            {
                _userRepository.Delete(user);
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

            var user = _userRepository.GetById(vm.Id);
            if (user == null)
            {
                return BadRequest("Потребителят не беше намерен");
            }

            var salt = Password.CreateSalt();
            user.Password = Password.HashPassword(vm.Password + salt);
            user.PasswordSalt = salt;

            try
            {
                _userRepository.Update(user);
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
                CurrentUser.UserName = HttpContext.Current.Server.HtmlEncode(model.UserName);
                CurrentUser.FirstName = HttpContext.Current.Server.HtmlEncode(model.FirstName);
                CurrentUser.MiddleName = HttpContext.Current.Server.HtmlEncode(model.MiddleName);
                CurrentUser.LastName = HttpContext.Current.Server.HtmlEncode(model.LastName);
                CurrentUser.Email = HttpContext.Current.Server.HtmlEncode(model.Email);
                CurrentUser.Company = HttpContext.Current.Server.HtmlEncode(model.Company);
                CurrentUser.Description = HttpContext.Current.Server.HtmlEncode(model.Description);
                CurrentUser.DwellingCountry = HttpContext.Current.Server.HtmlEncode(model.DwellingCountry);
                CurrentUser.City = HttpContext.Current.Server.HtmlEncode(model.City);
                CurrentUser.Division = model.Division;
                CurrentUser.YearOfGraduation = model.YearOfGraduation;
                CurrentUser.UniversityGraduated = HttpContext.Current.Server.HtmlEncode(model.UniversityGraduated);
                CurrentUser.Profession = HttpContext.Current.Server.HtmlEncode(model.Profession);
                CurrentUser.PhoneNumber = HttpContext.Current.Server.HtmlEncode(model.PhoneNumber);

                _userRepository.Update(CurrentUser);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Потребителските данни не можаха да бъдат обновени. Моля опитайте отново");
            }
        }
    }
}
