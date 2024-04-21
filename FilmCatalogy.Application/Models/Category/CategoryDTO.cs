using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmCatalogy.Application.Models.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<CategoryDTO>? ChildCategory { get; set; }
    }
}
