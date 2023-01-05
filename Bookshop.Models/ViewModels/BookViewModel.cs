using Microsoft.AspNetCore.Mvc.Rendering;

#nullable disable

namespace Bookshop.Models.ViewModels
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
