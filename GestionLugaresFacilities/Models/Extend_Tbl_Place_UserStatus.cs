using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionLugaresFacilities.Models
{
    [MetadataType(typeof(Extend_Tbl_Place_UserStatus))]
    public partial class Tbl_Place_UserStatus { }
    public class Extend_Tbl_Place_UserStatus
    {
        [Display(Name = "Estatus")]
         public string statusU { get; set; }
    }
}