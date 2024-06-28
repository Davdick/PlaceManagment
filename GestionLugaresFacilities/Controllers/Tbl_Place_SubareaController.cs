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
    public class Tbl_Place_SubareaController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        [Authorization]
        // GET: Tbl_Place_Subarea
        public ActionResult Index()
        {
            return View(db.Tbl_Place_Subarea.ToList());
        }
        public ActionResult SubareasPartialView()
        {
            return PartialView(db.Tbl_Place_Subarea.ToList());
        }

        // GET: Tbl_Place_Subarea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Subarea tbl_Place_Subarea = db.Tbl_Place_Subarea.Find(id);
            if (tbl_Place_Subarea == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Subarea);
        }

        // GET: Tbl_Place_Subarea/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Place_Subarea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_subarea,name_subarea")] Tbl_Place_Subarea tbl_Place_Subarea)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Place_Subarea.Add(tbl_Place_Subarea);
                db.SaveChanges();
                return RedirectToAction("About","Home");
            }

            return View(tbl_Place_Subarea);
        }

        // GET: Tbl_Place_Subarea/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Subarea tbl_Place_Subarea = db.Tbl_Place_Subarea.Find(id);
            if (tbl_Place_Subarea == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Subarea);
        }

        // POST: Tbl_Place_Subarea/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_subarea,name_subarea")] Tbl_Place_Subarea tbl_Place_Subarea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_Subarea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("About","Home");
            }
            return View(tbl_Place_Subarea);
        }

        // GET: Tbl_Place_Subarea/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Subarea tbl_Place_Subarea = db.Tbl_Place_Subarea.Find(id);
            if (tbl_Place_Subarea == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Subarea);
        }

        // POST: Tbl_Place_Subarea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Place_Subarea tbl_Place_Subarea = db.Tbl_Place_Subarea.Find(id);
            db.Tbl_Place_Subarea.Remove(tbl_Place_Subarea);
            db.SaveChanges();
            return RedirectToAction("About", "Home");
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
