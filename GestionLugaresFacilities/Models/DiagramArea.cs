using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestionLugaresFacilities.Models
{
    public class DiagramArea
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Subarea { get; set; }
        public int? Floor { get; set; }
    }
}