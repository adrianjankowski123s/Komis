using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Komis.Models
{
    public class ContactModel
    {
        [Required, Display(Name = "Imię i nazwisko")]
        public string SenderName { get; set; }
 
        [Required, Display(Name = "Twój e-mail"), EmailAddress]
        public string SenderEmail { get; set; }

        [Display(Name = "Temat")]
        public string Topic { get; set; }

        [Required, Display(Name = "Wiadomość")]
        public string Message { get; set; }
    }
}