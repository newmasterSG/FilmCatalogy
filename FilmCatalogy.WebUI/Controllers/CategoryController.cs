using FilmCatalogy.Application.Interfaces;
using FilmCatalogy.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmCatalogy.WebUI.Controllers
{
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await categoryService.GetAllAsync());
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await categoryService.GetAsync(id);
            var categories = await categoryService.GetAllAsync();
            ViewBag.ParentCategories = categories;
            ViewBag.ChildCategories = categories;
            return View(category);
        }

        public async Task<IActionResult> List()
        {
            return View(await categoryService.GetCategoriesWithNestingLevel());
        }
    }
}
