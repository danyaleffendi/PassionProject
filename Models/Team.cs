using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject_Danyal.Models
{
    public class Team
    {
        public int TeamID { get; set; }

        public string TeamName { get; set; }

        public string TeamColor { get; set; }

        //A team can have many players
        public ICollection<Driver> Drivers { get; set; }


    }

    public class TeamDto
    {
        public int TeamID { get; set; }

        [DisplayName("Team Name")]
        public string TeamName { get; set; }
        [DisplayName("Team Color")]
        public string TeamColor { get; set; }
    }
}