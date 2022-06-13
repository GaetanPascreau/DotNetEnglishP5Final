using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheCarHubApp.Data
{
    public class CarDetail
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("FK_Car")]
        public int CarId { get; set; }
        public List<CarPhoto> CarPhotos { get; set; }
    }
}
