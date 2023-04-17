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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            //client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";
            
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Volunteers/List
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult List()
        {
            GetApplicationCookie();
            //objective: communicate with our volunteers data api to retrieve a list of volunteers
            //curl https://localhost:44398/api/careersdata/listvolunteers

            string url = "volunteersdata/listvolunteers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<VolunteersDto> Volunteers = response.Content.ReadAsAsync<IEnumerable<VolunteersDto>>().Result;


            return View(Volunteers);
        }

        // GET: Volunteers/Details/5
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
            DetailsVolunteers ViewModel = new DetailsVolunteers();
            if (User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;

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
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult New()
        {
            //GET api/Volunteersdata/listVolunteers
            //string url = "Volunteersdata/listVolunteers";

            //GET all programs to choose from when creating
            string url = "programsdata/listprograms";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ProgramsDto> programOptions = response.Content.ReadAsAsync<IEnumerable<ProgramsDto>>().Result;

            return View(programOptions);
        }

        // POST: Volunteers/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Create(Volunteers volunteers)
        {
            GetApplicationCookie();
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
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Edit(int id)
        {
            UpdateVolunteers ViewModel = new UpdateVolunteers();

            string url = "volunteersdata/findvolunteers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VolunteersDto SelectedVolunteer = response.Content.ReadAsAsync<VolunteersDto>().Result;
            ViewModel.SelectedVolunteer = SelectedVolunteer;

            //all programs to choose from when creating
            url = "programsdata/listprograms";
            response = client.GetAsync(url).Result;
            IEnumerable<ProgramsDto> ProgramOptions = response.Content.ReadAsAsync<IEnumerable<ProgramsDto>>().Result;

            ViewModel.ProgramOptions = ProgramOptions;

            return View(ViewModel);
        }

        // POST: Volunteers/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Update(int id, Volunteers volunteers)
        {
            GetApplicationCookie();
            string url = "volunteersdata/updatevolunteers/" + id;
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
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Volunteersdata/findVolunteers/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            VolunteersDto SelectedVolunteer = response.Content.ReadAsAsync<VolunteersDto>().Result;

            return View(SelectedVolunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
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
