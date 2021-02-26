using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Danyal.Models.ViewModels
{
    public class ShowDriver
    {
        public DriverDto driver { get; set; }
        //information about the team the driver belongs to
        public TeamDto team { get; set; }

        //information about the races driver has won
        public IEnumerable<ScheduleDto> raceswon { get; set; }
    }
}