using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionLugaresFacilities.Models;
namespace GestionLugaresFacilities.Controllers
{
    public class HomeController : Controller
    {
       
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        [Authorization]
        public ActionResult Index()
        {

            return View(db.Tbl_Place_Area_.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // Aqui inicia la seguridad
        public ActionResult LogIn()
        {
            return Redirect(Settings.URL_LogIn);
        }
        public ActionResult LogOut()
        {
            HttpContext.Session.RemoveAll();
            HttpContext.Session.Abandon();
            Settings.LoggedUser = null;
            //return RedirectToAction("Unauthorized");
            //return View("Logout");
            return RedirectToAction("Index", "Home");
        }
        //public ActionResult Logged(string par1, string user)
        //{
        //    #region Security validation
        //    // If UrlReferrer IS NULL then DENY access
        //    if (HttpContext.Request.UrlReferrer == null)
        //        return RedirectToAction("Unauthorized","Home"); // change for your view

        //    var validUrlReferrers = new[] {
        //        "http://mexico/SensataLogin",
        //        "http://mexico.corp.sensata.com/SensataLogin",
        //        "http://mexico"
        //    };
        //    var referrer = HttpContext.Request.UrlReferrer.AbsoluteUri.ToLower();
        //    var requestComeFromSensataLogin = validUrlReferrers.Any(x => referrer.StartsWith(x));
        //    if (!requestComeFromSensataLogin)
        //        return RedirectToAction("Unauthorized"); // change for your view
        //    #endregion

        //    //  Get valid user if any
        //    var User = new DbFacilitesSystemEntities().Tbl_Place_Users.FirstOrDefault(x => x.id_employee == user);

        //    var registeredUser = new DbFacilitesSystemEntities().Tbl_Place_Users.Where(x => x.id_employee == user)
        //        .Select(x => new UserModel() {
        //            User = User,
        //            Rol = User.id_rol
        //        }).FirstOrDefault();

        //    if (registeredUser != null && User.id_status == 1)
        //    {
        //        Settings.LoggedUser = registeredUser;
        //        Settings.Role = registeredUser.Rol;

        //        if (!string.IsNullOrWhiteSpace(Settings.RedirectURL))
        //            return Redirect(Settings.RedirectURL);

        //        return RedirectToAction("Index", "Home");
        //    }

        //    // If not exists the employee number then deny access
        //    return RedirectToAction("Unauthorized", "Home");
        //}
            //Aqui termina
            public ActionResult Unauthorized()

            {
                return View("Unauthorized");

            }
        public ActionResult Abt()
        {
            return View();
        }
        public ActionResult ErrorNotFound()

        {
            return View();

        }
    }


}
