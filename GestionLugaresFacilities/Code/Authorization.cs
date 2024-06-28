using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GestionLugaresFacilities.Controllers;

namespace GestionLugaresFacilities
{
    public class Authorization : AuthorizeAttribute
    {
        /// <summary>
        /// Checks to see if the user is authenticated and has a valid session object
        /// </summary>        
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            //  If there is no user session return False
            if (Settings.LoggedUser == null)
                return false;

            //  If there is a user session and there are no roles required return true
            var roles = this.Roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (roles.Length == 0) return true;

            //  If there is required to be a Registered User and there is not one return false
            //if (roles.Contains("RegisteredUser") && Settings.User == null)
            //    return false;

            //  If there is no roles required return true, otherwise check for the user role to be in the valid roles
            //var realRoles = roles.Where(x => x != "RegisteredUser");
            //return realRoles.Count() == 0 ? true : realRoles.Contains(Settings.User.Role.Name);
            return roles.Contains(Settings.LoggedUser.User.id_rol.ToString());
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (Settings.LoggedUser == null)
            {
                Settings.RedirectURL = HttpContext.Current.Request.Url.AbsoluteUri;
                filterContext.HttpContext.Response.Redirect(Settings.URL_LogIn, false);
            }

            filterContext.Result = new HomeController().Unauthorized();
        }
    }
}