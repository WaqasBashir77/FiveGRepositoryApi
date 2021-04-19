using FiveGApi.DTOModels;
using FiveGApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace FiveGApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/UserManage")]
    public class UserManageController : ApiController
    {
        private string UserId;
        public UserManageController()
        {
            UserId = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

        }
        private MIS_DBEntities1 db = new MIS_DBEntities1();

        [Route("CreateUser")]
        [HttpPost]
        public IHttpActionResult CreateUser(UserDTO _userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UserExisted = db.Users.Where(x => x.UserName == _userDTO.UserName).FirstOrDefault();
            if (UserExisted != null)
            {
                return Conflict();
            }
            else
            {
                try
                {
                    User newUser = new User();
                    newUser.UserName = _userDTO.UserName;
                    newUser.Password = _userDTO.Password;
                    newUser.OldPassword = _userDTO.Password;
                    newUser.NewPassword = _userDTO.Password;
                    newUser.ConfirmPassword = _userDTO.Password;
                    newUser.Email = _userDTO.Email;
                    newUser.FirstName = _userDTO.FirstName;
                    newUser.LastName = _userDTO.LastName;
                    newUser.SecurityGroupId = _userDTO.SecurityGroupId;
                    newUser.IsActive = true;
                    newUser.IsDeleted = false;
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    RolesToUser rolesToUser = new RolesToUser();
                    rolesToUser.UserId = newUser.UserId;
                    rolesToUser.RoleId = _userDTO.RoleID;
                    rolesToUser.CreatedBy = "1";
                    rolesToUser.CreatedDate = DateTime.Now;
                    db.RolesToUsers.Add(rolesToUser);
                    db.SaveChanges();
                    return Ok(_userDTO.UserName);
                }
                catch (Exception ex)
                {
                    return Ok(ex);
                }
            }

        }
        [Route("UpdateUser")]
        [HttpPut]
        public IHttpActionResult UpdateUser(int id, UserDTO _userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UserExisted = db.Users.Where(x => x.UserId == id ).FirstOrDefault();
            if (UserExisted == null)
            {
                return NotFound();
            }
            else
            {

                UserExisted.UserName = _userDTO.UserName;
                UserExisted.Password = _userDTO.Password;
                UserExisted.NewPassword = _userDTO.Password;
                UserExisted.OldPassword = _userDTO.Password;
                UserExisted.ConfirmPassword = _userDTO.Password;
                UserExisted.Email = _userDTO.Email;
                UserExisted.FirstName = _userDTO.FirstName;
                UserExisted.LastName = _userDTO.LastName;
                UserExisted.SecurityGroupId = _userDTO.SecurityGroupId;
                UserExisted.IsActive = _userDTO.IsActive;
                UserExisted.IsDeleted = _userDTO.IsDeleted;
                UserExisted.LastModifiedBy = 1;
                UserExisted.LastModifiedDate = DateTime.Now;
                db.SaveChanges();

                var usertoroles = db.RolesToUsers.Where(x => x.UserId == UserExisted.UserId).FirstOrDefault();

                //usertoroles.UserId = newUser.UserId;
                usertoroles.RoleId = _userDTO.RoleID;
                usertoroles.CreatedBy = "1";
                usertoroles.CreatedDate = DateTime.Now;
                //db.RolesToUsers.Add(rolesToUser);
                db.SaveChanges();
                return Ok(_userDTO.UserName);
            }

        }
        [HttpGet]
        [Route("GetAllUsersList")]
        public IHttpActionResult GetAllUsersList()
        {
            var userList = (from u in db.Users
                            join ru in db.RolesToUsers on u.UserId equals ru.UserId
                            join r in db.Roles on ru.RoleId equals r.Id
                            //where u.UserId == RoleId
                            //&& f.IsMenuItem == true
                            select new UserDTO
                            {
                                UserID = u.UserId,
                                UserName = u.UserName,
                                Password = null,
                                NewPassword = null,
                                OldPassword = null,
                                ConfirmPassword = null,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Email = u.Email,
                                RoleID = ru.RoleId,
                                RoleName = r.Name,
                                SecurityGroupId = u.SecurityGroupId,
                                IsActive= (bool)u.IsActive,
                                IsDeleted=(bool)u.IsDeleted
                            })
                .OrderByDescending(x => x.UserID).AsQueryable()
               .ToList();
            return Ok(userList);
        }
        [HttpGet]
        [Route("GetUsersByID")]
        public IHttpActionResult GetUsersByID(int userID)
        {
            var user = db.Users.Where(x => x.UserId == userID).AsQueryable().FirstOrDefault();
            if (user != null)
            {
                UserDTO userDTO = new UserDTO();
                userDTO.UserID = user.UserId;
                userDTO.UserName = user.UserName;
                userDTO.Password = user.Password;
                userDTO.NewPassword = user.NewPassword;
                userDTO.OldPassword = user.OldPassword;
                userDTO.ConfirmPassword = user.ConfirmPassword;
                userDTO.Email = user.Email;
                userDTO.FirstName = user.FirstName;
                userDTO.LastName = user.LastName;
                userDTO.SecurityGroupId = user.SecurityGroupId;
                userDTO.RoleID = db.RolesToUsers.Where(x => x.UserId == user.UserId).Select(x => x.RoleId).AsQueryable().FirstOrDefault();
                userDTO.RoleName = db.Roles.Where(x => x.Id == userDTO.RoleID).Select(x => x.Name).AsQueryable().FirstOrDefault();
                userDTO.IsActive = (bool)user.IsActive;
                userDTO.IsDeleted = (bool)user.IsDeleted;
                return Ok(userDTO);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpGet]
        [Route("GetRolePermissionsByRoleID")]
        public IHttpActionResult GetRolePermissionsByRoleID(int roleID)
        {
            var user = db.PrivilegesToRoles.Where(x => x.RoleId == roleID).ToList();
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpGet]
        [Route("GetRoles")]
        public IHttpActionResult GetAllRoles()
        {
                
            return Ok(db.Roles.ToList());
        }
        [HttpPost]
        [Route("CreateUpdateRolesPermissions")]
        public IHttpActionResult CreateUpdateRolesPermissions(RoleAndPermissions roleAndPermissions)
        {
            
            var roles = db.Roles.Where(x => x.Id == roleAndPermissions.RoleID).FirstOrDefault();
            if (roles == null)
            {
                Role role = new Role();
                role.Name = roleAndPermissions.Role;
                db.Roles.Add(role);
                db.SaveChanges();
                int i = 1;
                foreach (var rolesPermissions in roleAndPermissions.RolePermisions)
                {
                    PrivilegesToRole privilegesToRole = new PrivilegesToRole();
                    privilegesToRole.FormId = rolesPermissions;
                    privilegesToRole.RoleId = role.Id;
                    privilegesToRole.Status = true;
                    privilegesToRole.FormOrder = i;
                    privilegesToRole.Created_By = 1;
                    privilegesToRole.Created_ON = DateTime.Now;
                    db.PrivilegesToRoles.Add(privilegesToRole);
                    db.SaveChanges();

                    i = i + 1;
                }
                return Ok(role);
            }
            else
            {
                roles.Name = roleAndPermissions.Role;
                db.SaveChanges();
                var removeList = db.PrivilegesToRoles.Where(x => x.RoleId == roles.Id).ToList();
                if (removeList != null)
                {
                    foreach (var removeL in removeList)
                    {
                        db.PrivilegesToRoles.Remove(removeL);
                        db.SaveChanges();
                    }

                }

                int i = 1;
                foreach (var rolesPermissions in roleAndPermissions.RolePermisions)
                {
                    PrivilegesToRole privilegesToRole = new PrivilegesToRole();
                    privilegesToRole.FormId = rolesPermissions;
                    privilegesToRole.RoleId = roles.Id;
                    privilegesToRole.Status = true;
                    privilegesToRole.FormOrder = i;
                    privilegesToRole.Created_By = 1;
                    privilegesToRole.Created_ON = DateTime.Now;
                    db.PrivilegesToRoles.Add(privilegesToRole);
                    db.SaveChanges();
                    i = i + 1;
                }
                return Ok(roles);
            }

        }
        [HttpPost]
        [Route("CreateSecutityGroup")]
        public IHttpActionResult CreateSecutityGroup(SecurityGroup _securityGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _securityGroup.Created_By = 1;
                _securityGroup.Created_Date = DateTime.Now;
                db.SecurityGroups.Add(_securityGroup);
                db.SaveChanges();
                return Ok(_securityGroup);
            }
        }
        [HttpPut]
        [Route("PutSecutityGroup")]
        public IHttpActionResult PutSecutityGroup(SecurityGroup _securityGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var dbSGroups = db.SecurityGroups.Where(x => x.Id == _securityGroup.Id).AsQueryable().FirstOrDefault();
                if (dbSGroups != null)
                {
                    dbSGroups.Parent_Id = _securityGroup.Parent_Id;
                    dbSGroups.Group_Name = _securityGroup.Group_Name;
                    dbSGroups.Update_By = 1;
                    dbSGroups.Update_Date = DateTime.Now;
                    db.SaveChanges();

                }
                return Ok(_securityGroup);
            }
        }
        [HttpGet]
        [Route("GetSecurityGroupList")]
        public IHttpActionResult GetSecurityGroupList()
        {
            return Ok(db.SecurityGroups.ToList());
        }
        [HttpPost]
        [Route("GetFormsPermissionsList")]
        public IHttpActionResult GetFormsPermissionsList(int userID)
        {
            RolesToUser getUserRole = db.RolesToUsers.Where(x => x.UserId == userID).AsQueryable().FirstOrDefault();
            List<PermissionFormDTO> forms = new List<PermissionFormDTO>();
            if (getUserRole != null)
            {
                var pra = (from f in db.Forms
                           join p in db.PrivilegesToRoles on f.Id equals p.FormId
                           //where p.RoleId == getUserRole.RoleId
                           //&& f.IsMenuItem == true
                           select f).OrderBy(x => x.OrderBy).GroupBy(x => x.ModuleId).AsQueryable().ToList();
                if (pra.Count() < 0)
                {
                    return NotFound();
                }
                else
                {
                    var modulesList = db.Modules.AsQueryable().ToList();
                    foreach (var item in pra)
                    {
                        List<FormPermission> _formPermission = new List<FormPermission>();
                        PermissionFormDTO moduleDTO = new PermissionFormDTO();
                        foreach (var Formvalues in item)
                        {
                            var getMenuItem = Formvalues;
                            var getModuleItem = modulesList.Where(x => x.Id == getMenuItem.ModuleId).FirstOrDefault();
                            if (!forms.Any(x => x.name == getModuleItem.Name))
                            {

                                moduleDTO = new PermissionFormDTO();
                                _formPermission = new List<FormPermission>();
                                moduleDTO.name = getModuleItem.Name;
                                moduleDTO.ModuleID = getModuleItem.Id;
                                forms.Add(moduleDTO);
                            }
                            FormPermission formDTO = new FormPermission();
                            formDTO.FormID = getMenuItem.Id;
                            formDTO.FormName = getMenuItem.Alias;
                            // formDTO.url = getMenuItem.FormUrl;
                            _formPermission.Add(formDTO);
                            moduleDTO.ModuleFormsList = _formPermission;
                        }
                    }
                }
            }
            return Ok(forms);
        }
        [HttpPost]
        [Route("GetAllModulesFormsList")]
        public IHttpActionResult GetAllModulesFormsList()
        {
            //RolesToUser getUserRole = db.RolesToUsers.Where(x => x.UserId == userID).AsQueryable().FirstOrDefault();
            List<PermissionFormDTO> forms = new List<PermissionFormDTO>();
            //if (getUserRole != null)
            //{
            var pra = (from m in db.Modules
                       join f in db.Forms on m.Id equals f.ModuleId
                       //where p.RoleId == getUserRole.RoleId
                       //&& f.IsMenuItem == true
                       select f).OrderBy(x => x.OrderBy).GroupBy(x => x.ModuleId).AsQueryable().ToList();
            if (pra.Count() < 0)
            {
                return NotFound();
            }
            else
            {
                var modulesList = db.Modules.AsQueryable().ToList();
                foreach (var item in pra)
                {
                    List<FormPermission> _formPermission = new List<FormPermission>();
                    PermissionFormDTO moduleDTO = new PermissionFormDTO();
                    foreach (var Formvalues in item)
                    {
                        var getMenuItem = Formvalues;
                        var getModuleItem = modulesList.Where(x => x.Id == getMenuItem.ModuleId).FirstOrDefault();
                        if (!forms.Any(x => x.name == getModuleItem.Name))
                        {

                            moduleDTO = new PermissionFormDTO();
                            _formPermission = new List<FormPermission>();
                            moduleDTO.name = getModuleItem.Name;
                            moduleDTO.ModuleID = getModuleItem.Id;
                            forms.Add(moduleDTO);
                        }
                        FormPermission formDTO = new FormPermission();
                        formDTO.FormID = getMenuItem.Id;
                        formDTO.FormName = getMenuItem.Alias;
                        // formDTO.url = getMenuItem.FormUrl;
                        _formPermission.Add(formDTO);
                        moduleDTO.ModuleFormsList = _formPermission;
                    }
                }
            }
            //}
            return Ok(forms);
        }
        [HttpPost]
        [Route("GetFormsPermissionsOnRoleList")]
        public IHttpActionResult GetFormsPermissionsOnRoleList(int userID, int RoleId)
        {
            // RolesToUser getUserRole = db.RolesToUsers.Where(x => x.UserId == userID).AsQueryable().FirstOrDefault();
            List<PermissionFormDTO> forms = new List<PermissionFormDTO>();
            //if (getUserRole != null)
            //{
            var pra = (from f in db.Forms
                       join p in db.PrivilegesToRoles on f.Id equals p.FormId
                       where p.RoleId == RoleId
                       //&& f.IsMenuItem == true
                       select f).OrderBy(x => x.OrderBy).GroupBy(x => x.ModuleId).AsQueryable().ToList();
            if (pra.Count() < 0)
            {
                return NotFound();
            }
            else
            {
                var modulesList = db.Modules.AsQueryable().ToList();
                foreach (var item in pra)
                {
                    List<FormPermission> _formPermission = new List<FormPermission>();
                    PermissionFormDTO moduleDTO = new PermissionFormDTO();
                    foreach (var Formvalues in item)
                    {
                        var getMenuItem = Formvalues;
                        var getModuleItem = modulesList.Where(x => x.Id == getMenuItem.ModuleId).FirstOrDefault();
                        if (!forms.Any(x => x.name == getModuleItem.Name))
                        {

                            moduleDTO = new PermissionFormDTO();
                            _formPermission = new List<FormPermission>();
                            moduleDTO.name = getModuleItem.Name;
                            moduleDTO.ModuleID = getModuleItem.Id;
                            forms.Add(moduleDTO);
                        }
                        FormPermission formDTO = new FormPermission();
                        formDTO.FormID = getMenuItem.Id;
                        formDTO.FormName = getMenuItem.Alias;
                        // formDTO.url = getMenuItem.FormUrl;
                        _formPermission.Add(formDTO);
                        moduleDTO.ModuleFormsList = _formPermission;
                    }
                }
            }
            //}
            return Ok(forms);
        }
        [HttpPost]
        [Route("GetPermissionVerify")]
        public IHttpActionResult GetPermissionVerify(string url)
        {
           
            if (UserId != null)
            {
                try
                { var user = db.Users.Where(x => x.UserName.Equals(UserId.ToString())).AsQueryable().FirstOrDefault();
                  var FormList = (from u in db.Users
                                    join ru in db.RolesToUsers on u.UserId equals ru.UserId
                                    join pr in db.PrivilegesToRoles on ru.RoleId equals pr.RoleId
                                    join f in db.Forms on pr.FormId equals f.Id
                                    where u.UserId == user.UserId && f.FormUrl == url
                                    select f).OrderBy(x => x.OrderBy).GroupBy(x => x.ModuleId).AsQueryable().ToList();
                    if (FormList.Count>0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                catch(Exception ex)
                {
                    return this.Unauthorized();
                }
            }
            else
            {
                return this.Unauthorized();
            }
        }
    }
}
