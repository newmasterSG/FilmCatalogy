using FilmCatalogy.Application.Models.Category;

namespace FilmCatalogy.Application.Interfaces
{
    public interface ICategoryService : IService<CategoryDTO>
    {
        Task<List<CategoryListDTO>> GetCategoriesWithNestingLevel();
    }
}
