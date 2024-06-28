using GestionLugaresFacilities.Code;
using GestionLugaresFacilities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GestionLugaresFacilities.Controllers
{
    public class BaseController : Controller
    {
        private DbFacilitesSystemEntities db = new DbFacilitesSystemEntities();
        public String GetEmployeeNumber(String AuthCode)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Settings.URL_GetEmployeeNumber);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = 0;
            request.Headers.Add("Authorization", AuthCode);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string responseText = reader.ReadToEnd();
            dynamic model = JsonConvert.DeserializeObject<dynamic>(responseText);
            return model.EmpNum;
        }
        public ActionResult DoLogin(string xid)
        {
            if(db.Tbl_Place_Users.Any(x => x.id_employee.Contains(xid)))
            {
                Tbl_Place_Users user = db.Tbl_Place_Users.Where(x => x.id_employee.Contains(xid)).First();
                Settings.LoggedUser = new UserModel()
                {
                    User = user,
                    Rol = user.id_rol
                };
               
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult EndPoint(String AuthCode, bool KioskoMode = false)
        {
            Settings.KioskoMode = KioskoMode;
            var num_empleado = GetEmployeeNumber(AuthCode).ToLower();
            num_empleado = num_empleado.ToLower().Replace("x", String.Empty).Replace("a", String.Empty);
            return DoLogin(num_empleado);
        }

        public async Task<TokenModel> GetToken()
        {
            TokenModel tokenModel = new TokenModel();
            try
            {

                // Realiza la solicitud GET
                HttpClient _httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(EnviromentVars.URL_BASE_API)
                };

                string url = String.Format("api/{0}?ID_App={1}", "GetToken", EnviromentVars.ID_APP_API);
                var response = await _httpClient.GetAsync(url);

                // Asegúrate de que la solicitud fue exitosa (código de estado 200)
                if (response.IsSuccessStatusCode)
                {
                    // Lee la respuesta como una cadena JSON
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TokenModel>(jsonResponse);
                }
                else
                {
                    // Maneja los errores de acuerdo a tus necesidades
                    //return "Error en la solicitud: " + response.StatusCode.ToString();
                    throw new Exception("Error al solicitar token");
                }
            }
            catch (Exception ex)
            {
                // Maneja las excepciones, por ejemplo, una excepción de red
                //return "Error de red: " + ex.Message;
            }

            return tokenModel;
        }


    }
}