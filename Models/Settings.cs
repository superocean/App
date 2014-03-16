using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XPGroup.Models
{
    public class AdminUser
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class SmtpSetting
    {
        public string SmtpServer { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsSSL { get; set; }

        public int Port { get; set; }
    }

    public class Settings
    {
        public AdminUser AdminUser { get; set; }

        public SmtpSetting SmtpSetting { get; set; }
    }
}