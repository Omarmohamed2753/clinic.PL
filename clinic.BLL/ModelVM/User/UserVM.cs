using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace clinic.BLL.ModelVM.User
{
    public class UserVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
