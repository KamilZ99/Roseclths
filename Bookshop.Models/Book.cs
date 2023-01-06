using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

#nullable disable

namespace Bookshop.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1, 10000)]
        [DisplayName("List Price")]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 10000)]
        public double Price { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }


        [Required]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }


        [Required]
        public int CoverTypeId { get; set; }

        [ValidateNever]
        [DisplayName("Cover Type")]
        public CoverType CoverType { get; set; }
    }
}
