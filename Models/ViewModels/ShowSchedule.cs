using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Danyal.Models.ViewModels
{
    public class ShowSchedule
    {
        public ScheduleDto schedule { get; set; }
        //information about the races in schedule
        public DriverDto driver { get; set; }
    }
}