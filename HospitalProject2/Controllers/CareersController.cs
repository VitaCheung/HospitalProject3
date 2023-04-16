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
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
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

        // GET: Careers/List
        public ActionResult List()
        {
            //objective: communicate with our Careers data api to retrieve a list of Careers
            //curl https://localhost:44398/api/careersdata/listcareers

            CareerList ViewModel = new CareerList();
            if (User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;

            string url = "careersdata/listcareers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CareersDto> Careers = response.Content.ReadAsAsync<IEnumerable<CareersDto>>().Result;

           
            ViewModel.Careers = Careers;

            return View(ViewModel);

            //return View(Careers);
        }

        // GET: Careers/Details/5
        public ActionResult Details(int id)
        {
            DetailsCareers ViewModel = new DetailsCareers();
            if (User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;

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
            GetApplicationCookie();
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
            GetApplicationCookie();
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
            GetApplicationCookie();
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
