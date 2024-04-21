using FilmCatalogy.Application.Interfaces;
using FilmCatalogy.Application.Models.Category;
using FilmCatalogy.Application.Models.Films;
using FilmCatalogy.Application.Services;
using FilmCatalogy.Infrastructure.DbContexts.Configs;
using Microsoft.AspNetCore.Mvc;

namespace FilmCatalogy.WebUI.Controllers
{
    public class FilmController(IFilmService filmService, ICategoryService categoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var films = await filmService.GetAllAsync();
            return View(films);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Categories = await categoryService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(FilmDTO filmDTO, List<int> CategoryIds)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<CategoryDTO> categories = new List<CategoryDTO>();
            foreach (var categoryId in CategoryIds)
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

            filmDTO.Categories = categories;

            await filmService.AddAsync(filmDTO);

            return Redirect("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id, CancellationToken cancellation)
        {
            ViewBag.Categories = await categoryService.GetAllAsync();
            return View(await filmService.GetAsync(id, cancellation));
        }

        [HttpPost]
        public async Task<IActionResult> Update(FilmDTO filmDTO, List<int> CategoryIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<CategoryDTO> categories = new List<CategoryDTO>();
            foreach (var categoryId in CategoryIds)
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

            filmDTO.Categories = categories;

            await filmService.UpdateAsync(filmDTO.Id, filmDTO);

            return Redirect("Index");
        }
    }
}
