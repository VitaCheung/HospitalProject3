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
    public class CareersController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CareersController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }

        // GET: Careers/List
        public ActionResult List()
        {
            //objective: communicate with our Careers data api to retrieve a list of Careers
            //curl https://localhost:44398/api/careersdata/listcareers

            string url = "careersdata/listcareers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CareersDto> Careers = response.Content.ReadAsAsync<IEnumerable<CareersDto>>().Result;


            return View(Careers);
        }

        // GET: Careers/Details/5
        public ActionResult Details(int id)
        {
            DetailsCareers ViewModel = new DetailsCareers();
            //communicate with price data api to retrieve price
            //curl https://localhost:44398/api/careersdata/findcareers/{id}


            string url = "careersdata/findCareers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CareersDto SelectedCareer = response.Content.ReadAsAsync<CareersDto>().Result;
            ViewModel.SelectedCareer = SelectedCareer;

            return View(ViewModel);
        }

        // GET: Careers/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Careers/New      
        public ActionResult New()
        {
            //GET api/Careersdata/listCareers
            //string url = "Careersdata/listCareers";

            //GET all departments to choose from when creating
            string url = "departmentsdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            return View(DepartmentOptions);
        }

        // POST: Careers/Create
        [HttpPost]
        public ActionResult Create(Careers careers)
        {
            //Add a new careers into the system using the API
            //curl -H "Content-Type:application/json" -d @careers.json https://localhost:44398/api/careersdata/addcareers
            string url = "careersdata/addcareers";
            string jsonpayload = jss.Serialize(careers);
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

        // GET: Careers/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCareers ViewModel = new UpdateCareers();

            string url = "Careersdata/findCareers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareersDto SelectedCareer = response.Content.ReadAsAsync<CareersDto>().Result;
            ViewModel.SelectedCareer = SelectedCareer;

            //all departments to choose from when creating
            url = "departmentsdata/listdepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        // POST: Careers/Update/5
        [HttpPost]
        public ActionResult Update(int id, Careers careers)
        {
            string url = "careersdata/updatecareers/" + id;
            string jsonpayload = jss.Serialize(careers);

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

        // GET: Careers/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Careersdata/findCareers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CareersDto SelectedCareer = response.Content.ReadAsAsync<CareersDto>().Result;

            return View(SelectedCareer);
        }

        // POST: Careers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Careersdata/deleteCareers/" + id;
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
