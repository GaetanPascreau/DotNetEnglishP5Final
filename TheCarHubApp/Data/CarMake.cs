using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheCarHubApp.Data
{
    public class CarMake
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Make")]
        public string MakeName { get; set; }
        public List<Car> Cars { get; set; }
        public List<CarModel> CarModels { get; set; }
    }
}
