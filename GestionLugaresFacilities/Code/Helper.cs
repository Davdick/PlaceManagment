using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionLugaresFacilities
{
    public static class Helper
    {
        public static MvcHtmlString SuccessMessage(this HtmlHelper helper)
        {
            var message = helper.ViewContext.Controller.TempData["success_message"]; string html = (message != null) ? string.Format("<script>alert('{0}')</script>", message) : "";
            return new MvcHtmlString(html);
        }
    }
}