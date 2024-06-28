using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GestionLugaresFacilities.Models
{
    [MetadataType(typeof(Extend_Tbl_Place_Diagram))]
    public partial class Tbl_Place_Diagram { }
    public class Extend_Tbl_Place_Diagram
    {
        [Display(Name = "Piso o Snorkel")]
        public Nullable<int> piso_snorkel { get; set; }
    }
}