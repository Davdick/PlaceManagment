using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GestionLugaresFacilities.Code
{
    public class EnviromentVars
    {
        public static string URL_BASE_API
        {
            get { return ConfigurationManager.AppSettings["URL_BASE_API"]; }
        }

        public static string ID_APP_API
        {
            get { return ConfigurationManager.AppSettings["ID_APP_API"]; }
        }
    }
}