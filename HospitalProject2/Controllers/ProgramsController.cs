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
    public class ProgramsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ProgramsController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }
        // GET: Programs/List
        public ActionResult List()
        {
            string url = "ProgramsData/ListPrograms";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ProgramsDto> Programs = response.Content.ReadAsAsync<IEnumerable<ProgramsDto>>().Result;

            return View(Programs);
        }

        // GET: Programs/FindProgram/5
        public ActionResult Details(int id)
        {
            DetailsProgram ViewModel = new DetailsProgram();

            string url = "ProgramsData/FindProgram/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ProgramsDto SelectedProgram = response.Content.ReadAsAsync<ProgramsDto>().Result;
            ViewModel.SelectedProgram = SelectedProgram;

            //Show Volunteers related to this program
            url = "Volunteersdata/listVolunteersforprogram/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<VolunteersDto> RelatedVolunteers = response.Content.ReadAsAsync<IEnumerable<VolunteersDto>>().Result;
            ViewModel.RelatedVolunteers = RelatedVolunteers;

            return View(ViewModel);
        }
        //// POST: Programs/Associate/{programid}
        //[HttpPost]
        //public ActionResult Associate(int id, int department_id)
        //{
        //    string url = "programsdata/associateprogramwithdepartment/" + id + "/" + department_id;
        //    HttpContent content = new StringContent("");
        //    content.Headers.ContentType.MediaType = "application/json";
        //    HttpResponseMessage response = client.PostAsync(url, content).Result;

        //    return RedirectToAction("Details/" + id);

        //}
        // GET: Programs/UnAssociate/{id}?department_id={department_id}
        //[HttpGet]
        //public ActionResult UnAssociate(int id, int department_id)
        //{
        //    string url = "programsdata/unassociateprogramwithdepartment/" + id + "/" + department_id;
        //    HttpContent content = new StringContent("");
        //    content.Headers.ContentType.MediaType = "application/json";
        //    HttpResponseMessage response = client.PostAsync(url, content).Result;

        //    return RedirectToAction("Details/" + id);
        //}

        public ActionResult Error()
        {
            return View();
        }

        // GET: Programs/New
        public ActionResult New()
        {
            //GET all departments to choose from when creating
            string url = "DepartmentsData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            return View(DepartmentOptions);
        }

        [HttpPost]
        public ActionResult Create(Programs program)
        {
            string url = "ProgramsData/AddProgram";
            string jsonpayload = jss.Serialize(program);

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
        // GET: Programs/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateProgram ViewModel = new UpdateProgram();

            string url = "ProgramsData/FindProgram/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProgramsDto SelectedProgram = response.Content.ReadAsAsync<ProgramsDto>().Result;
            ViewModel.SelectedProgram = SelectedProgram;

            //departments to choose from when creating
            url = "DepartmentsData/ListDepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;
            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }
        // POST: Programs/Update/5
        [HttpPost]
        public ActionResult Update(int id, Programs program)
        {
            string url = "programsdata/updateprogram/" + id;
            string jsonpayload = jss.Serialize(program);

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
        // GET: Programs/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Programsdata/findProgram/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProgramsDto SelectedProgram = response.Content.ReadAsAsync<ProgramsDto>().Result;

            return View(SelectedProgram);
        }

        // POST: Programs/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Programsdata/deleteProgram/" + id;
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