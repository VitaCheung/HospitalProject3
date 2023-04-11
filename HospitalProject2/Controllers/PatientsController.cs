using HospitalProject2.Models;
using HospitalProject2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalProject2.Controllers
{
    public class PatientsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/PatientsData/");
        }


        // GET: Patients/List
        public ActionResult List()
        {
            //objective: communicate with our patients data api to retrieve a list of patients
            //curl https://localhost:44398/api/patientsdata/listpatients

            string url = "ListPatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PatientsDto> Patients = response.Content.ReadAsAsync<IEnumerable<PatientsDto>>().Result;


            return View(Patients);
        }

        // GET: Patients/Details/5
        public ActionResult Details(int id)
        {
            //DetailsPatients ViewModel = new DetailsPatients();
            //communicate with patients data api to retrieve patients
            //curl https://localhost:44398/api/patientsdata/findpatients/{id}


            string url = "FindPatients/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientsDto SelectedPatient = response.Content.ReadAsAsync<PatientsDto>().Result;
            //ViewModel.SelectedCareer = SelectedPatient;

            //Show appointments related to this patients
            url = "appointmentsdata/listappointentsfordpatients/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AppointmentsDto> RelatedCareers = response.Content.ReadAsAsync<IEnumerable<AppointmentsDto>>().Result;
            //ViewModel.RelatedCareers = RelatedCareers;

            return View(SelectedPatient);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Patients/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Patients/Create
        [HttpPost]
        public ActionResult Create(Patients patient)
        {
            // objective: add a new patients into our system using our API
            //curl: -H "Content-Type:application/json" -d @patients.json https://localhost:44398/api/PatientsData/AddPatient

            string url = "AddPatient";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(patient);

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

        // GET: Patients/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FindPatients/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientsDto SelectedPatient = response.Content.ReadAsAsync<PatientsDto>().Result;
            return View(SelectedPatient);
        }

        // POST: Patients/Update/5
        [HttpPost]
        public ActionResult Update(int id, Patients patient)
        {
            string url = "UpdatePatients/" + id;
            string jsonpayload = jss.Serialize(patient);

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

        // GET: Patients/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindPatients/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientsDto SelectedPatient = response.Content.ReadAsAsync<PatientsDto>().Result;

            return View(SelectedPatient);
        }

        // POST: Patients/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeletePatients/" + id;
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
