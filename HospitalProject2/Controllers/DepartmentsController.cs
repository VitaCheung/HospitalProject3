using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HospitalProject2.Models;

namespace HospitalProject2.Controllers
{
    public class DepartmentsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DepartmentsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/DepartmentsData/");
        }
        // GET: Departments/List
        public ActionResult List()
        {
            //objective: communication with our department data api to retrieve a list of departments
            //curl: https://localhost:44398/api/DepartmentsData/ListDepartments

            string url = "ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DepartmentsDto> Departments = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            return View(Departments);
        }

        // GET: Departments/Departments/5
        public ActionResult Details(int id)
        {
            //objective: communication with our Department data api to retrieve one department
            //curl: https://localhost:44398/api/DepartmentsData/FindDepartment/{id}

            string url = "FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentsDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentsDto>().Result;


            //Show careers related to this department
            url = "careersdata/listcareersfordepartment/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CareersDto> RelatedCareers = response.Content.ReadAsAsync<IEnumerable<CareersDto>>().Result;
            //ViewModel.RelatedCareers = RelatedCareers;

            return View(SelectedDepartment);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Departments/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Departments/Create
        [HttpPost]
        public ActionResult Create(Departments Department)
        {
            // objective: add a new Department into our system using our API
            //curl: -H "Content-Type:application/json" -d @departments.json https://localhost:44398/api/DepartmentsData/AddDepartment

            string url = "AddDepartment";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(Department);

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

        // GET: Departments/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FindDepartments/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentsDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentsDto>().Result;
            return View(SelectedDepartment);
        }

        // POST: Departments/Update/5
        [HttpPost]
        public ActionResult Update(int id, Departments Department)
        {
            string url = "UpdateDepartments/" + id;
            string jsonpayload = jss.Serialize(Department);

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


        // GET: Departments/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentsDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentsDto>().Result;

            return View(SelectedDepartment);
        }

        // POST: Departments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "deleteDepartments/" + id;
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
