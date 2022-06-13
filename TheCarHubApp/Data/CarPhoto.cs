using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheCarHubApp.Data
{
    public class CarPhoto
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FK_CarDetail")]
        public int CarDetailId { get; set; }
        public string PhotoTitle { get; set; }
        public string PhotoFilePath { get; set; }
    }
}
