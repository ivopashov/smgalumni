using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using NLog;
using SmgAlumni.App.Logging;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using SmgAlumni.EF.Models;
using SmgAlumni.Utils.DomainEvents;
using SmgAlumni.Utils.DomainEvents.Interfaces;
using SmgAlumni.Utils.Membership;

namespace SmgAlumni.App.Api
{
    [Authorize(Roles = "Admin, MasterAdmin")]
    public class AdminController : BaseApiController
    {

        private readonly RoleRepository _roleRepository;
        private readonly NewsRepository _newsRepository;
        private readonly CauseRepository _causeRepository;
        private readonly EFUserManager _userManager;

        public AdminController(ILogger logger,
            RoleRepository roleRepository,
            NewsRepository newsRepository,
            CauseRepository causeRepository,
            EFUserManager userManager)
            : base(logger)
        {
            _roleRepository = roleRepository;
            VerifyNotNull(_roleRepository);
            _newsRepository = newsRepository;
            VerifyNotNull(_newsRepository);
            _causeRepository = causeRepository;
            VerifyNotNull(_causeRepository);
            _userManager = userManager;
            VerifyNotNull(_userManager);
        }

        [HttpGet]
        [Route("api/admin/unverifiedusers")]
        public IHttpActionResult GetUnverifiedUsers()
        {
            var vm = new List<UserForVerifyViewModel>();
            try
            {
                var unverifiedUsers = Users.GetAll().Where(a => !a.Verified).ToList();
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
                    user = Users.GetById(item);
                    if (user == null) throw new Exception("Tried to verify user with id: " + item + " but user does not exist");
                    user.Verified = true;
                    Users.Update(user);
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


