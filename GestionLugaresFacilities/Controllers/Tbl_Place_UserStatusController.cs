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
    public class Tbl_Place_UserStatusController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        [Authorization]
        // GET: Tbl_Place_UserStatus
        public ActionResult Index()
        {
            return View(db.Tbl_Place_UserStatus.ToList());
        }

        // GET: Tbl_Place_UserStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_UserStatus tbl_Place_UserStatus = db.Tbl_Place_UserStatus.Find(id);
            if (tbl_Place_UserStatus == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_UserStatus);
        }

        // GET: Tbl_Place_UserStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Place_UserStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_status,statusU")] Tbl_Place_UserStatus tbl_Place_UserStatus)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Place_UserStatus.Add(tbl_Place_UserStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Place_UserStatus);
        }

        // GET: Tbl_Place_UserStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_UserStatus tbl_Place_UserStatus = db.Tbl_Place_UserStatus.Find(id);
            if (tbl_Place_UserStatus == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_UserStatus);
        }

        // POST: Tbl_Place_UserStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_status,statusU")] Tbl_Place_UserStatus tbl_Place_UserStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_UserStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Place_UserStatus);
        }

        // GET: Tbl_Place_UserStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_UserStatus tbl_Place_UserStatus = db.Tbl_Place_UserStatus.Find(id);
            if (tbl_Place_UserStatus == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_UserStatus);
        }

        // POST: Tbl_Place_UserStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Place_UserStatus tbl_Place_UserStatus = db.Tbl_Place_UserStatus.Find(id);
            db.Tbl_Place_UserStatus.Remove(tbl_Place_UserStatus);
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
