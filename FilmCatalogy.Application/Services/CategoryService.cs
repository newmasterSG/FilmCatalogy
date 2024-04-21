using FilmCatalogy.Application.Interfaces;
using FilmCatalogy.Application.Mapping;
using FilmCatalogy.Application.Models.Category;
using FilmCatalogy.Application.Models.Films;
using FilmCatalogy.Entities;
using FilmCatalogy.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FilmCatalogy.Application.Services
{
    public class CategoryService(FilmCatalogyDbContext dbContext) : ICategoryService
    {
        public async Task<bool> AddAsync(CategoryDTO dTO, CancellationToken cancellation = default)
        {
            await dbContext.Categories.AddAsync(Mapper.DTOToCategory(dTO), cancellation);

            var changeColumn = await dbContext.SaveChangesAsync();

            return changeColumn >= 0;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellation = default)
        {
            await dbContext.Categories.Where(x => x.Id == id).ExecuteDeleteAsync(cancellation);

            var changeColumn = await dbContext.SaveChangesAsync();

            return changeColumn >= 0;
        }

        public async Task<List<CategoryDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string attribute = "", string order = "asc", CancellationToken cancellation = default)
        {
            // Calculate the number of elements to skip based on the page number and page size
            int skipElements = (pageNumber - 1) * pageSize;

            Expression<Func<Category, object>> orderByExpression = null;

            switch (attribute.ToLower())
            {
                case "name":
                    orderByExpression = category => category.Name;
                    break;
                default:
                    orderByExpression = category => category.Name;
                    break;
            }

            bool ordering = order == "asc" ? true : false;

            var query = dbContext.Categories
                .AsNoTracking()
                .Include(x => x.ChildCategories)
                .Include(x => x.ParentCategory)
                .AsQueryable();

            if (ordering)
            {
                query = query.OrderBy(orderByExpression);
            }
            else
            {
                query = query.OrderByDescending(orderByExpression);
            }

            var dbResult = await query
                .Skip(skipElements)
                .Take(pageSize)
                .ToListAsync(cancellation);

            List<CategoryDTO> dTOs = new List<CategoryDTO>();

            foreach (var db in dbResult)
            {
                dTOs.Add(Mapper.CategoryToDTO(db));
            }

            return dTOs;
        }

        public async Task<CategoryDTO> GetAsync(int id, CancellationToken cancellation = default)
        {
            var dbFilm = await dbContext.Categories
                .Include(x => x.ChildCategories)
                .Include(x => x.ParentCategory)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dbFilm == null)
            {
                return default;
            }

            var result = Mapper.CategoryToDTO(dbFilm);

            return result;
        }

        public async Task UpdateAsync(int id, CategoryDTO dto, CancellationToken cancellation = default)
        {
            var updatedCategory = await dbContext.Categories
                .Include(c => c.ChildCategories)
                .Include(x => x.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == id);

            bool hasChanges = false;

            if (dto.Name != null && !updatedCategory.Name.Equals(dto.Name))
            {
                updatedCategory.Name = dto.Name;
                hasChanges = true;
            }

            hasChanges |= await UpdateChildCategoriesAsync(updatedCategory.ChildCategories, dto.ChildCategory);

            if (hasChanges)
            {
                await dbContext.SaveChangesAsync();
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<CategoryListDTO>> GetCategoriesWithNestingLevel()
        {
            var categories = await dbContext.Categories.AsNoTracking().ToListAsync();
            var categoryViewModels = new List<CategoryListDTO>();

            foreach (var category in categories)
            {
                var nestingLevel = CalculateNestingLevel(category);
                var categoryViewModel = new CategoryListDTO
                {
                    Name = category.Name,
                    NumberOfFilms = GetNumberOfFilmsInCategory(category),
                    NestingLevel = nestingLevel
                };
                categoryViewModels.Add(categoryViewModel);
            }

            return categoryViewModels;
        }


        private int CalculateNestingLevel(Category category)
        {
            int nestingLevel = 0;
            var parentCategory = category.ParentCategory;
            while (parentCategory != null)
            {
                nestingLevel++;
                parentCategory = parentCategory.ParentCategory;
            }
            return nestingLevel;
        }

        private int GetNumberOfFilmsInCategory(Category category)
        {
            var filmCount = dbContext.FilmCategories.Count(fc => fc.CategoryId == category.Id);
            return filmCount;
        }

        private async Task<bool> UpdateChildCategoriesAsync(List<Category> childCategories, List<CategoryDTO> dtoChildCategories)
        {
            if (childCategories == null || dtoChildCategories == null)
            {
                return false;
            }

            bool hasChanges = false;

            foreach (var childCategory in childCategories)
            {
                var dtoChildCategory = dtoChildCategories.FirstOrDefault(dto => dto.Id == childCategory.Id);
                if (dtoChildCategory != null)
                {
                    if (dtoChildCategory.Name != null && !childCategory.Name.Equals(dtoChildCategory.Name))
                    {
                        childCategory.Name = dtoChildCategory.Name;
                        hasChanges = true;
                    }

                    hasChanges |= await UpdateChildCategoriesAsync(childCategory.ChildCategories, dtoChildCategory.ChildCategory);
                }
            }

            return hasChanges;
        }
    }
}
