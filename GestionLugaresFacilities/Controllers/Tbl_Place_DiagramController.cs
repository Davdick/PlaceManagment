using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using GestionLugaresFacilities.Models;
using System.Threading.Tasks;

namespace GestionLugaresFacilities.Controllers
{
    public class Tbl_Place_DiagramController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        private Tbl_Place_UsersController _users;
        public Tbl_Place_DiagramController()
        {
            _users = new Tbl_Place_UsersController();
        }
        [Authorization]



        // GET: Tbl_Place_Diagram
        public ActionResult Index()
        {
            ViewBag.id_area = new SelectList(db.Tbl_Place_Area_, "id_area", "name_area");
            ViewBag.id_subarea = new SelectList(db.Tbl_Place_Subarea, "id_subarea", "name_subarea");
            var tbl_Place_Diagram = db.Tbl_Place_Diagram.Include(t => t.Tbl_Place_Area_).Include(t => t.Tbl_Place_Subarea);
            return View(tbl_Place_Diagram.ToList());
        }

        // GET: Tbl_Place_Diagram/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Diagram diagram = db.Tbl_Place_Diagram.Find(id);
            var codes = db.Tbl_Place_Code.Where(c => c.id_diagram == id)
                         .Select(c => new {
                             dataC = c.data_coord,
                             codeN = c.code
                         })
                         .ToList();
            var codes_n = db.Tbl_Place_Code.Where(c => c.id_diagram == id)
                         .Select(c => new {
                             codeDat = c.code,
                         })
                         .ToList();
            var codes_id = db.Tbl_Place_Code.Where(c => c.id_diagram == id)
                         .Select(c => new {
                             id = c.id_code,
                         })
                         .ToList();

