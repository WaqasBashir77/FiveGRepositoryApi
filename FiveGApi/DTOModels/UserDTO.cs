using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FiveGApi.DTOModels
{
    public class UserDTO
    {
        public int? UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Nullable<int> SecurityGroupId { get; set; }
        [Required]
        public Nullable<int> RoleID { get; set; }
        public string RoleName { get; set; }
        public int? StaffID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}