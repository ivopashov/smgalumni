using AutoMapper;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Interfaces;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    [Authorize(Roles = "Admin, MasterAdmin")]
    public class AdminController : BaseApiController
    {
        public AdminController(ILogger logger, IUserRepository userRepository)
            : base(logger, userRepository)
        {
        }

        [HttpGet]
        [Route("api/admin/unverifiedusers")]
        public IHttpActionResult GetUnverifiedUsers()
        {
            var vm = new List<UserForVerifyViewModel>();
            try
            {
                var unverifiedUsers = _userRepository.UnVerifiedUsers();
                if (unverifiedUsers.Any())
                {
                    vm = Mapper.Map<List<UserForVerifyViewModel>>(unverifiedUsers);
                }
                return Ok(vm);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Възникна грешка с колекцията с неверифицирани потребители. Моля опитайте отново");
            }
        }

        [HttpPost]
        [Route("api/admin/verifyusers")]
        public IHttpActionResult VerifyUsers(UserIdsToVerifyViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");
            if (vm.IdsToVerify.Count == 0) return Ok();
            //put everything in a transaction so that if anything fails nothing goes through

            User user;
            try
            {
                foreach (var item in vm.IdsToVerify)
                {
                    user = _userRepository.GetById(item);
                    if (user == null) throw new Exception("Tried to verify user with id: " + item + " but user does not exist");
                    user.Verified = true;
                    _userRepository.Update(user);
                    DomainEvents.Raise(new VerifyUserEvent() { UserName = user.UserName, User = CurrentUser });
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + "\nInner Exception: " + e.InnerException);
                return BadRequest("Възникна грешка и операцията не беше изпълнена. Моля опитайте отново");
            }
        }
    }
}


