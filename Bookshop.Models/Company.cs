using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Bookshop.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
