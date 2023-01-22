using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

#nullable disable

namespace Roseclth.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> TypeList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> MaterialList { get; set; }
    }
}
