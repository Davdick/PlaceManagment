using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GestionLugaresFacilities.Models;
using System.Threading.Tasks;
using GestionLugaresFacilities.Controllers;
using System.Net.Http;
using GestionLugaresFacilities.Code;
using Newtonsoft.Json;

namespace GestionLugaresFacilities.Controllers
{
    public class Tbl_Place_UsersController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        private BaseController _base;
        public Tbl_Place_UsersController()
        {
            _base = new BaseController();
        }
        [Authorization]
        // GET: Tbl_Place_Users
        public ActionResult Index()
        {
            var tbl_Place_Users = db.Tbl_Place_Users.Include(t => t.Tbl_Place_UserRoles).Include(t => t.Tbl_Place_UserStatus);
            return View(tbl_Place_Users.ToList());
        }
        public ActionResult UsersPartial()
        {
            var users = db.Tbl_Place_Users.Include(u => u.Tbl_Place_UserRoles).Include(u => u.Tbl_Place_UserRoles);
            return PartialView(users.ToList());
        }
        public ActionResult UsersPartialSpecific(string code)
        {
            var users = db.Tbl_Place_Users
                          .Include(u => u.Tbl_Place_UserRoles)
                          .Include(u => u.Tbl_Place_UserStatus)
                          .Where(u => u.id_employee == code);
            return PartialView(users.ToList());
        }

        // GET: Tbl_Place_Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Users tbl_Place_Users = db.Tbl_Place_Users.Find(id);
            if (tbl_Place_Users == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Users);
        }

        // GET: Tbl_Place_Users/Create
        public ActionResult Create()
        {
            ViewBag.id_rol = new SelectList(db.Tbl_Place_UserRoles, "id_roles", "rol");
            ViewBag.id_status = new SelectList(db.Tbl_Place_UserStatus, "id_status", "statusU");
            return View();
        }
        public ActionResult CreateAll(int? id)
        {
            ViewBag.id_rol = new SelectList(db.Tbl_Place_UserRoles, "id_roles", "rol");
            ViewBag.id_status = new SelectList(db.Tbl_Place_UserStatus, "id_status", "statusU");
            ViewBag.idDiagram = id;
            return RedirectToAction("JUASview", "Tbl_Place_Diagram", new { idDiagram = id });
        }

        // POST: Tbl_Place_Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_users,id_employee,name_employee,lastname,ibt,position,supervisor,email,id_rol,id_status")] Tbl_Place_Users user)
        {
            var finUser = db.Tbl_Place_Users.Where(u => u.id_employee == user.id_employee)
                            .Select(c => new {
                                id = c.id_users,
                            })
                        .FirstOrDefault();
            try
            {
                Tbl_Place_Users user1 = db.Tbl_Place_Users.Find(finUser.id);
                string response = "Error! empleado ya registrado";

                return Json(new { success = response });
            }
            catch (NullReferenceException ex)
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Tbl_Place_Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex2)
                    {
                        // Iterar a través de los errores de validación para obtener detalles
                        foreach (var entityValidationError in ex2.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationError.ValidationErrors)
                            {
                                // Puedes imprimir o registrar estos errores para diagnosticar el problema
                                Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            }
                        }

                        // Puedes decidir cómo manejar el error aquí (por ejemplo, mostrar un mensaje al usuario o registrar el error).
                        // También puedes lanzar una excepción personalizada si es necesario.
                        return RedirectToAction("ErrorNotFound","Home"); // Otra acción o vista que maneje errores
                    }



                }
            }




            ViewBag.id_rol = new SelectList(db.Tbl_Place_UserRoles, "id_roles", "rol", user.id_rol);
            ViewBag.id_status = new SelectList(db.Tbl_Place_UserStatus, "id_status", "statusU", user.id_status);
            return View(user);
        }

        // GET: Tbl_Place_Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Users tbl_Place_Users = db.Tbl_Place_Users.Find(id);
            if (tbl_Place_Users == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_rol = new SelectList(db.Tbl_Place_UserRoles, "id_roles", "rol", tbl_Place_Users.id_rol);
            ViewBag.id_status = new SelectList(db.Tbl_Place_UserStatus, "id_status", "statusU", tbl_Place_Users.id_status);
            return View(tbl_Place_Users);
        }

        // POST: Tbl_Place_Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_users,id_employee,name_employee,lastname,ibt,position,supervisor,email,id_rol,id_status")] Tbl_Place_Users tbl_Place_Users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_Users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_rol = new SelectList(db.Tbl_Place_UserRoles, "id_roles", "rol", tbl_Place_Users.id_rol);
            ViewBag.id_status = new SelectList(db.Tbl_Place_UserStatus, "id_status", "statusU", tbl_Place_Users.id_status);
            return View(tbl_Place_Users);
        }

        // GET: Tbl_Place_Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Users tbl_Place_Users = db.Tbl_Place_Users.Find(id);
            if (tbl_Place_Users == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Users);
        }

        // POST: Tbl_Place_Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Place_Users tbl_Place_Users = db.Tbl_Place_Users.Find(id);
            db.Tbl_Place_Users.Remove(tbl_Place_Users);
            db.SaveChanges();
            return RedirectToAction("About", "Home");
        }

        public async Task<string> GetApiEmployee(string idEmp)
        {
           // ApiEmployeeModel employeeModel = new ApiEmployeeModel();
            TokenModel token = await _base.GetToken();
            var jsonResponse = String.Empty;

            try
            {
                HttpClient _httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(EnviromentVars.URL_BASE_API)
                };

                _httpClient.DefaultRequestHeaders.Add("Authorization", token.Token);

                var response = await _httpClient.GetAsync("api/Employee?EmpNum="+idEmp);
                
                if (response.IsSuccessStatusCode)
                {
                    jsonResponse = await response.Content.ReadAsStringAsync();
                    //employeeModel = JsonConvert.DeserializeObject<ApiEmployeeModel>(jsonResponse);
                }
                else
                {
                    throw new Exception("Error de respuesto");
                }
            }
            catch (Exception ex)
            {

            }
            return jsonResponse;
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
