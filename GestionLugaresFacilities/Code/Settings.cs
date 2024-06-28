using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using GestionLugaresFacilities.Models;

namespace GestionLugaresFacilities
{
    public class Settings
    {
        //public static Models.Tbl_Place_Users User
        //{
        //    get { return HttpContext.Current.Session["ServiceRequestUserLoggedIn"] as Models.Tbl_Place_Users; }
        //    set { HttpContext.Current.Session["ServiceRequestUserLoggedIn"] = value; }
        //}
        public static UserModel LoggedUser
        {
            get
            {
                if (HttpContext.Current.Session["user"] == null)
                    return null;
                return (UserModel)HttpContext.Current.Session["user"];
            }
            set
            {
                HttpContext.Current.Session["user"] = value;
            }
        }
        public static bool KioskoMode
        {
            get
            {
                if (HttpContext.Current.Session["KioskoMode"] == null)
                    HttpContext.Current.Session["kioskoMode"] = true;
                return (bool)HttpContext.Current.Session["KioskoMode"];
            }
            set
            {
                HttpContext.Current.Session["KioskoMode"] = value;
            }
        }
        
        public static String RedirectURL
        {
            get { return HttpContext.Current.Session["RedirectURL"]?.ToString(); }
            set { HttpContext.Current.Session["RedirectURL"] = value; }
        }
        public static string URL_LogIn
        {
            get { return ConfigurationManager.AppSettings["LogInURL"]; }
        }
        //public static int Role
        //{
        //    get { return (int)HttpContext.Current.Session["Rol"]; }

        //    set { HttpContext.Current.Session["Rol"] = value; }
        //}
        public static string URL_GetEmployeeNumber
        {
            get { return ConfigurationManager.AppSettings["URL_GetEmployeeNumber"]; }
        }

    }
}