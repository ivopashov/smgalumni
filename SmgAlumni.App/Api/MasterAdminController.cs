using System;
using System.Linq;
using System.Web.Http;
using NLog;
using SmgAlumni.App.Models;
using SmgAlumni.Data.Repositories;
using SmgAlumni.EF.Models;

namespace SmgAlumni.App.Api
{
    [Authorize(Roles = "MasterAdmin")]
    public class MasterAdminController : BaseApiController
    {
        private readonly UserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly SettingRepository _settingRepository;

        public MasterAdminController(Logger logger, UserRepository userRepository, RoleRepository roleRepository, SettingRepository settingRepository)
            : base(logger)
        {
            _userRepository = userRepository;
            VerifyNotNull(_userRepository);
            _roleRepository = roleRepository;
            VerifyNotNull(_roleRepository);
            _settingRepository = settingRepository;
            VerifyNotNull(_settingRepository);
        }

        [HttpGet]
        [Route("api/masteradmin/getallroles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = _roleRepository.GetAll().Select(a => a.Name).Distinct().ToList();
            return Ok(roles);
        }

        [HttpPost]
        [Route("api/masteradmin/updateuserroles")]
        public IHttpActionResult UpdateRoles(UserAccountShortWithRolesViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невярни входни данни");
            try
            {
                var user = _userRepository.GetById(vm.Id);
                var deletedRoles = user.Roles.Where(a => !vm.Roles.Contains(a.Name)).ToList();
                var addedRoles = vm.Roles.Where(a => !user.Roles.Select(r => r.Name).Contains(a));

                foreach (var item in deletedRoles)
                {
                    user.Roles.Remove(item);
                }

                foreach (var item in addedRoles)
                {
                    user.Roles.Add(new Role() { Name = item });
                }

                _userRepository.Update(user);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Could not update user roles");
            }

        }

        [HttpGet]
        [Route("api/settings/getall")]
        public IHttpActionResult GetAllSettings()
        {
            var settings = _settingRepository.GetAll().ToList();
            return Ok(settings);
        }

        [HttpPost]
        [Route("api/settings/update")]
        public IHttpActionResult UpdateSetting(Setting vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            try
            {
                _settingRepository.Update(vm);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Настройките не можаха да бъдат обновени");
            }
        }

        [HttpPost]
        [Route("api/settings/delete")]
        public IHttpActionResult DeleteSetting(Setting vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            try
            {
                var setting = _settingRepository.GetById(vm.Id);
                _settingRepository.Delete(setting);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Настройката не можа да бъде изтрита");
            }
        }

        [HttpPost]
        [Route("api/settings/add")]
        public IHttpActionResult AddSetting(SettingViewModel vm)
        {
            if (!ModelState.IsValid) return BadRequest("Невалидни входни данни");

            try
            {
                _settingRepository.Add(new Setting()
                {
                    SettingKey = vm.SettingKey,
                    SettingName = vm.SettingValue
                });
                _settingRepository.Save();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return BadRequest("Настройките не можаха да бъдат обновени");
            }
        }
    }
}
