using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using GestionLugaresFacilities.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GestionLugaresFacilities.Controllers
{
    public class Tbl_Place_AssignmentController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        private const string From = "itappsnotifications@sensata.com";
        private Tbl_Place_UsersController _users;
        public Tbl_Place_AssignmentController()
        {
            _users = new Tbl_Place_UsersController();
        }
        [Authorization]
        // GET: Tbl_Place_Assignment
        public ActionResult Index(string area, string subarea, int piso_snk)

        {
            var assignments = db.Tbl_Place_Assignment
                .Include(a => a.Tbl_Place_Code)
                .Include(a => a.Tbl_Place_Users)
                .Where(a => a.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Area_.name_area == area && a.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Subarea.name_subarea == subarea &&
                a.Tbl_Place_Code.Tbl_Place_Diagram.piso_snorkel == piso_snk);
            if(assignments == null)
            {
                return RedirectToAction("ErrorNotFound", "Home");
            }
            if (subarea == null)
            {
                assignments = db.Tbl_Place_Assignment
               .Include(a => a.Tbl_Place_Code)
               .Include(a => a.Tbl_Place_Users)
               .Where(a => a.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Area_.name_area == area && a.Tbl_Place_Code.Tbl_Place_Diagram.piso_snorkel == piso_snk);
            }

            ViewBag.area = area;
            ViewBag.subarea = subarea;
            ViewBag.piso_snk = piso_snk;
            ViewBag.ModelJson = Newtonsoft.Json.JsonConvert.SerializeObject(assignments.ToList());

            return View(assignments.ToList());
        }

        // GET: Tbl_Place_Assignment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Assignment tbl_Place_Assignment = db.Tbl_Place_Assignment.Find(id);
            if (tbl_Place_Assignment == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_Assignment);
        }
        public JsonResult GetUserList(string txt)
        {
            List<SelectListItem> items = db.Tbl_Place_Users
                .Where(x =>
                x.id_employee.Contains(txt))
                .Select(x => new SelectListItem()
                {
                    Text = x.id_employee,
                    Value = x.id_users.ToString()
                })
                .ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    
        public JsonResult JUAS(int? id)
        {
            if (id == null)
            {
                return Json(new { error = "ID is missing" }, JsonRequestBehavior.AllowGet);
            }
            //Assignment assignment = db.Assignments.Find(id);
            //Assignment assignment;
            var assignments = db.Tbl_Place_Assignment
                                             .Where(a => a.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram == id)
                                           .Select(a => new
                                           {
                                               // Selecciona solo las propiedades necesarias de 'Assignment', 'Code' y 'User'
                                               // Puedes agregar más propiedades aquí
                                               //Code = a.Code.code1,
                                               // Otras propiedades
                                               IdAssign = a.id_assignment,
                                               Coden = a.Tbl_Place_Code.code,
                                               CodeDataC = a.Tbl_Place_Code.data_coord,
                                               CodeTypeP = a.Tbl_Place_Code.type_place,
                                               CodeChair = a.Tbl_Place_Code.chair_code,
                                               UserId = a.Tbl_Place_Users.id_employee
                                               //UserName = a.Tbl_Place_Users.name_employee,
                                               //UserLast = a.Tbl_Place_Users.lastname,
                                               //UserIbt = a.Tbl_Place_Users.ibt,
                                               //UserPos = a.Tbl_Place_Users.position,
                                               //UserSup = a.Tbl_Place_Users.supervisor,
                                               //UserEmail = a.Tbl_Place_Users.email

                                               // Otras propiedades

                                           })
                                            .ToList();
            //assignment = db.Assignments
            //               .Include(a => a.Code)
            //               .Include(a => a.User)
            //               .FirstOrDefault(a => a.Code.Diagram.id_diagram == id);

            return Json(assignments, JsonRequestBehavior.AllowGet);
        }

        // GET: Tbl_Place_Assignment/Create
        public ActionResult Create()
        {
            ViewBag.id_code = new SelectList(db.Tbl_Place_Code, "id_code", "data_coord");
            ViewBag.id_user = new SelectList(db.Tbl_Place_Users, "id_users", "id_employee");
            return View();
        }

        // POST: Tbl_Place_Assignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(Tbl_Place_Assignment ass)
        {

            if (ModelState.IsValid)
            {
                db.Tbl_Place_Assignment.Add(ass);
                db.SaveChanges();

                try
                {


                    var findAss = db.Tbl_Place_AssignmentRequests
                            .Where(a => a.id_code == ass.id_code && a.id_user == ass.id_user)
                            .FirstOrDefault();
                   
                    db.Tbl_Place_AssignmentRequests.Remove(findAss);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("no , asignacion de ADMIN", ex.Message);
                }
                finally
                {
                    //AQUI LO DEL MAIL!!!!!!!!!!!!!!!!!!
                    var findMail = db.Tbl_Place_Assignment
                          .Where(a => a.id_code == ass.id_code && a.id_user == ass.id_user)
                          .Select(a => new
                          {
                              id = a.Tbl_Place_Users.id_employee,
                              area = a.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Area_.name_area,
                              subarea = a.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Subarea.name_subarea,
                              fsnk = a.Tbl_Place_Code.Tbl_Place_Diagram.piso_snorkel,
                              code = a.Tbl_Place_Code.code
                          })
                          .FirstOrDefault();
                    //ApiEmployeeModel employeeModel = new ApiEmployeeModel();
                    //var jsonResponse = String.Empty;
                    //jsonResponse = await _users.GetApiEmployee(ass.Tbl_Place_Users.id_employee);
                    //employeeModel = JsonConvert.DeserializeObject<ApiEmployeeModel>(jsonResponse);

                    const string TemplateMail = @"
                                   <div>
                                       <div style = 'background-color: rgb(25, 32, 30);'> 
                                            <h3 style='color: white;'>Aviso de su estado de lugar</h3>
                                            <h3 style='font-size:30px; color:white;'>HA SIDO ASIGNADO</h3>
                                       </div>
                                       <section>
                                            <table>
                                                <thead style='background:#0f94e6, color:white;'>
                                                    <tr>
                                                        <th>Area</th>
                                                        <th>Subarea</th>
                                                        <th>Piso o snorkel</th>
                                                        <th>Codigo de lugar</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td>{0}</td>
                                                        <td>{1}</td>
                                                        <td>{2}</td>
                                                        <td>{3}</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                       </section>
                                   </div>
                                 ";
                    var mailToSend = @"{0}@sensata.com";

                    var client = new SmtpClient();
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(From);
                    mailMessage.To.Add(string.Format(mailToSend, findMail.id));
                    mailMessage.Subject = "Asignado! Gestion de lugares";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = string.Format(TemplateMail, findMail.area, findMail.subarea, findMail.fsnk, findMail.code);
                    client.Send(mailMessage);
                }
                return RedirectToAction("About", "Home");

            }



            return View(ass);
        }

        // GET: Tbl_Place_Assignment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Assignment tbl_Place_Assignment = db.Tbl_Place_Assignment.Find(id);
            if (tbl_Place_Assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_code = new SelectList(db.Tbl_Place_Code, "id_code", "data_coord", tbl_Place_Assignment.id_code);
            ViewBag.id_user = new SelectList(db.Tbl_Place_Users, "id_users", "id_employee", tbl_Place_Assignment.id_user);
            return View(tbl_Place_Assignment);
        }

        // POST: Tbl_Place_Assignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_assignment,id_code,id_user")] Tbl_Place_Assignment tbl_Place_Assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_Assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_code = new SelectList(db.Tbl_Place_Code, "id_code", "data_coord", tbl_Place_Assignment.id_code);
            ViewBag.id_user = new SelectList(db.Tbl_Place_Users, "id_users", "id_employee", tbl_Place_Assignment.id_user);
            return View(tbl_Place_Assignment);
        }

        // GET: Tbl_Place_Assignment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Assignment assignment = db.Tbl_Place_Assignment.Find(id);
            var idDiagram = db.Tbl_Place_Assignment
                  .Where(c => c.id_assignment == id)
           .Select(c => new {
               Id = c.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram,
           })
           .FirstOrDefault();
            ViewBag.id_diagram = idDiagram.Id;
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Tbl_Place_Assignment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Place_Assignment tbl_Place_Assignment = db.Tbl_Place_Assignment.Find(id);
            var idDiagram = tbl_Place_Assignment.Tbl_Place_Code.id_diagram;
            db.Tbl_Place_Assignment.Remove(tbl_Place_Assignment);
            db.SaveChanges();
            return RedirectToAction("editAdmin", "Tbl_Place_Diagram", new { id = idDiagram });
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
