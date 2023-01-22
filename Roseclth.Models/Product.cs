using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

#nullable disable

namespace Roseclth.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        [Range(1, 10000)]
        [DisplayName("List Price")]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
        [NotMapped]
        [ValidateNever]
        public IFormFile Image { get; set; }

        [Required]
        [DisplayName("Type")]
        public int TypeId { get; set; }
        [ValidateNever]
        public Type Type { get; set; }


        [Required]
        [DisplayName("Material")]
        public int MaterialId { get; set; }

        [ValidateNever]
        public Material Material { get; set; }
    }
}
