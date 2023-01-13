using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Bookshop.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }

        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        [ValidateNever]
        public Book Book { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }
    }
}
