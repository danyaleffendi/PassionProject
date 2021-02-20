using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_Danyal.Models.ViewModels
{
    public class ShowDriver
    {
        public DriverDto driver { get; set; }
        //information about the team the player plays for
        public TeamDto team { get; set; }
    }
}