using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarManagementApplication.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Reg_Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ResetPasswordCode { get; set; }
        public int Token { get; set; }
        public int TokenExpireDate { get; set; }
    }
}
