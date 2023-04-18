using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HospitalProject2.Migrations;
using HospitalProject2.Models;
using HospitalProject2.Models.ViewModels;

namespace HospitalProject2.Controllers
{
    public class StaffsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static StaffsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }
        // GET: Staffs/List
        public ActionResult List()
        {
            //objective: communication with our staff data api to retrieve a list of staff
            //curl: https://localhost:44398/api/StaffsData/ListStaffs

            string url = "StaffsData/ListStaffs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<StaffsDto> Staffs = response.Content.ReadAsAsync<IEnumerable<StaffsDto>>().Result;

            return View(Staffs);
        }

        // GET: Staffs/FindStaff/5
        public ActionResult Details(int id)
        {
            //objective: communication with our staff data api to retrieve one staff
            //curl: https://localhost:44398/api/StaffsData/FindStaff/{id}

            string url = "StaffsData/FindStaff/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            StaffsDto SelectedStaff = response.Content.ReadAsAsync<StaffsDto>().Result;

            return View(SelectedStaff);
        } 
        public ActionResult Error()
        {
            return View();
        }

        // GET: Staffs/New
        public ActionResult New()
        {
            // information about all departments in the system
            // GET api/departmentsdata/listdepartments

            string url = "DepartmentsData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            return View(DepartmentOptions);
        }

        // POST: Staffs/Create
        [HttpPost]
        public ActionResult Create(Staffs staff)
        {
            // objective: add a new staff into our system using our API
            //curl: -H "Content-Type:application/json" -d @staffs.json https://localhost:44398/api/StaffsData/AddStaff
           
            string url = "StaffsData/AddStaff";

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(staff);

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

        // GET: Staffs/Edit/5
        public ActionResult Edit(int id)
        {

            UpdateStaff ViewModel = new UpdateStaff();

            // the existing staff information
            string url = "StaffsData/FindStaffs/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffsDto SelectedStaff = response.Content.ReadAsAsync<StaffsDto>().Result;
            ViewModel.SelectedStaff = SelectedStaff;

            // include all departments to choose from updating this staff
            url = "DepartmentsData/ListDepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        // POST: Staffs/Update/5
        [HttpPost]
        public ActionResult Update(int id, Staffs Staff)
        {
            string url = "StaffsData/updatestaffs/" + id;
            string jsonpayload = jss.Serialize(Staff);

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

        // GET: Staffs/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "StaffsData/FindStaffs/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StaffsDto SelectedStaff = response.Content.ReadAsAsync<StaffsDto>().Result;

            return View(SelectedStaff);
        }

        // POST: Staffs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "StaffsData/deleteStaffs/" + id;
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
