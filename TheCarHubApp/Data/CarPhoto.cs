﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Column(TypeName ="nvarchar(50)")]
        [DisplayName("Photo Title")]
        public string PhotoTitle { get; set; }
        [Column(TypeName ="nvarchar(100)")]
        [DisplayName("Photo Name")]
        public string PhotoFilePath { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile PhotoFile { get; set; }
    }
}
