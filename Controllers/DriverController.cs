using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PassionProject_Danyal.Models;
using PassionProject_Danyal.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace PassionProject_Danyal.Controllers
{
    public class DriverController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DriverController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44333/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        // GET: Driver/List
        public ActionResult List()
        {
            string url = "driverdata/getdrivers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DriverDto> SelectedPlayers = response.Content.ReadAsAsync<IEnumerable<DriverDto>>().Result;
                return View(SelectedPlayers);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Driver/Details/5
        public ActionResult Details(int id)
        {
            ShowDriver ViewModel = new ShowDriver();
            string url = "driverdata/finddriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into driver data transfer object
                DriverDto SelectedDriver = response.Content.ReadAsAsync<DriverDto>().Result;
                ViewModel.driver = SelectedDriver;


                url = "driverdata/findteamfordriver/" + id;
                response = client.GetAsync(url).Result;
                TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
                ViewModel.team = SelectedTeam;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Driver/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Driver/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Driver DriverInfo)
        {
            Debug.WriteLine(DriverInfo.Name);
            string url = "driverdata/adddriver";
            Debug.WriteLine(jss.Serialize(DriverInfo));
            HttpContent content = new StringContent(jss.Serialize(DriverInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int DriverID = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = DriverID });
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Driver/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateDriver ViewModel = new UpdateDriver();

            string url = "driverdata/finddriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into driver data transfer object
                DriverDto SelectedDriver = response.Content.ReadAsAsync<DriverDto>().Result;
                ViewModel.driver = SelectedDriver;

                //get information about teams this driver can drive for.
                url = "teamdata/getteams";
                response = client.GetAsync(url).Result;
                IEnumerable<TeamDto> PotentialTeams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
                ViewModel.allteams = PotentialTeams;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Driver/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Driver driver)
        {
            Debug.WriteLine(driver.Name);
            string url = "driverdata/updatedriver/"+id;
            Debug.WriteLine(jss.Serialize(driver));
            HttpContent content = new StringContent(jss.Serialize(driver));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Driver/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Driverdata/finddriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into driver data transfer object
                DriverDto SelectedDriver = response.Content.ReadAsAsync<DriverDto>().Result;
                return View(SelectedDriver);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Driver/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "driverdata/deletedriver/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }    
    }
}
