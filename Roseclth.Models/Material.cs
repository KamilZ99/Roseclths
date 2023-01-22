using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Roseclth.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Material")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
