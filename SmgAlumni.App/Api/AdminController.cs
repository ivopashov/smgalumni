using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmgAlumni.App.Api
{
    [Authorize(Roles="Admin,MasterAdmin")]
    public class AdminController : BaseApiController
    {

        private readonly UserRepository _userRepository;

        public AdminController(Logger logger, UserRepository userRepository):base(logger)
        {
            _userRepository = userRepository;
            VerifyNotNull(_userRepository);
        }

        [HttpGet]
        [Route("api/admin/unverifiedusers")]
        public IHttpActionResult GetUnverifiedUsers()
        {
            var vm = new List<UserForVerifyViewModel>();
            try
            {
                var unverifiedUsers = _userRepository.GetAll().Where(a => !a.Verified).ToList();
                if (unverifiedUsers.Any())
                {
                    vm = AutoMapper.Mapper.Map<List<UserForVerifyViewModel>>(unverifiedUsers);
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
            using (var context = new SmgAlumniContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in vm.IdsToVerify)
                        {
                            var user = context.Users.SingleOrDefault(a => a.Id == item);
                            if (user == null) throw new Exception("Tried to verify user with id: "+item+" but user does not exist");
                            user.Verified = true;
                        }
                        context.SaveChanges();

                        transaction.Commit();

                        return Ok();
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message);
                        transaction.Rollback();
                        return BadRequest("Възникна грешка и операцията не беше изпълнена. Моля опитайте отново");
                    }
                }
            }
        }
    }
}
