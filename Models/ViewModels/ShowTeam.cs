using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Danyal.Models.ViewModels
{
    public class ShowTeam
    {
        //Information about the team
        public TeamDto team { get; set; }

        //Information about all drivers on that team
        public IEnumerable<DriverDto> teamdrivers { get; set; }


    }
}