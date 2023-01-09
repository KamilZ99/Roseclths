using System.ComponentModel.DataAnnotations;

namespace Bookshop.Models
{
    public class ShoppingCart
    {
        public Book Book { get; set; }
        [Range(1, 999, ErrorMessage = "Please enter a value between 1 and 999")]
        public int Count { get; set; }
    }
}
