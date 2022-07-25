using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TheCarHubApp.Data
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Make is required")]
        [ForeignKey("FK_CarMake")]
        public int? CarMakeId { get; set; }

        [Required(ErrorMessage = "The Model is required")]
        [ForeignKey("FK_CarModel")]
        public int? CarModelId { get; set; }

        [Required(ErrorMessage = "The VIN is required")]
        [StringLength(17)]
        [RegularExpression(@"[A-HJ-NPR-Z0-9]{17}")]
        public string VIN { get; set; }

        [Required(ErrorMessage = "The Year is required")]
        [Range(1990, 2022)]
        public int Year { get; set; }

        [Required(ErrorMessage = "The Trim is required")]
        public string Trim { get; set; }

        [Required(ErrorMessage = "The Purchase Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "The Purchase Price is required")]
        [Display(Name = "Purchase Price")]
        public double PurchasePrice { get; set; }

        public string Repairs { get; set; }

        [Required(ErrorMessage = "The Repair cost is required. In the absence of repair, enter 0")]
        [Display(Name = "Repair Costs")]
        public double RepairCost { get; set; }

        [Required(ErrorMessage = "The Lot Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Lot Date")]
        public DateTime LotDate { get; set; }

        [Required(ErrorMessage = "The Selling Price is required")]
        [Display(Name = "Selling Price")]
        public double SellingPrice { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Sale Date")]
        public DateTime? SaleDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Milleage is required")]
        public int Milleage { get; set; }

        [Required(ErrorMessage = "The Color is required")]
        public string Color { get; set; }

        [Display(Name = "Make")]
        public string MakeName { get; set; }

        [Display(Name = "Model")]
        public string ModelName
        {
            get; set;
        }
        [NotMapped]
        [Display(Name = "Add Car Photoes")]
        public List<IFormFile> CarPhotoes { get; set; }
    }
}
