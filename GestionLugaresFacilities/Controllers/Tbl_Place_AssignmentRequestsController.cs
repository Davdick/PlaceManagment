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
    public class Tbl_Place_AssignmentRequestsController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        private const string From = "itappsnotifications@sensata.com";
        private Tbl_Place_UsersController _users;
        public Tbl_Place_AssignmentRequestsController()
        {
            _users = new Tbl_Place_UsersController();
        }
        [Authorization]
        // GET: Tbl_Place_AssignmentRequests
        public ActionResult Index()
        {
            var tbl_Place_AssignmentRequests = db.Tbl_Place_AssignmentRequests.Include(t => t.Tbl_Place_Code).Include(t => t.Tbl_Place_Users);
            return View(tbl_Place_AssignmentRequests.ToList());
        }
        public ActionResult PartialIndex()
        {
            var tbl_Place_AssignmentRequests = db.Tbl_Place_AssignmentRequests.Include(t => t.Tbl_Place_Code).Include(t => t.Tbl_Place_Users);
            return PartialView(tbl_Place_AssignmentRequests);
        }

        // GET: Tbl_Place_AssignmentRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_AssignmentRequests tbl_Place_AssignmentRequests = db.Tbl_Place_AssignmentRequests.Find(id);
            if (tbl_Place_AssignmentRequests == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_AssignmentRequests);
        }

        // GET: Tbl_Place_AssignmentRequests/Create
        public ActionResult Create()
        {
            ViewBag.id_code = new SelectList(db.Tbl_Place_Code, "id_code", "data_coord");
            ViewBag.id_user = new SelectList(db.Tbl_Place_Users, "id_users", "id_employee");
            return View();
        }

        // POST: Tbl_Place_AssignmentRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Tbl_Place_AssignmentRequests ass)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Place_AssignmentRequests.Add(ass);
                db.SaveChanges();

                //AQUI LO DEL MAIL!!!!!!!!!!!!!!!!!
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
                const string TemplateMail = @"
                                   <div>
                                       <div> 
                                            <h3 style='color:red; font-size:30px'>TICKET PLACE</h3>
                                       </div>
                                       <section>
                                            <h3>Ha solicitado un lugar con los siguientes datos</h3>
                                            <table border='1'>
                                                <thead style='background:##0f94e6,color:white'; >
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

                //ApiEmployeeModel employeeModel = new ApiEmployeeModel();
                //var jsonResponse = String.Empty;
                //jsonResponse = await _users.GetApiEmployee(ass.Tbl_Place_Users.id_employee);
                //employeeModel = JsonConvert.DeserializeObject<ApiEmployeeModel>(jsonResponse);
                try
                {
                    var client = new SmtpClient();
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(From);
                    mailMessage.To.Add(string.Format(mailToSend, findMail.id));
                    mailMessage.Subject = "Ticket gestion de lugares";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = string.Format(TemplateMail, findMail.area, findMail.subarea,
                        findMail.fsnk, findMail.code);
                    client.Send(mailMessage);

                }
                catch (Exception ex)
                {
                    return Content("Error ",ex.Message);
                }
                //return RedirectToAction("Index");
            }



            return View(ass);
        }

        // GET: Tbl_Place_AssignmentRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_AssignmentRequests tbl_Place_AssignmentRequests = db.Tbl_Place_AssignmentRequests.Find(id);
            if (tbl_Place_AssignmentRequests == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_code = new SelectList(db.Tbl_Place_Code, "id_code", "data_coord", tbl_Place_AssignmentRequests.id_code);
            ViewBag.id_user = new SelectList(db.Tbl_Place_Users, "id_users", "id_employee", tbl_Place_AssignmentRequests.id_user);
            return View(tbl_Place_AssignmentRequests);
        }

        // POST: Tbl_Place_AssignmentRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_assreq,id_code,id_user")] Tbl_Place_AssignmentRequests tbl_Place_AssignmentRequests)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_AssignmentRequests).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_code = new SelectList(db.Tbl_Place_Code, "id_code", "data_coord", tbl_Place_AssignmentRequests.id_code);
            ViewBag.id_user = new SelectList(db.Tbl_Place_Users, "id_users", "id_employee", tbl_Place_AssignmentRequests.id_user);
            return View(tbl_Place_AssignmentRequests);
        }

        // GET: Tbl_Place_AssignmentRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_AssignmentRequests tbl_Place_AssignmentRequests = db.Tbl_Place_AssignmentRequests.Find(id);
            if (tbl_Place_AssignmentRequests == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Place_AssignmentRequests);
        }

        // POST: Tbl_Place_AssignmentRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Place_AssignmentRequests assignmentRequest = db.Tbl_Place_AssignmentRequests.Find(id);

            //AQUI LO DEL MAIL!!!!!!!!!!!!!!!!!
            const string TemplateMail = @"
                                   <body>
                                       <div> 
                                            <h3>Aviso de su peticion de lugar</h3>
                                       </div>
                                       <section>
                                            <h3 style='color:red;'>HA SIDO RECHAZADO</h3>
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
                                   </body>
                                 ";
            var mailToSend = @"{0}@sensata.com";
            try
            {
                //ApiEmployeeModel employeeModel = new ApiEmployeeModel();
                //var jsonResponse = String.Empty;
                //jsonResponse = await _users.GetApiEmployee(assignmentRequest.Tbl_Place_Users.id_employee);
                //employeeModel = JsonConvert.DeserializeObject<ApiEmployeeModel>(jsonResponse);

                var client = new SmtpClient();
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(From);
                //mailMessage.To.Add(string.Format(mailToSend, assignmentRequest.Tbl_Place_Users.id_employee));
                mailMessage.To.Add(assignmentRequest.Tbl_Place_Users.id_employee+"@sensata.com");
                mailMessage.Subject = "Ticket gestion de lugares";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = string.Format(TemplateMail, assignmentRequest.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Area_.name_area, assignmentRequest.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Subarea.name_subarea,
                    assignmentRequest.Tbl_Place_Code.Tbl_Place_Diagram.piso_snorkel, assignmentRequest.Tbl_Place_Code.code);
                client.Send(mailMessage);

                db.Tbl_Place_AssignmentRequests.Remove(assignmentRequest);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorNotFound", "Home");
            }
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
