using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AppShareServices.Models;

namespace ProductApplication.DTOs
{
    public class CategoryDto : DtoBase
    {
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}

