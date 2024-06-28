using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionLugaresFacilities.Models
{
    [MetadataType(typeof(Extend_Tbl_Place_Area))]
    public partial class Tbl_Place_Area_ { }
    public class Extend_Tbl_Place_Area
    {
        [Display(Name = "Area")]
        public string name_area { get; set; }
    }
}