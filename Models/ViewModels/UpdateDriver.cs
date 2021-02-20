using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Danyal.Models.ViewModels
{
    //This View Model will update driver
    public class UpdateDriver
    {
        //Information about the driver
        public DriverDto driver { get; set; }

        //Needed for a dropdownlist which presents the driver with a choice of teams to drive for
        public IEnumerable<TeamDto> allteams { get; set; }
    }
}