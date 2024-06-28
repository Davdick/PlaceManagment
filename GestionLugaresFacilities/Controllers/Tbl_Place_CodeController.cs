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
    public class Tbl_Place_CodeController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        [Authorization]
        // GET: Tbl_Place_Code
        public ActionResult Index()
        {
            var tbl_Place_Code = db.Tbl_Place_Code.Include(t => t.Tbl_Place_Diagram);
            return View(tbl_Place_Code.ToList());
        }

        // GET: Tbl_Place_Code/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Code tbl_Place_Code = db.Tbl_Place_Code.Find(id);
            if (tbl_Place_Code == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Code);
        }
        public JsonResult GetCodeList(int id_d, string d_coord)
        {
            List<SelectListItem> items = db.Tbl_Place_Code
                .Where(x => x.id_diagram == id_d && x.data_coord == d_coord)
                .Select(x => new SelectListItem()
                {
                    Text = x.code,
                    Value = x.id_code.ToString()
                })
                .ToList();

            return Json(items, JsonRequestBehavior.AllowGet);



        }
        public JsonResult GetIdCodes(string code)
        {
            if (code == null)
            {
                return Json(new { error = "code is missing" }, JsonRequestBehavior.AllowGet);
            }
            //Assignment assignment = db.Assignments.Find(id);
            //Assignment assignment;
            var id_code = db.Tbl_Place_Code
                            .Where(a => a.code == code)
                            .Select(a => new
                            {
                                IdCode = a.code,
                                // Otras propiedades

                            })
                            .ToList();

            return Json(id_code, JsonRequestBehavior.AllowGet);
        }

      //  GET: Tbl_Place_Code/Create
        public ActionResult Create()
        {
            ViewBag.id_diagram = new SelectList(db.Tbl_Place_Diagram, "id_diagram", "data_info");
            return View();
        }

        //POST: Tbl_Place_Code/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Tbl_Place_Code Data)
        {
            if (ModelState.IsValid && Data.code != null)
            {
                Data.date_time = DateTime.Now;
                db.Tbl_Place_Code.Add(Data);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Data);
        }

        // GET: Tbl_Place_Code/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Code tbl_Place_Code = db.Tbl_Place_Code.Find(id);
            if (tbl_Place_Code == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_diagram = new SelectList(db.Tbl_Place_Diagram, "id_diagram", "data_info", tbl_Place_Code.id_diagram);
            return View(tbl_Place_Code);
        }

        // POST: Tbl_Place_Code/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_code,id_diagram,data_coord,code,date_time,type_place,chair_code")] Tbl_Place_Code tbl_Place_Code)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_Code).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_diagram = new SelectList(db.Tbl_Place_Diagram, "id_diagram", "data_info", tbl_Place_Code.id_diagram);
            return View(tbl_Place_Code);
        }
        [HttpPost]
        public ActionResult UpdateCode(int idCode, string code)
        {
            Tbl_Place_Code tbl_Place_Code = db.Tbl_Place_Code.Find(idCode);
            tbl_Place_Code.code = code;
            db.SaveChanges();
            return Json(tbl_Place_Code.code, JsonRequestBehavior.AllowGet);
        }
        // GET: Tbl_Place_Code/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Code code = db.Tbl_Place_Code.Find(id);
            var idDiagram = db.Tbl_Place_Assignment
                  .Where(c => c.id_assignment == id)
           .Select(c => new {
               Id = c.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram,
           })
           .FirstOrDefault();
            ViewBag.id_diagram = code.Tbl_Place_Diagram.id_diagram;
            if (code == null)
            {
                return HttpNotFound();
            }
            return View(code);
        }

        // POST: Tbl_Place_Code/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string view)
        {
            Tbl_Place_Code tbl_Place_Code = db.Tbl_Place_Code.Find(id);
            db.Tbl_Place_Code.Remove(tbl_Place_Code);
            db.SaveChanges();
           
            return RedirectToAction("editAdmin", "Tbl_Place_Diagram", new { id = tbl_Place_Code.id_diagram });
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
