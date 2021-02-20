using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject_Danyal.Models
{
    //This class describes a driver entity, used for actually generating a database table
    public class Driver
    {
        [Key]
        public int DriverID { get; set; }

        public string Name { get; set; }

        public string PSNTag { get; set; }

        public string Nationality { get; set; }

        public string Abbreviation { get; set; }

        public string Status { get; set; }

        
        //A driver plays for one team
        [ForeignKey("Team")]
        public int TeamID { get; set; }

        public virtual Team Team { get; set; }
        //A team can have many sponsors
        public ICollection<Schedule> Schedule { get; set; }
    }

    //This class can be used to transfer information about a player.
    //also known as a "Data Transfer Object"
    //https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5
    //You can think of this class as the 'Model' that was used in 5101.
    //It is simply a vessel of communication
    public class DriverDto
    {
        public int DriverID { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("PSN Tag")]
        public string PSNTag { get; set; }
        [DisplayName("Nationality")]
        public string Nationality { get; set; }
        [DisplayName("Abbreviation")]
        public string Abbreviation { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }

        public int TeamID { get; set; }

        public string TeamName { get; set; }
    }
}