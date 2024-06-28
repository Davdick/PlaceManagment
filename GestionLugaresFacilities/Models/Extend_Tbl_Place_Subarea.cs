using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace GestionLugaresFacilities.Models
{
    [MetadataType(typeof(Extend_Tbl_Place_Subarea))]
    public partial class Tbl_Place_Subarea { }
    public class Extend_Tbl_Place_Subarea
    {
        [Display(Name = "Subarea")]
        public string name_subarea { get; set; }
    }
}