            if (diagram == null)
            {
                return HttpNotFound();
            }
            ViewBag.size_x = diagram.size_x;
            ViewBag.size_y = diagram.size_y;
            ViewBag.data_infos = JsonConvert.SerializeObject(diagram.data_info);
            ViewBag.data_coord = codes;
            ViewBag.codes1 = codes_n;
            ViewBag.codes_id = codes_id;
            return View(diagram);
        }
        public ActionResult editAdmin(string area, string subarea, int? piso_snk, string code, int? id, string viewTicket)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            ///////////////////////
            bool backIndex = false;
            bool viewT;
            if (viewTicket!=null)
            {
                viewT = bool.Parse(viewTicket);
            }
            else
            {
                 viewT = false;
            }
           
            var idDiagram = db.Tbl_Place_Diagram
                              .Where(c => c.Tbl_Place_Area_.name_area == area && c.Tbl_Place_Subarea.name_subarea == subarea && c.piso_snorkel == piso_snk)
                       .Select(c => new {
                           c.id_diagram,
                       })
                       .FirstOrDefault();
            if (subarea == null || subarea == "")
            {
                idDiagram = db.Tbl_Place_Diagram
                            .Where(c => c.Tbl_Place_Area_.name_area == area && c.piso_snorkel == piso_snk)
                            .Select(c => new {
                                id_diagram = c.id_diagram
                            })
                            .FirstOrDefault();
                backIndex = true;
            }

            if (code != null)
            {
                idDiagram = db.Tbl_Place_Assignment
                              .Where(c => c.Tbl_Place_Users.id_employee == code || c.Tbl_Place_Code.code == code)
                       .Select(c => new {
                           id_diagram = c.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram,
                       })
                       .FirstOrDefault();
                backIndex = true;
                if (idDiagram == null)
                {
                    return RedirectToAction("ErrorNotFound", "Home");
                }
            }
            if (id != null)
            {
                idDiagram = db.Tbl_Place_Diagram
                                             .Where(c => c.id_diagram == id)
                                      .Select(c => new {
                                          id_diagram = c.id_diagram,
                                      })
                                      .FirstOrDefault();


            }
            if (idDiagram == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ErrorNotFound", "Home");
            }
            ///////////////////////

            Tbl_Place_Diagram diagram = db.Tbl_Place_Diagram.Find(idDiagram.id_diagram);
            var codes = db.Tbl_Place_Code.Where(c => c.id_diagram == diagram.id_diagram)
                         .Select(c => new {
                             dataC = c.data_coord,
                             codeN = c.code
                         })
                         .ToList();
            var codes_n = db.Tbl_Place_Code.Where(c => c.id_diagram == diagram.id_diagram)
                         .Select(c => new {
                             codeDat = c.code,
                             typePlace = c.type_place
                         })
                         .ToList();
            var codes_id = db.Tbl_Place_Code.Where(c => c.id_diagram == diagram.id_diagram)
                         .Select(c => new {
                             id = c.id_code,

                         })
                         .ToList();
            ///////////////
            var assignments = db.Tbl_Place_Assignment
                .Where(a => a.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram == idDiagram.id_diagram)
                .ToList();

            var findCoordsEmployee = db.Tbl_Place_Assignment
                              .Where(c => c.Tbl_Place_Users.id_employee == code || c.Tbl_Place_Code.code == code)
                       .Select(c => new {
                           coordsEmp = c.Tbl_Place_Code.data_coord,
                       })
                       .FirstOrDefault();

            ///////////////

            //ViewBag.subarea = diagram.Tbl_Place_Subarea.name_subarea;
            try
            {
                ViewBag.subarea = diagram.Tbl_Place_Subarea.name_subarea;
            }
            catch (NullReferenceException ex)
            {
                ViewBag.subarea = "NA";
            }
            ViewBag.size_x = diagram.size_x;
            ViewBag.size_y = diagram.size_y;
            ViewBag.data_infos = JsonConvert.SerializeObject(diagram.data_info);
            ViewBag.data_coord = codes;
            ViewBag.codes1 = codes_n;
            ViewBag.codes_id = codes_id;



            ViewBag.type_place = new SelectList(db.Tbl_Place_TypePlace, "Id", "Type");
            ViewBag.idDiagram = diagram.id_diagram;
            ViewBag.back = backIndex;
            ViewBag.backTicket = viewT;
            ViewBag.area = diagram.Tbl_Place_Area_.name_area;
           // ViewBag.subarea = diagram.Tbl_Place_Subarea.name_subarea;
            ViewBag.piso_snk = diagram.piso_snorkel;
            ViewBag.coordsEmp = findCoordsEmployee;
            

            return View(diagram);
        }
        [HttpGet]
        public JsonResult GetTypePlaceList()
        {
            List<SelectListItem> items = db.Tbl_Place_TypePlace
                .Select(x => new SelectListItem()
                {
                    Text = x.Type,
                    Value = x.Id.ToString()
                })
                .ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDiagramAreasPlanta()
        {
            
            
            var data = db.Tbl_Place_Diagram
             .Include(t => t.Tbl_Place_Area_)
             .Include(t => t.Tbl_Place_Subarea)
             .Where(t => t.Tbl_Place_Area_.name_area == "Planta Manufactura AGS")
             .ToList()
             .Select(p => new DiagramArea
             {
                 Id = p.id_area,
                 Area = p.Tbl_Place_Area_.name_area,
                 Subarea = p.Tbl_Place_Subarea.name_subarea,
                 Floor = p.piso_snorkel
             }).ToList();


            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDiagramAreasCB()
        {
            ViewBag.id_area = new SelectList(db.Tbl_Place_Area_, "id_area", "name_area");
            ViewBag.id_subarea = new SelectList(db.Tbl_Place_Subarea, "id_subarea", "name_subarea");
            var tbl_Place_Diagram = db.Tbl_Place_Diagram.Include(t => t.Tbl_Place_Area_).Include(t => t.Tbl_Place_Subarea)
                .Where(t => t.Tbl_Place_Area_.name_area == "Central Building");
            return Json(tbl_Place_Diagram, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JUASview(string area, string subarea, int? piso_snk, string code, int? id)
        {

            var idDiagram = db.Tbl_Place_Diagram
                              .Where(c => c.Tbl_Place_Area_.name_area == area && c.Tbl_Place_Subarea.name_subarea == subarea && c.piso_snorkel == piso_snk)
                       .Select(c => new {
                           c.id_diagram,
                       })
                       .FirstOrDefault();
            if (subarea != null)
            {
                idDiagram = db.Tbl_Place_Diagram
                            .Where(c => c.Tbl_Place_Area_.name_area == area && c.piso_snorkel == piso_snk)
                            .Select(c => new {
                                id_diagram = c.id_diagram
                            })
                            .FirstOrDefault();
            }
         
            if (code != null)
            {
                idDiagram = db.Tbl_Place_Assignment
                              .Where(c => c.Tbl_Place_Users.id_employee == code || c.Tbl_Place_Code.code == code)
                       .Select(c => new {
                           id_diagram = c.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram,
                       })
                       .FirstOrDefault();
                if (idDiagram == null)
                {
                    return RedirectToAction("ErrorNotFound", "Home");
                }
            }
            if (id != null)
            {
                idDiagram = db.Tbl_Place_Diagram
                                             .Where(c => c.id_diagram == id)
                                      .Select(c => new {
                                          id_diagram = c.id_diagram,
                                      })
                                      .FirstOrDefault();


            }
            if (idDiagram == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ErrorNotFound", "Home");
            }
            Tbl_Place_Diagram diagram = db.Tbl_Place_Diagram.Find(idDiagram.id_diagram);

            var assignments = db.Tbl_Place_Assignment
                .Where(a => a.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram == idDiagram.id_diagram)
                .ToList();
            var codes = db.Tbl_Place_Code.Where(c => c.id_diagram == idDiagram.id_diagram)
                         .Select(c => new {
                             dataC = c.data_coord,
                             codeN = c.code
                         })
                         .ToList();
            var findCoordsEmployee = db.Tbl_Place_Assignment
                              .Where(c => c.Tbl_Place_Users.id_employee == code || c.Tbl_Place_Code.code == code)
                       .Select(c => new {
                           coordsEmp = c.Tbl_Place_Code.data_coord,
                       })
                       .FirstOrDefault();
            var codes_id = db.Tbl_Place_Code.Where(c => c.id_diagram == idDiagram.id_diagram)
                         .Select(c => new {
                             id = c.id_code,
                         })
                         .ToList();

            if (diagram == null)
            {
                return RedirectToAction("ErrorNotFound", "Home");
            }

            ViewBag.size_x = diagram.size_x;
            ViewBag.size_y = diagram.size_y;
            ViewBag.data_infos = diagram.data_info;
            ViewBag.idDiagram = diagram.id_diagram;
            ViewBag.data_coord = codes;
            ViewBag.area = diagram.Tbl_Place_Area_.name_area;
            ViewBag.subarea = diagram.Tbl_Place_Subarea.name_subarea;
            ViewBag.piso_snk = diagram.piso_snorkel;
            ViewBag.coordsEmp = findCoordsEmployee;
            ViewBag.codes_id = codes_id;

            return View(diagram);
        }

        public ActionResult Create()
        {
            ViewBag.id_area = new SelectList(db.Tbl_Place_Area_, "id_area", "name_area");
            ViewBag.id_subarea = new SelectList(db.Tbl_Place_Subarea, "id_subarea", "name_subarea");
            return View();
        }

        // POST: Tbl_Place_Diagram/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Tbl_Place_Diagram diagram)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Place_Diagram.Add(diagram);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_area = new SelectList(db.Tbl_Place_Area_, "id_area", "name_area", diagram.id_area);
            ViewBag.id_subarea = new SelectList(db.Tbl_Place_Subarea, "id_subarea", "name_subarea", diagram.id_subarea);

            return RedirectToAction("Index");
        }
     

        // GET: Tbl_Place_Diagram/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Diagram tbl_Place_Diagram = db.Tbl_Place_Diagram.Find(id);
            if (tbl_Place_Diagram == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_area = new SelectList(db.Tbl_Place_Area_, "id_area", "name_area", tbl_Place_Diagram.id_area);
            ViewBag.id_subarea = new SelectList(db.Tbl_Place_Subarea, "id_subarea", "name_subarea", tbl_Place_Diagram.id_subarea);
            ViewBag.data_infos = tbl_Place_Diagram.data_info;
            return View(tbl_Place_Diagram);
        }

        // POST: Tbl_Place_Diagram/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_diagram,id_area,id_subarea,piso_snorkel,size_x,size_y,data_info")] Tbl_Place_Diagram tbl_Place_Diagram)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Place_Diagram).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_area = new SelectList(db.Tbl_Place_Area_, "id_area", "name_area", tbl_Place_Diagram.id_area);
            ViewBag.id_subarea = new SelectList(db.Tbl_Place_Subarea, "id_subarea", "name_subarea", tbl_Place_Diagram.id_subarea);
            return View(tbl_Place_Diagram);
        }
        [HttpPost]
        public ActionResult RemoveCell(int idDiagram, string data)
        {
            Tbl_Place_Diagram tbl_Place_Diagram = db.Tbl_Place_Diagram.Find(idDiagram);
            var datoEliminar = JsonConvert.DeserializeObject<DataJSON>(data);
            // Deserializar la lista total de datos a una lista de objetos
            var datosTotales = JsonConvert.DeserializeObject<List<DataJSON>>(tbl_Place_Diagram.data_info);
            // Encontrar y eliminar el objeto que coincide con el dato a eliminar
            Validators equal = new Validators();
            datosTotales = datosTotales.Where(d => !equal.IsEqual(d, datoEliminar)).ToList();
            // Serializar la lista resultante a una cadena JSON
            string datosFinalesString = JsonConvert.SerializeObject(datosTotales);

            tbl_Place_Diagram.data_info = datosFinalesString ;
            db.SaveChanges();

            return Json(new { success = true, message = "Datos actualizados exitosamente." });
        }
        [HttpPost]
        public ActionResult AddCells(int idDiagram, string data, int? limitX, int? limitY)
        {
            Tbl_Place_Diagram tbl_Place_Diagram = db.Tbl_Place_Diagram.Find(idDiagram);
            var datosTotales = JsonConvert.DeserializeObject<List<DataJSON>>(tbl_Place_Diagram.data_info);
            if(limitX != null)
            {
                tbl_Place_Diagram.size_x = limitX;
            }
            if (limitY != null)
            {
                tbl_Place_Diagram.size_y = limitY;
            }
            //var datas = JsonConvert.DeserializeObject<DataJSON>(data);
            //tring jsonString = JsonConvert.SerializeObject(datas);
            Validators equal = new Validators();

            try
            {
                List<DataJSON> datosAdd = JsonConvert.DeserializeObject<List<DataJSON>>(data);
                var datosFinales = equal.AddCells(datosTotales, datosAdd);
                string datosFinalesString = JsonConvert.SerializeObject(datosFinales);
                tbl_Place_Diagram.data_info = datosFinalesString;

            }
            catch (JsonException)
            {
                var datosAdd = JsonConvert.DeserializeObject<DataJSON>(data);
                var datosFinales = equal.AddCells(datosTotales, datosAdd);
                string datosFinalesString = JsonConvert.SerializeObject(datosFinales);
                tbl_Place_Diagram.data_info = datosFinalesString;
            }
            

            db.SaveChanges();

            return Json(new { success = true, message = "Datos actualizados exitosamente." });
        }
        public async Task<FileResult> DownloadExcel(Int32 ID)
        {
            // Crear un nuevo archivo de Excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                // Agregar una hoja al archivo de Excel
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                Tbl_Place_Diagram diagram = db.Tbl_Place_Diagram.Find(ID);






                //Llenar encabezados
                worksheet.Cells["A1"].Value = "CODIGO";
                worksheet.Cells["B1"].Value = "AREA";
                worksheet.Cells["C1"].Value = "SUBAREA";
                worksheet.Cells["D1"].Value = "PISO O SNORKEL";
                worksheet.Cells["E1"].Value = "ID";
                worksheet.Cells["F1"].Value = "NOMBRE";
                //worksheet.Cells["G1"].Value = "APELLIDOS";
                worksheet.Cells["G1"].Value = "EMAIL";
                //worksheet.Cells["I1"].Value = "POSICION";
                worksheet.Cells["H1"].Value = "SUPERVISOR";
                worksheet.Cells["I1"].Value = "DISPONIBLES";
                worksheet.Cells["J1"].Value = "OCUPADOS";
                worksheet.Cells["K1"].Value = "TOTAL";






                // Llenar el archivo de Excel con datos (ejemplo)
                //List<Int32> IdsForms = dbRecruitment.Tb_RelGroupForm.Where(x => x.ID_OnboardingGroup == ID_OnboardingGroup).Select(x => x.ID_Form).ToList();
                //List<Tb_FormPost> formPostList = dbRecruitment.Tb_FormPost.Where(x => IdsForms.Contains(x.ID_Form)).ToList();
                List<Tbl_Place_Assignment> assignments = db.Tbl_Place_Assignment
                .Include(a => a.Tbl_Place_Code)
                .Include(a => a.Tbl_Place_Users)
                .Where(a => a.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram == diagram.id_diagram).ToList();



                Int32 row = 2;
                String workShift = String.Empty;



                foreach (var item in assignments)
                {
                    worksheet.Cells[row, 1].Value = item.Tbl_Place_Code.code;
                    worksheet.Cells[row, 2].Value = item.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Area_.name_area;
                    try
                    {
                        worksheet.Cells[row, 3].Value = item.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Subarea.name_subarea;
                    }
                    catch (NullReferenceException ex)
                    {
                        worksheet.Cells[row, 3].Value = "NA";
                    }
                    ApiEmployeeModel employeeModel = new ApiEmployeeModel();
                    var jsonResponse = String.Empty;
                    jsonResponse = await _users.GetApiEmployee(item.Tbl_Place_Users.id_employee);
                    employeeModel = JsonConvert.DeserializeObject<ApiEmployeeModel>(jsonResponse);

                    
                    worksheet.Cells[row, 4].Value = item.Tbl_Place_Code.Tbl_Place_Diagram.piso_snorkel;
                    worksheet.Cells[row, 5].Value = employeeModel.EmployeeNumber;
                    worksheet.Cells[row, 6].Value = employeeModel.EmployeeName;
                    //worksheet.Cells[row, 7].Value = item.Tbl_Place_Users.lastname;
                    worksheet.Cells[row, 7].Value = employeeModel.EmployeeNumber+"@sensata.com";
                    //worksheet.Cells[row, 9].Value = item.Tbl_Place_Users.position;
                    worksheet.Cells[row, 8].Value = employeeModel.SupervisorName;
                    worksheet.Cells[row, 9].Value = String.Empty;
                    worksheet.Cells[row, 10].Value = String.Empty;
                    worksheet.Cells[row, 11].Value = String.Empty;
                    //workShift = item.Tb_Form.Tb_FormPost.First().WorkShift.Replace("WORK_SHIFT-6-30", "");
                    //workShift = workShift + " (Mexico)";



                    worksheet.Cells[row, 13].Value = workShift;





                    row++;
                }



                // Establecer el formato de fecha
                string strDateRange = String.Format("F2:F{0}", row - 1);
                using (var rangeDate = worksheet.Cells[strDateRange])
                {
                    rangeDate.Style.Numberformat.Format = "dd/MM/yyyy";
                }



                //paint all headers
                // Pintar el rango A1:B3 de verde claro
                var range = worksheet.Cells["A1:M1"];

                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(1, 180, 198, 231);





                // Establecer negritas en el encabezado
                worksheet.Cells["A1:N1"].Style.Font.Bold = true;





                // Guardar el archivo de Excel en una memoria en memoria
                var stream = new MemoryStream(package.GetAsByteArray());



                // Descargar el archivo de Excel
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Doc_" + diagram.Tbl_Place_Area_.name_area + "_" + diagram.Tbl_Place_Subarea.name_subarea + "_" + diagram.piso_snorkel + ".xlsx");
            }
        }
        public async Task<FileResult> DownloadExcelAll()
        {
            // Crear un nuevo archivo de Excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                // Agregar una hoja al archivo de Excel
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Recupera todos los registros de la tabla Diagram
                List<Tbl_Place_Diagram> diagram = db.Tbl_Place_Diagram.ToList();







                //Llenar encabezados
                worksheet.Cells["A1"].Value = "CODIGO";
                worksheet.Cells["B1"].Value = "AREA";
                worksheet.Cells["C1"].Value = "SUBAREA";
                worksheet.Cells["D1"].Value = "PISO O SNORKEL";
                worksheet.Cells["E1"].Value = "ID";
                worksheet.Cells["F1"].Value = "NOMBRE";
                //worksheet.Cells["G1"].Value = "APELLIDOS";
                worksheet.Cells["G1"].Value = "EMAIL";
                //worksheet.Cells["I1"].Value = "POSICION";
                worksheet.Cells["H1"].Value = "SUPERVISOR";
                







                // Llenar el archivo de Excel con datos (ejemplo)
                //List<Int32> IdsForms = dbRecruitment.Tb_RelGroupForm.Where(x => x.ID_OnboardingGroup == ID_OnboardingGroup).Select(x => x.ID_Form).ToList();
                //List<Tb_FormPost> formPostList = dbRecruitment.Tb_FormPost.Where(x => IdsForms.Contains(x.ID_Form)).ToList();
                List<Tbl_Place_Assignment> assignments = db.Tbl_Place_Assignment.ToList();



                Int32 row = 2;
                String workShift = String.Empty;



                foreach (var item in assignments)
                {
                    worksheet.Cells[row, 1].Value = item.Tbl_Place_Code.code;
                    worksheet.Cells[row, 2].Value = item.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Area_.name_area;
                    try
                    {
                        worksheet.Cells[row, 3].Value = item.Tbl_Place_Code.Tbl_Place_Diagram.Tbl_Place_Subarea.name_subarea;
                    }
                    catch (NullReferenceException ex)
                    {
                        worksheet.Cells[row, 3].Value = "NA";
                    }
                    ApiEmployeeModel employeeModel = new ApiEmployeeModel();
                    var jsonResponse = String.Empty;
                    jsonResponse = await _users.GetApiEmployee(item.Tbl_Place_Users.id_employee);
                    employeeModel = JsonConvert.DeserializeObject<ApiEmployeeModel>(jsonResponse);

                    worksheet.Cells[row, 4].Value = item.Tbl_Place_Code.Tbl_Place_Diagram.piso_snorkel;
                    worksheet.Cells[row, 5].Value = employeeModel.EmployeeNumber;
                    worksheet.Cells[row, 6].Value = employeeModel.EmployeeName;
                    //worksheet.Cells[row, 7].Value = item.Tbl_Place_Users.lastname;
                    worksheet.Cells[row, 7].Value = employeeModel.EmployeeNumber+"@sensata.com";
                    //worksheet.Cells[row, 9].Value = item.Tbl_Place_Users.position;
                    worksheet.Cells[row, 8].Value = employeeModel.SupervisorName;
                    //workShift = item.Tb_Form.Tb_FormPost.First().WorkShift.Replace("WORK_SHIFT-6-30", "");
                    //workShift = workShift + " (Mexico)";



                    worksheet.Cells[row, 13].Value = workShift;





                    row++;
                }



                

                try
                {
                    // Establecer el formato de fecha
                    string strDateRange = String.Format("F2:F{0}", row - 1);
                    using (var rangeDate = worksheet.Cells[strDateRange])
                    {
                        rangeDate.Style.Numberformat.Format = "dd/MM/yyyy";
                    }
                }
                catch (Exception err)
                {
                    
                }



                //paint all headers
                // Pintar el rango A1:B3 de verde claro
                var range = worksheet.Cells["A1:M1"];

                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(1, 180, 198, 231);





                // Establecer negritas en el encabezado
                worksheet.Cells["A1:N1"].Style.Font.Bold = true;





                // Guardar el archivo de Excel en una memoria en memoria
                var stream = new MemoryStream(package.GetAsByteArray());



                // Descargar el archivo de Excel
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Doc_GL_ALL.xlsx");
            }
        }

        // GET: Tbl_Place_Diagram/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Place_Diagram tbl_Place_Diagram = db.Tbl_Place_Diagram.Find(id);
            if (tbl_Place_Diagram == null)
            {
                return HttpNotFound();
            }
            ViewBag.Alert = null;
            return View(tbl_Place_Diagram);
        }

        // POST: Tbl_Place_Diagram/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Place_Diagram tbl_Place_Diagram = db.Tbl_Place_Diagram.Find(id);
                if(tbl_Place_Diagram != null)
            {
                try{
                    // Elimina todos los datos de la tabla que coincidan con el ID de la consulta.
                    db.Tbl_Place_Assignment.RemoveRange(db.Tbl_Place_Assignment.Where(item => item.Tbl_Place_Code.Tbl_Place_Diagram.id_diagram == tbl_Place_Diagram.id_diagram));
                    db.Tbl_Place_Code.RemoveRange(db.Tbl_Place_Code.Where(item => item.Tbl_Place_Diagram.id_diagram == tbl_Place_Diagram.id_diagram));
                }
                catch (Exception)
                {
                    return RedirectToAction("Home", "ErrorNotFound");
                }

                

            }

            db.Tbl_Place_Diagram.Remove(tbl_Place_Diagram);
            //db.Tbl_Place_Assignment.Remove(tbl_Place_Assignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult DeleteOne(int id,string cell)
        {
            Tbl_Place_Diagram tbl_Place_Diagram = db.Tbl_Place_Diagram.Find(id);
            if (tbl_Place_Diagram != null)
            {
                try
                {
                    var cells = db.Tbl_Place_Code.Select(r => r.data_coord).ToList();

                    // Serializa los registros a formato JSON
                    string jsonResultCells = JsonConvert.SerializeObject(cells, Formatting.Indented);
                    string jsonResultCell = JsonConvert.SerializeObject(cell, Formatting.Indented);

                   var consulta = db.Tbl_Place_Code.SingleOrDefault(p => p.id_diagram == tbl_Place_Diagram.id_diagram);
                    consulta.data_coord = jsonResultCell;
                    db.SaveChanges();
                    
                }
                catch (Exception)
                {
                    return RedirectToAction("Home", "ErrorNotFound");
                }



            }

            //db.Tbl_Place_Diagram.Remove(tbl_Place_Diagram);
            //db.Tbl_Place_Assignment.Remove(tbl_Place_Assignment);
            //db.SaveChanges();
            return RedirectToAction("editAdmin", new {id = tbl_Place_Diagram.id_diagram});
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
