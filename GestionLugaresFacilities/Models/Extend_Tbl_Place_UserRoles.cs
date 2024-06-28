using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionLugaresFacilities.Models
{
    [MetadataType(typeof(Extend_Tbl_Place_UserRoles))]
    public partial class Tbl_Place_UserRoles { }
    public class Extend_Tbl_Place_UserRoles
    {
        [Display(Name = "Rol")]
        public string rol { get; set; }
    }
}