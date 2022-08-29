using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERSEntity.Entity;

namespace ERSEntity
{
    public class SystemUserEntity : CommonEntity
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int FailedAttempt { get; set; }
        public bool IsPasswordChanged { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }
        public bool isStudent { get; set; }
        public bool isLabPersonnel { get; set; }
        public string ResetPasswordCode { get; set; }

    }
    public class SystemUserChangePasswordEntity : CommonEntity
    {
        public int SystemUserID { get; set; }
        public string OldPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }
        public string ResetPasswordCode { get; set; }
        // public Guid ActivationCode { get; set; }
        //public string ResetPasswordCode { get; set; }
    }
}
