using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountManager.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class ChangeNameModel
    {
        public string NewUsername { get; set; }
    }

    public class ChangeEmailModel
    {
        public string NewEmail { get; set; }
    }

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
        public string RetryPassword { get; set; }
    }

    public class ClanCreateModel
    {
        public string ClanName { get; set; }
    }

    public class ClanInviteModel
    {
        public string Username { get; set; }
    }
}