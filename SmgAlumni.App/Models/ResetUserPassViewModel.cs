using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmgAlumni.App.Models
{
    public class ResetUserPassViewModel
    {
        public int Id { get; set; }
        
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string NewPassword { get; set; }
    }
}