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
    public class FAQsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static FAQsController()
        {

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");

        }
        // GET: FAQ/List
        public ActionResult List()
        {
            string url = "FAQsData/ListFAQs";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<FAQsDto> FAQs = response.Content.ReadAsAsync<IEnumerable<FAQsDto>>().Result;
            return View(FAQs);
        }

        // GET: FAQ/Details/5
        public ActionResult Details(int id)
        {
            string url = "FAQData/FindFAQ/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FAQsDto fAQ = response.Content.ReadAsAsync<FAQsDto>().Result;
            return View(fAQ);
        }

        // GET: FAQ/New

        public ActionResult New()
        {
            string url = "DepartmentsData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;
            ViewData["Departments"] = departments;
            return View();
        }

        // POST: FAQ/Create
        [HttpPost]

        public ActionResult Create(FAQs FAQ)
        {

            string url = "FAQData/CreateFAQ";
            string jsonpayload = jss.Serialize(FAQ);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("list", "FAQ");
            }
            else
            {
                return View("Error");
            }
        }


        // GET: FAQ/Edit/5

        public ActionResult Edit(int id)
        {
            UpdateFAQ ViewModel = new UpdateFAQ();

            string url = "FAQsData/FindFAQ/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FAQsDto SelectedFAQ = response.Content.ReadAsAsync<FAQsDto>().Result;
            ViewModel.SelectedFAQ = SelectedFAQ;

            url = "DepartmentsData/ListDepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        // POST: FAQ/Update/5
        [HttpPost]

        public ActionResult Update(int id, FAQs faq)
        {

            string url = "FAQData/EditFAQ/" + id;
            string jsonpayload = jss.Serialize(faq);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("list", "FAQ");
            }
            else
            {
                return View("Error");
            }
        }


        // GET: FAQ/Delete/5

        public ActionResult ConfirmDelete(int id)
        {
            string url = "FAQsData/FindFAQ/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FAQsDto FAQ = response.Content.ReadAsAsync<FAQsDto>().Result;
            return View(FAQ);
        }

        // POST: FAQ/Delete/5
        [HttpPost]

        public ActionResult Delete(int id)
        {

            string url = "FAQsData/DeleteFAQ/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View("Error");
            }

        }
    }
}