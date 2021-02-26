using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject_Danyal.Models
{
    public class Schedule
    {
       
        [Key]
        public int RaceID { get; set; }

        public int Round { get; set; }

        public string Circuit { get; set; }

        public string Date { get; set; }

        //A Race has 1 winner
        [ForeignKey("Driver")]
        public int DriverID { get; set; }

        public virtual Driver Driver { get; set; }
        public ICollection<Driver> Drivers { get; set; }
    }


    public class ScheduleDto
    {
        public int RaceID { get; set; }
        [DisplayName("Round")]
        public int Round { get; set; }
        [DisplayName("Circuit")]
        public string Circuit { get; set; }
        [DisplayName("Date")]
        public string Date { get; set; }
        [DisplayName("DriverID")]
        public int DriverID { get; set; }
        [DisplayName("Winner")]
        public string Abbreviation { get; set; }
    }
}
