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
	public class FilmService(FilmCatalogyDbContext dbContext) : IFilmService
    {
		public async Task<bool> AddAsync(FilmDTO dTO, CancellationToken cancellation = default)
		{
			await dbContext.Films.AddAsync(Mapper.DTOToFilm(dTO), cancellation);

			var changeColumn = await dbContext.SaveChangesAsync();

			return changeColumn >= 0;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken cancellation = default)
		{
			await dbContext.Films.Where(x => x.Id == id).ExecuteDeleteAsync(cancellation);

            var changeColumn = await dbContext.SaveChangesAsync();

            return changeColumn >= 0;
        }

		public async Task<List<FilmDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string attribute = "", string order = "asc", CancellationToken cancellation = default)
		{
            // Calculate the number of elements to skip based on the page number and page size
            int skipElements = (pageNumber - 1) * pageSize;

            Expression<Func<Film, object>> orderByExpression = null;

            switch (attribute.ToLower())
            {
                case "name":
                    orderByExpression = film => film.Name;
                    break;
                case "director":
                    orderByExpression = film => film.Director;
                    break;
                case "release":
                    orderByExpression = film => film.Release;
                    break;
                default:
                    orderByExpression = film => film.Name;
                    break;
            }

            bool ordering = order == "asc" ? true : false;

            var query = dbContext.Films
				.AsNoTracking()
				.Include(x => x.Categories)
				.ThenInclude(x => x.Category)
				.ThenInclude(c => c.ChildCategories)
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

			List<FilmDTO> dTOs = new List<FilmDTO>();

			foreach(var db in dbResult)
			{
				dTOs.Add(Mapper.FilmToDTO(db));
			}

			return dTOs;
        }

		public async Task<FilmDTO> GetAsync(int id, CancellationToken cancellation = default)
		{
			var dbFilm = await dbContext.Films
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .ThenInclude(c => c.ChildCategories)
                .FirstOrDefaultAsync(x => x.Id == id);

			if(dbFilm == null)
			{
				return default;
			}

			var result = Mapper.FilmToDTO(dbFilm);

			return result;
		}

		public async Task UpdateAsync(int id, FilmDTO dto, CancellationToken cancellation = default)
		{
			var updatedFilm = await dbContext.Films
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .ThenInclude(c => c.ChildCategories)
                .FirstOrDefaultAsync(x => x.Id == id);

            bool hasChanges = false;

            if (!updatedFilm.Name.Equals(dto.Name))
            {
                updatedFilm.Name = dto.Name;
                hasChanges = true;
            }

            if (!updatedFilm.Director.Equals(dto.Director))
            {
                updatedFilm.Director = dto.Director;
                hasChanges = true;
            }

            if (!updatedFilm.Release.Equals(dto.Release))
            {
                updatedFilm.Release = dto.Release;
                hasChanges = true;
            }

            hasChanges |= await UpdateFilmCategoriesAsync(updatedFilm, dto.Categories);

            if (hasChanges)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task<bool> UpdateFilmCategoriesAsync(Film film, List<CategoryDTO> dtoCategories)
        {
            if (film == null || dtoCategories == null)
            {
                return false;
            }

            bool hasChanges = false;

            var categoriesToRemove = film.Categories.Where(fc => !dtoCategories.Any(dto => dto.Id == fc.CategoryId)).ToList();
            var categoriesToAdd = dtoCategories.Where(dto => !film.Categories.Any(fc => fc.CategoryId == dto.Id)).ToList();

            if (categoriesToRemove.Any() || categoriesToAdd.Any())
            {
                hasChanges = true;

                foreach (var categoryToRemove in categoriesToRemove)
                {
                    film.Categories.Remove(categoryToRemove);
                }

                foreach (var categoryToAdd in categoriesToAdd)
                {
                    film.Categories.Add(new FilmCategory { CategoryId = categoryToAdd.Id });
                }
            }

            return hasChanges;
        }
    }
}
