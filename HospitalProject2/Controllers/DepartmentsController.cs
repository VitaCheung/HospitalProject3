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
    public class DepartmentsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DepartmentsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }
        // GET: Departments/List
        public ActionResult List()
        {
            //objective: communication with our department data api to retrieve a list of departments
            //curl: https://localhost:44398/api/DepartmentsData/ListDepartments

            string url = "DepartmentsData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DepartmentsDto> Departments = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            return View(Departments);
        }

        // GET: Departments/Departments/5
        public ActionResult Details(int id)
        {
            //objective: communication with our Department data api to retrieve one department
            //curl: https://localhost:44398/api/DepartmentsData/FindDepartment/{id}

            DetailsDepartment ViewModel = new DetailsDepartment();

            string url = "DepartmentsData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentsDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentsDto>().Result;
            ViewModel.SelectedDepartment = SelectedDepartment;


            //Show careers related to this department
            url = "careersdata/listcareersfordepartment/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CareersDto> RelatedCareers = response.Content.ReadAsAsync<IEnumerable<CareersDto>>().Result;
            ViewModel.RelatedCareers = RelatedCareers;

            //Show programs related to this department
            //url = "programsdata/listprogramsfordepartment/" + id;
            //response = client.GetAsync(url).Result;
            //IEnumerable<ProgramsDto> RelatedPrograms = response.Content.ReadAsAsync<IEnumerable<ProgramsDto>>().Result;
            //ViewModel.RelatedPrograms = RelatedPrograms;
         
         
            return View(ViewModel);         
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

            string url = "DepartmentsData/AddDepartment";

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
            string url = "DepartmentsData/FindDepartments/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentsDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentsDto>().Result;
            return View(SelectedDepartment);
        }

        // POST: Departments/Update/5
        [HttpPost]
        public ActionResult Update(int id, Departments Department)
        {
            string url = "DepartmentsData/UpdateDepartments/" + id;
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
            string url = "DepartmentsData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentsDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentsDto>().Result;

            return View(SelectedDepartment);
        }

        // POST: Departments/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DepartmentsData/deleteDepartments/" + id;
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
