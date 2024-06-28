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
    public class Tbl_Place_Area_Controller : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        [Authorization]
        // GET: Tbl_Place_Area_
        public ActionResult Index()
        {
            return View(db.Tbl_Place_Area_.ToList());
        }
        public ActionResult AreasPartial()
        {
            return PartialView(db.Tbl_Place_Area_.ToList());
        }

        // GET: Tbl_Place_Area_/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Area_ tbl_Place_Area_ = db.Tbl_Place_Area_.Find(id);
            if (tbl_Place_Area_ == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Area_);
        }

        // GET: Tbl_Place_Area_/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Place_Area_/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_area,name_area")] Tbl_Place_Area_ tbl_Place_Area_)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Place_Area_.Add(tbl_Place_Area_);
                db.SaveChanges();
                return RedirectToAction("About","Home");
            }

            return View(tbl_Place_Area_);
        }

        // GET: Tbl_Place_Area_/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Area_ tbl_Place_Area_ = db.Tbl_Place_Area_.Find(id);
            if (tbl_Place_Area_ == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Area_);
        }

        // POST: Tbl_Place_Area_/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_area,name_area")] Tbl_Place_Area_ tbl_Place_Area_)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_Area_).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("About","Home");
            }
            return View(tbl_Place_Area_);
        }

        // GET: Tbl_Place_Area_/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Area_ tbl_Place_Area_ = db.Tbl_Place_Area_.Find(id);
            if (tbl_Place_Area_ == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Area_);
        }

        // POST: Tbl_Place_Area_/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Place_Area_ tbl_Place_Area_ = db.Tbl_Place_Area_.Find(id);
            db.Tbl_Place_Area_.Remove(tbl_Place_Area_);
            db.SaveChanges();
            return RedirectToAction("About","Home");
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
