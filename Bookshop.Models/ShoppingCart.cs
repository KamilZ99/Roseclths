using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Bookshop.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }


        public int BookId { get; set; }

        [ValidateNever]
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [Range(1, 999, ErrorMessage = "Please enter a value between 1 and 999")]
        public int Count { get; set; }


        public string ApplicationUserId { get; set; }
        
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
