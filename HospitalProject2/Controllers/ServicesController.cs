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
using HospitalProject2.Migrations;

namespace HospitalProject2.Controllers
{
    public class ServicesController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ServicesController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }
        /// <summary>
        /// list function for all services
        /// </summary>
        /// <returns>a view of all services</returns>
        // GET: Services/List
        public ActionResult List()
        {
            string url = "servicesdata/ListServices";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ServicesDto> Services = response.Content.ReadAsAsync<IEnumerable<ServicesDto>>().Result;

            return View(Services);
        }
        /// <summary>
        /// details function for a specified service
        /// </summary>
        /// <param name="id">service id</param>
        /// <returns>view of one service details by id</returns>
        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            DetailsService ViewModel = new DetailsService();

            string url = "servicesdata/findservices/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ServicesDto SelectedService = response.Content.ReadAsAsync<ServicesDto>().Result;
            ViewModel.SelectedService = SelectedService;

            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// when creating a new service, this function provides the program options to the view which allows the user to fill in the other fields about a service
        /// </summary>
        /// <returns>lists programs to add to the service information before creating new service</returns>
        // GET: Programs/New
        public ActionResult New()
        {
            //GET all programs to choose from when creating
            string url = "programsdata/listprograms";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ProgramsDto> ProgramOptions = response.Content.ReadAsAsync<IEnumerable<ProgramsDto>>().Result;

            return View(ProgramOptions);
        }
        /// <summary>
        /// creates new service using api fucntion to add a new service
        /// </summary>
        /// <param name="service">service model</param>
        /// <returns>new service in the database and returns to list</returns>
        // POST: Services/Create
        [HttpPost]
        public ActionResult Create(Services service)
        {
            string url = "servicesdata/addservice";
            string jsonpayload = jss.Serialize(service);

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
        /// finds service by id and supplies a list of programs to choose from
        /// </summary>
        /// <param name="id">service id</param>
        /// <returns>view for updating a service</returns>
        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            UpdateService ViewModel = new UpdateService();

            string url = "servicesdata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServicesDto SelectedService = response.Content.ReadAsAsync<ServicesDto>().Result;
            ViewModel.SelectedService = SelectedService;

            //departments to choose from when creating
            url = "ProgramsData/ListPrograms";
            response = client.GetAsync(url).Result;
            IEnumerable<ProgramsDto> ProgramOptions = response.Content.ReadAsAsync<IEnumerable<ProgramsDto>>().Result;
            ViewModel.ProgramOptions = ProgramOptions;

            return View(ViewModel);
        }
        /// <summary>
        /// applies changes made in Edit function to the selected service
        /// </summary>
        /// <param name="id">service id</param>
        /// <param name="service">service model</param>
        /// <returns>updated info in service list</returns>
        // POST: Services/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Services service)
        {
            string url = "servicesdata/updateservice/" + id;
            string jsonpayload = jss.Serialize(service);

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
        /// confirms the deletion of a specific service
        /// </summary>
        /// <param name="id">service id</param>
        /// <returns>the selected service that is going to be deleted</returns>
        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            string url = "servicesdata/deleteservice/" + id;
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
        /// <summary>
        /// deletes the service
        /// </summary>
        /// <param name="id">service id</param>
        /// <returns>service is removed from list</returns>
        // POST: Services/DeleteConfirm/5
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "servicesdata/findservice/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ServicesDto SelectedService = response.Content.ReadAsAsync<ServicesDto>().Result;

            return View(SelectedService);
        }
    }
}
