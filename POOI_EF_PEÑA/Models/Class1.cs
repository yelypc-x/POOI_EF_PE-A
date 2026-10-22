using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POOI_EF_PEÑA.Models
{
    public class Consumidor
    {
        [Display(Name = "DNI")]
        public string dnicont { get; set; }

        [Display(Name = "Nombres")]
        public string nomcont { get; set; }

        [Display(Name = "Apellidos")]
        public string apecont { get; set; }

        [Display(Name = "Email")]
        public string emailcont { get; set; }

        [Display(Name = "País")]
        public string idpais { get; set; }
    }
}