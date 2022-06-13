using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheCarHubApp.Data
{
    public class CarModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FK_CarMake")]
        public int? CarMakeId { get; set; }
        public string ModelName { get; set; }
    }
}
