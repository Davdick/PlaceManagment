using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionLugaresFacilities.Models
{
    [MetadataType(typeof(Extend_Tbl_Place_Users))]
    public partial class Tbl_Place_Users { }
    public class Extend_Tbl_Place_Users
    {
        [Display(Name = "XID")]
        public string id_employee { get; set; }

        //[Display(Name = "Nombre")]
        //public string name_employee { get; set; }

        //[Display(Name = "Apellidos")]
        //public string lastname { get; set; }

        //[Display(Name = "IBT")]
        //public string ibt { get; set; }

        //[Display(Name = "Posicion")]
        //public string position { get; set; }

        //[Display(Name = "Supervisor")]
        //public string supervisor { get; set; }

        //[Display(Name = "Email")]
        //public string email { get; set; }
    }
}