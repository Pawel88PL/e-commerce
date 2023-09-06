﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiodOdStaniula.Models
{
    public class ProductImageAdd
    {
        [Key]
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string? ImagePath { get; set; }
    }
}
