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
    public class DriverDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Gets a list or drivers in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of drivers including all their available info.</returns>
        /// <example>
        // GET: api/DriverData/GetDrivers
        /// </example>
        [ResponseType(typeof(IEnumerable<DriverDto>))]
        public IHttpActionResult GetDrivers()
        {
            List<Driver> Drivers = db.Drivers.ToList();
            List<DriverDto> DriverDtos = new List<DriverDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Driver in Drivers)
            {
                DriverDto NewDriver = new DriverDto
                {
                    DriverID = Driver.DriverID,
                    Name = Driver.Name,
                    PSNTag = Driver.PSNTag,
                    Nationality = Driver.Nationality,
                    Abbreviation = Driver.Abbreviation,
                    Status = Driver.Status,
                    TeamID = Driver.TeamID,
                    TeamName = Driver.Team.TeamName
                };
                DriverDtos.Add(NewDriver);
            }

            return Ok(DriverDtos);
        }

        /// <summary>
        /// Finds a particular driver in the database with a 200 status code. If the driver is not found, return 404.
        /// </summary>
        /// <param name="id">The driver id</param>
        /// <returns>Information about the driver</returns>
        // <example>
        // GET: api/DriverData/FindDriver/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(DriverDto))]
        public IHttpActionResult FindDriver(int id)
        {
            //Find the data
            Driver Driver = db.Drivers.Find(id);
            //if not found, return 404 status code.
            if (Driver == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            DriverDto DriverDto = new DriverDto
            {
                DriverID = Driver.DriverID,
                Name = Driver.Name,
                PSNTag = Driver.PSNTag,
                Nationality = Driver.Nationality,
                Abbreviation = Driver.Abbreviation,
                Status = Driver.Status,
                TeamID = Driver.TeamID,
                TeamName = Driver.Team.TeamName
            };

            //pass along data as 200 status code OK response
            return Ok(DriverDto);
        }
        /// <summary>
        /// Finds a particular Team in the database given a driver id with a 200 status code. If the Team is not found, return 404.
        /// </summary>
        /// <param name="id">The driver id</param>
        /// <returns>Information about the Team, including Team id, team name, and team color</returns>
        // <example>
        // GET: api/TeamData/FindTeamForDriver/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(TeamDto))]
        public IHttpActionResult FindTeamForDriver(int id)
        {
            //Finds the first team which has any drivers
            //that match the input driverid
            Team Team = db.Teams
                .Where(t => t.Drivers.Any(p => p.DriverID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Team == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            TeamDto TeamDto = new TeamDto
            {
                TeamID = Team.TeamID,
                TeamName = Team.TeamName,
                TeamColor = Team.TeamColor
            };


            //pass along data as 200 status code OK response
            return Ok(TeamDto);
        }
        
        /// <summary>
        /// Gets a list of races won by the driver in the database alongside a status code (200 OK).
        /// </summary>
        /// <param name="id">The driver id</param>
        /// <returns>A list of races won by the driver </returns>
        // <example>
        // GET: api/DriverData/GetRaceWonbyDriver/5
        // </example>
        [ResponseType(typeof(IEnumerable<ScheduleDto>))]
        public IHttpActionResult GetRaceWonbyDriver(int id)
        {
            List<Schedule> Schedules = db.Schedule.Where(p => p.DriverID == id)
                .ToList();
            List<ScheduleDto> ScheduleDtos = new List<ScheduleDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Schedule in Schedules)
            {
                //put into a 'friendly object format'
                ScheduleDto Wonraces = new ScheduleDto
                {
                    RaceID = Schedule.RaceID,
                    Round = Schedule.Round,
                    Circuit = Schedule.Circuit,
                    Date = Schedule.Date,
                };

                ScheduleDtos.Add(Wonraces);
            }
            //pass along data as 200 status code OK response
            return Ok(ScheduleDtos);
        }
         
        /// <summary>
        /// Updates a driver in the database given information about the driver.
        /// </summary>
        /// <param name="id">The driver id</param>
        /// <param name="driver">A driver object. Received as POST data.</param>
        /// <example>
        /// POST: api/DriverData/UpdateDriver/5
        /// FORM DATA: Driver JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDriver(int id, [FromBody] Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driver.DriverID)
            {

                return BadRequest();
            }

            db.Entry(driver).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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
        /// Adds a driver to the database.
        /// </summary>
        /// <param name="driver">A driver object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/DriverData/AddDriver
        ///  FORM DATA: Driver JSON Object
        /// </example>
        [ResponseType(typeof(Driver))]
        [HttpPost]
        public IHttpActionResult AddDriver([FromBody] Driver driver)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            db.SaveChanges();
            Debug.WriteLine(driver.Name);
            return Ok(driver.DriverID);
        }

        /// <summary>
        /// Deletes a driver in the database
        /// </summary>
        /// <param name="id">The id of the driver to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/DriverData/DeleteDriver/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
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
        /// Finds a driver in the system. Internal use only.
        /// </summary>
        /// <param name="id">The driver id</param>
        /// <returns>TRUE if the driver exists, false otherwise.</returns>
        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.DriverID == id) > 0;
        }
    }

}
