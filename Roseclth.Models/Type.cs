using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Roseclth.Models
{
    public class Type
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
