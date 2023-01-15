using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Bookshop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
