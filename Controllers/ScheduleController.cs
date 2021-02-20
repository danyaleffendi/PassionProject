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
    public class ScheduleController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static ScheduleController()
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

        // GET: Schedule/List
        public ActionResult List()
        {
            string url = "scheduledata/getschedule";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ScheduleDto> SelectedRace = response.Content.ReadAsAsync<IEnumerable<ScheduleDto>>().Result;
                return View(SelectedRace);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Schedule/Details/5

        public ActionResult Details(int id)
        {
            ShowSchedule ViewModel = new ShowSchedule();
            string url = "scheduledata/findschedule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                ScheduleDto SelectedSchedule = response.Content.ReadAsAsync<ScheduleDto>().Result;
                ViewModel.schedule = SelectedSchedule;

                /*
                url = "playerdata/findteamforplayer/" + id;
                response = client.GetAsync(url).Result;
                TeamDto SelectedTeam = response.Content.ReadAsAsync<TeamDto>().Result;
                ViewModel.team = SelectedTeam;
                */
                return View(ViewModel);

            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Schedule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Schedule ScheduleInfo)
        {
            Debug.WriteLine(ScheduleInfo.Circuit);
            string url = "scheduledata/addschedule";
            Debug.WriteLine(jss.Serialize(ScheduleInfo));
            HttpContent content = new StringContent(jss.Serialize(ScheduleInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int RaceID = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = RaceID });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Schedule/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateSchedule ViewModel = new UpdateSchedule();

            string url = "scheduledata/findschedule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                ScheduleDto SelectedSchedule = response.Content.ReadAsAsync<ScheduleDto>().Result;
                ViewModel.schedule = SelectedSchedule;

                /* get information about teams this player COULD play for.
                url = "teamdata/getteams";
                response = client.GetAsync(url).Result;
                IEnumerable<PlayerDto> PotentialPlayers = response.Content.ReadAsAsync<IEnumerable<PlayerDto>>().Result;
                ViewModel.allplayers = PotentialPlayers;
                */
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Schedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Schedule ScheduleInfo)
        {
            Debug.WriteLine(ScheduleInfo.Round);
            string url = "scheduledata/updateschedule/" + id;
            Debug.WriteLine(jss.Serialize(ScheduleInfo));
            HttpContent content = new StringContent(jss.Serialize(ScheduleInfo));
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

        // GET: Schedule/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "scheduledata/findschedule/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                ScheduleDto SelectedSchedule = response.Content.ReadAsAsync<ScheduleDto>().Result;
                return View(SelectedSchedule);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Schedule/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "scheduledata/deleteschedule/" + id;
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
