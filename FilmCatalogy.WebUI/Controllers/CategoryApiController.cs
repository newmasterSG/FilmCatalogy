using FilmCatalogy.Application.Interfaces;
using FilmCatalogy.Application.Models.Category;
using FilmCatalogy.Application.Models.Films;
using FilmCatalogy.Application.Services;
using FilmCatalogy.Entities;
using FilmCatalogy.Infrastructure.DbContexts;
using FilmCatalogy.Infrastructure.DbContexts.Configs;
using Microsoft.AspNetCore.Mvc;

namespace FilmCatalogy.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryApiController(IFilmService filmService, ICategoryService categoryService) : ControllerBase
    {

        [HttpGet("{filmId}")]
        public async Task<IActionResult> GetCategoriesForFilm(int filmId)
        {
            var film = await filmService.GetAsync(filmId);
            if (film.Categories == null)
                return NotFound();

            return Ok(film.Categories);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await categoryService.GetAllAsync());
        } 

        [HttpPost("{filmId}")]
        public async Task<IActionResult> UpdateCategoriesForFilm(int filmId, List<int> categoryIds)
        {
            var film = await filmService.GetAsync(filmId);
            if (film == null)
                return NotFound();

            List<CategoryDTO> categories = new List<CategoryDTO>();
            foreach (var categoryId in categoryIds)
            {
                var category = await categoryService.GetAsync(categoryId);
                if (category != null)
                {
                    categories.Add(new CategoryDTO
                    {
                        Id = category.Id,
                        Name = category.Name,
                        ChildCategory = category.ChildCategory?.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name }).ToList()
                    });
                }
            }

            film.Categories = categories;

            await filmService.UpdateAsync(film.Id, film);

            return Ok();
        }
    }
}
