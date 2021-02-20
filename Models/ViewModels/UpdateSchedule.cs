using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Danyal.Models.ViewModels
{
    public class UpdateSchedule
    {
        //Information about the races
        public ScheduleDto schedule { get; set; }
        //Needed for a dropdownlist to chose winner for each race
        public IEnumerable<DriverDto> alldrivers { get; set; }
    }
}