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
        /// <summary>
        /// list function for all programs
        /// </summary>
        /// <returns>a view of all programs listed</returns>
        // GET: Programs/List
        public ActionResult List()
        {
            string url = "ProgramsData/ListPrograms";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ProgramsDto> Programs = response.Content.ReadAsAsync<IEnumerable<ProgramsDto>>().Result;

            return View(Programs);

        }
        /// <summary>
        /// details function for a specified program
        /// </summary>
        /// <param name="id">program id</param>
        /// <returns>a view of one program details specified by id, including volunteers and services related to it</returns>
        // GET: Programs/Details/5
        public ActionResult Details(int id)
        {
            DetailsProgram ViewModel = new DetailsProgram();

            string url = "ProgramsData/FindProgram/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ProgramsDto SelectedProgram = response.Content.ReadAsAsync<ProgramsDto>().Result;
            ViewModel.SelectedProgram = SelectedProgram;

            //show Services related to this program
            url = "servicesdata/listservicesforprogram/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ServicesDto> RelatedServices = response.Content.ReadAsAsync<IEnumerable<ServicesDto>>().Result;

            ViewModel.RelatedServices = RelatedServices;
            
            //Show Volunteers related to this program
            url = "Volunteersdata/listVolunteersforprogram/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<VolunteersDto> RelatedVolunteers = response.Content.ReadAsAsync<IEnumerable<VolunteersDto>>().Result;
            ViewModel.RelatedVolunteers = RelatedVolunteers;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// when creating a new program, this function provides the department options to the view which allows the user to fill in the other fields about a program
        /// </summary>
        /// <returns>lists departments to add to the program information before creating a new program in the database</returns>
        // GET: Programs/New
        public ActionResult New()
        {
            //GET all departments to choose from when creating
            string url = "DepartmentsData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            return View(DepartmentOptions);
        }
        /// <summary>
        /// creates new program in the database using the api function to add a new program
        /// </summary>
        /// <param name="program">program model</param>
        /// <returns>new program in the database</returns>
        // POST: Programs/Create
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
        /// <summary>
        /// finds the program by id, supplies a page for editing programs, supplies departments to select from when editing
        /// </summary>
        /// <param name="id">program id</param>
        /// <returns>edit page for a specific program</returns>
        // GET: Programs/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateProgram ViewModel = new UpdateProgram();

            string url = "ProgramsData/FindProgram/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProgramsDto SelectedProgram = response.Content.ReadAsAsync<ProgramsDto>().Result;
            ViewModel.SelectedProgram = SelectedProgram;

            //departments to choose from when editing
            url = "DepartmentsData/ListDepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;
            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }
        /// <summary>
        /// applies changes made in Edit function to program in database
        /// </summary>
        /// <param name="id">program id</param>
        /// <param name="program">program model</param>
        /// <returns>update program information and returns to list</returns>
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
        /// <summary>
        /// confirms the deletion of a specific program
        /// </summary>
        /// <param name="id">program id</param>
        /// <returns>the selected program that is going to be deleted</returns>
        // GET: Programs/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Programsdata/findProgram/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProgramsDto SelectedProgram = response.Content.ReadAsAsync<ProgramsDto>().Result;

            return View(SelectedProgram);
        }
        /// <summary>
        /// deletes the program from the database
        /// </summary>
        /// <param name="id">program id</param>
        /// <returns>the program is removed from the database and redirects to the list function</returns>
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