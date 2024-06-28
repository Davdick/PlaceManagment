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
    public class Tbl_Place_UserRolesController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        [Authorization]
        // GET: Tbl_Place_UserRoles
        public ActionResult Index()
        {
            return View(db.Tbl_Place_UserRoles.ToList());
        }

        // GET: Tbl_Place_UserRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_UserRoles tbl_Place_UserRoles = db.Tbl_Place_UserRoles.Find(id);
            if (tbl_Place_UserRoles == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_UserRoles);
        }

        // GET: Tbl_Place_UserRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Place_UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_roles,rol")] Tbl_Place_UserRoles tbl_Place_UserRoles)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Place_UserRoles.Add(tbl_Place_UserRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Place_UserRoles);
        }

        // GET: Tbl_Place_UserRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_UserRoles tbl_Place_UserRoles = db.Tbl_Place_UserRoles.Find(id);
            if (tbl_Place_UserRoles == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_UserRoles);
        }

        // POST: Tbl_Place_UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_roles,rol")] Tbl_Place_UserRoles tbl_Place_UserRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_UserRoles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Place_UserRoles);
        }

        // GET: Tbl_Place_UserRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_UserRoles tbl_Place_UserRoles = db.Tbl_Place_UserRoles.Find(id);
            if (tbl_Place_UserRoles == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_UserRoles);
        }

        // POST: Tbl_Place_UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Place_UserRoles tbl_Place_UserRoles = db.Tbl_Place_UserRoles.Find(id);
            db.Tbl_Place_UserRoles.Remove(tbl_Place_UserRoles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
