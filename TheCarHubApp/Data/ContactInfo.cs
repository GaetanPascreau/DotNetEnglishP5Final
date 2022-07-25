using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheCarHubApp.Data
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage = "Address Line 2 is required")]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        [StringLength(5)]
        public string Zip { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Week-day opening hour is required")]
        [DataType(DataType.Time)]
        [Display(Name = "Week-day Opening Hour")]
        public DateTime WeekDayOpeningHour { get; set; }

        [Required(ErrorMessage = "Week-day closing hour is required")]
        [DataType(DataType.Time)]
        [Display(Name = "Week-day Closing Hour")]
        public DateTime WeekDayClosingHour { get; set; }

        [Required(ErrorMessage = "Saturday opening hour is required")]
        [DataType(DataType.Time)]
        [Display(Name = "Saturday Opening Hour")]
        public DateTime SaturdarOpeningHour { get; set; }

        [Required(ErrorMessage = "Saturday closing hour is required")]
        [DataType(DataType.Time)]
        [Display(Name = "Saturday Closing Hour")]
        public DateTime SaturdayClosingHour { get; set; }

        [Required(ErrorMessage = "Map Latitude is required")]
        [Display(Name = "Map Latitude")]
        public string MapLat { get; set; }

        [Required(ErrorMessage = "Map Longitude is required")]
        [Display(Name = "Map Longitude")]
        public string MapLong { get; set; }

        [Required(ErrorMessage = "Map Zoom is required")]
        [Display(Name = "Map Zoom")]
        public int MapZoom { get; set; }

        [Required(ErrorMessage = "Map Title is required")]
        [Display(Name = "Map Title")]
        public string MapTitle { get; set; }
    }
}