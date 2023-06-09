﻿using System;
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
    public class DonationsController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonationsController()
        {

            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44398/api/");


        }


        // GET: Donations/List
        public ActionResult List()
        {
            string url = "DonationsData/ListDonations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DonationsDto> donations = response.Content.ReadAsAsync<IEnumerable<DonationsDto>>().Result;
            return View(donations);
        }

        // GET: Donations/Details/5
        public ActionResult Details(int id)
        {
            string url = "DonationsData/FindDonations/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonationsDto donations = response.Content.ReadAsAsync<DonationsDto>().Result;
            return View(donations);
        }

        // GET: Donations/New
        public ActionResult New()
        {
            string url = "departmentsdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            return View(DepartmentOptions);
        }

        // POST: Donations/Create
        [HttpPost]
        public ActionResult Create(Donations donations)
        {
            string url = "DonationsData/AddDonations";
            string jsonpayload = jss.Serialize(donations);

            HttpContent content = new StringContent(jsonpayload);
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

        // GET: Donations/Edit/5

        public ActionResult Edit(int id)
        {
            UpdateDonation ViewModel = new UpdateDonation();
            string url = "DonationsData/FindDonationsr/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonationsDto donations = response.Content.ReadAsAsync<DonationsDto>().Result;

            url = "departmentsdata/listdepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;

            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        // POST: Donations/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Donations donation)
        {
            
                string url = "DonorData/UpdateDonor/" + id;
                string jsonpayload = jss.Serialize(donation);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("list", "Donation");
                }
                else
                {
                    return View("Error");
                }
        }

        // GET: Donations/Delete/5

        public ActionResult ConfirmDelete(int id)
        {
            string url = "DonationsData/FindDonations/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonationsDto donation = response.Content.ReadAsAsync<DonationsDto>().Result;
            return View(donation);
        }

        // POST: Donor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DonationsData/DeleteDonations/" + id;
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

        public ActionResult Thankyou()
        {
            return View();
        }
    }

}
