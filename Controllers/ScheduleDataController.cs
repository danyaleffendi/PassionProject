using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProject_Danyal.Models;
using System.Diagnostics;

namespace PassionProject_Danyal.Controllers
{
    public class ScheduleDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list or races in the schedule in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of races along with all the info.</returns>
        /// <example>
        /// GET: api/ScheduleData/GetSchedule
        /// </example>
        [ResponseType(typeof(IEnumerable<ScheduleDto>))]
        public IHttpActionResult GetSchedule()
        {
            List<Schedule> Schedules = db.Schedule.ToList();
            List<ScheduleDto> ScheduleDtos = new List<ScheduleDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Schedule in Schedules)
            {
                ScheduleDto NewSchedule = new ScheduleDto
                {
                    RaceID = Schedule.RaceID,
                    Round = Schedule.Round,
                    Circuit = Schedule.Circuit,
                    Date = Schedule.Date,
                    DriverID = Schedule.DriverID


                };
                ScheduleDtos.Add(NewSchedule);
            }

            return Ok(ScheduleDtos);
        }

        /// <summary>
        /// Finds a particular schedule race in the database with a 200 status code. If the race is not found, return 404.
        /// </summary>
        /// <param name="id">The race id</param>
        /// <returns>Information about the race</returns>
        // <example>
        // GET: api/ScheduleData/FindSchedule/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(ScheduleDto))]
        public IHttpActionResult FindSchedule(int id)
        {
            //Find the data
            Schedule Schedule = db.Schedule.Find(id);
            //if not found, return 404 status code.
            if (Schedule == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            ScheduleDto ScheduleDto = new ScheduleDto
            {
                RaceID = Schedule.RaceID,
                Round = Schedule.Round,
                Circuit = Schedule.Circuit,
                Date = Schedule.Date,
                DriverID = Schedule.DriverID
            };

            //pass along data as 200 status code OK response
            return Ok(ScheduleDto);
        }

        /// <summary>
        /// Finds a race winner driver in the database given a driver id with a 200 status code. If the driver is not found, return 404.
        /// </summary>
        /// <param name="id">The race id</param>
        /// <returns>Information about the driver</returns>
        // <example>
        // GET: api/DriverData/FindDriverForSchedule/5
        // </example> 
        /*
        [HttpGet]
        [ResponseType(typeof(PlayerDto))]
        public IHttpActionResult FindDriverForSchedule(int id)
        {
            //Finds the winner of the race
            //that match the input raceid
            Schedule Schedule = db.Schedules
                .Where(t=> t.Schedules.Any(p=> p.RaceID==id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Team == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            DriverDto NewDriver = new DriverDto
                {
                    DriverID = Driver.DriverID,
                    Name = Driver.Name,
                    PSNTag = Driver.PSNTag,
                    Nationality = Driver.Nationality,
                    Abbreviation = Driver.Abbreviation,
                    Status = Driver.Status,
                    TeamID = Driver.TeamID
                };


            //pass along data as 200 status code OK response
            return Ok(DriverDto);
        }

        */

        /// <summary>
        /// Updates a race in the database given information about the race.
        /// </summary>
        /// <param name="id">The race id</param>
        /// <param name="player">A race object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/ScheduleData/UpdateSchedule/5
        /// FORM DATA: Schedule JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSchedule(int id, [FromBody] Schedule Schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Schedule.RaceID)
            {
                return BadRequest();
            }

            db.Entry(Schedule).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Adds a race to the database.
        /// </summary>
        /// <param name="player">A race object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/ScheduleData/AddSchedule
        ///  FORM DATA: Schedule JSON Object
        /// </example>
        [ResponseType(typeof(ScheduleDto))]
        [HttpPost]
        public IHttpActionResult AddSchedule([FromBody] Schedule Schedule)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Schedule.Add(Schedule);
            db.SaveChanges();

            return Ok(Schedule.RaceID);
        }

        /// <summary>
        /// Deletes a race in the database
        /// </summary>
        /// <param name="id">The id of the race to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/ScheduleData/DeleteSchedule/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteSchedule(int id)
        {
            Schedule Schedule = db.Schedule.Find(id);
            if (Schedule == null)
            {
                return NotFound();
            }

            db.Schedule.Remove(Schedule);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Finds a race in the system. Internal use only.
        /// </summary>
        /// <param name="id">The race id</param>
        /// <returns>TRUE if the race exists, false otherwise.</returns>
        private bool ScheduleExists(int id)
        {
            return db.Schedule.Count(e => e.RaceID == id) > 0;
        }
    }

}
