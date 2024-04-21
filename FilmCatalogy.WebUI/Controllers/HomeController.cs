using FilmCatalogy.Application.Interfaces;
using FilmCatalogy.Application.Services;
using FilmCatalogy.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FilmCatalogy.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFilmService _filmService;
        public HomeController(ILogger<HomeController> logger, IFilmService filmService)
        {
            _logger = logger;
            _filmService = filmService;
        }

        public async Task<IActionResult> Index()
        {
            var films = await _filmService.GetAllAsync();
            return View(films);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
