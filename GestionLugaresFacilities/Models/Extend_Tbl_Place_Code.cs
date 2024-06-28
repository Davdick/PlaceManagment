using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionLugaresFacilities.Models
{
    [MetadataType(typeof(Extend_Tbl_Place_Code))]
    public partial class Tbl_Place_Code { }
    public class Extend_Tbl_Place_Code
    {
        [Display(Name = "Código")]
        public string code { get; set; }
        [Display(Name = "Fecha")]
        public Nullable<System.DateTime> date_time { get; set; }
        [Display(Name = "Tipo de lugar")]
        public string type_place { get; set; }
        [Display(Name = "Codigo de silla")]
        public string chair_code { get; set; }
    }
}