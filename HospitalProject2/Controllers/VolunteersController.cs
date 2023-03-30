using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalProject2.Models;
using HospitalProject2.Models.ViewModels;
using System.Web.Script.Serialization;

namespace HospitalProject2.Controllers
{
    public class VolunteersController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static VolunteersController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }

        // GET: Volunteers/List
        public ActionResult List()
        {
            //objective: communicate with our volunteers data api to retrieve a list of volunteers
            //curl https://localhost:44398/api/careersdata/listvolunteers

            string url = "volunteersdata/listvolunteers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<VolunteersDto> Volunteers = response.Content.ReadAsAsync<IEnumerable<VolunteersDto>>().Result;


            return View(Volunteers);
        }

        // GET: Volunteers/Details/5
        public ActionResult Details(int id)
        {
            DetailsVolunteers ViewModel = new DetailsVolunteers();
            //communicate with price data api to retrieve price
            //curl https://localhost:44398/api/careersdata/findcareers/{id}


            string url = "Volunteersdata/findVolunteers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            VolunteersDto SelectedVolunteer = response.Content.ReadAsAsync<VolunteersDto>().Result;
            ViewModel.SelectedVolunteer = SelectedVolunteer;

            return View(ViewModel);
        }

        // GET: Volunteers/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Volunteers/New    
        public ActionResult New()
        {
            //GET api/Volunteersdata/listVolunteers
            //string url = "Volunteersdata/listVolunteers";

            //GET all departments to choose from when creating
            string url = "departmentsdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            return View(DepartmentOptions);
        }

        // POST: Volunteers/Create
        [HttpPost]
        public ActionResult Create(Volunteers volunteers)
        {
            //Add a new Volunteer into the system using the API
            //curl -H "Content-Type:application/json" -d @careers.json https://localhost:44398/api/Volunteersdata/addVolunteers
            string url = "Volunteersdata/addVolunteers";
            string jsonpayload = jss.Serialize(volunteers);
            Debug.WriteLine(jsonpayload);

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

        // GET: Volunteers/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateVolunteers ViewModel = new UpdateVolunteers();

            string url = "Volunteersdata/findVolunteers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VolunteersDto SelectedVolunteer = response.Content.ReadAsAsync<VolunteersDto>().Result;
            ViewModel.SelectedVolunteer = SelectedVolunteer;

            //all departments to choose from when creating
            url = "departmentsdata/listdepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        // POST: Volunteers/Update/5
        [HttpPost]
        public ActionResult Update(int id, Volunteers volunteers)
        {
            string url = "Volunteersdata/updateVolunteers/" + id;
            string jsonpayload = jss.Serialize(volunteers);

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

        // GET: Volunteers/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Volunteersdata/findVolunteers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VolunteersDto SelectedVolunteer = response.Content.ReadAsAsync<VolunteersDto>().Result;

            return View(SelectedVolunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Volunteersdata/deleteVolunteers/" + id;
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
