using HospitalProject2.Models;
using System;
using System.Collections.Generic;
using HospitalProject2.Models.ViewModels;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace HospitalProject2.Controllers
{
    public class AppointmentsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static AppointmentsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/AppointmentsData/");
        }

        // GET: Appointments/List
        public ActionResult List()
        {
            //objective: communication with our staff data api to retrieve a list of staff
            //curl: https://localhost:44398/api/appointmentsdata/listappointments

            string url = "ListAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AppointmentsDto> Appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentsDto>>().Result;

            return View(Appointments);
        }

        // GET: Appointments/FindAppointments/5
        public ActionResult Details(int id)
        {
            //objective: communication with our appointments data api to retrieve one appointment
            //curl: https://localhost:44398/api/appointmentsdata/findappoinments/{id}

            string url = "FindStaff/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AppointmentsDto SelectedAppointments = response.Content.ReadAsAsync<AppointmentsDto>().Result;

            return View(SelectedAppointments);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Appointments/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        public ActionResult Create(Appointments appointment)
        {
            // objective: add a new staff into our system using our API
            //curl: -H "Content-Type:application/json" -d @staffs.json https://localhost:44398/api/StaffsData/AddStaff

            string url = "AddAppointments";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(appointment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
                
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FindAppointments/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentsDto SelectedStaff = response.Content.ReadAsAsync<AppointmentsDto>().Result;
            return View(SelectedStaff);
        }

        // POST: Appointments/Update/5
        [HttpPost]
        public ActionResult Update(int id, Appointments appointment)
        {
            string url = "UpdateAppoinments/" + id;
            string jsonpayload = jss.Serialize(appointment);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointments/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindAppointments/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentsDto SelectedAppointments = response.Content.ReadAsAsync<AppointmentsDto>().Result;

            return View(SelectedAppointments);
        }

        // POST: Appointments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteAppointments/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
