using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Lexiflix.Models;
using Lexiflix.Models.Db;
using Lexiflix.Services;

namespace Lexiflix.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger, IHomeService homeService)
    {
        _logger = logger;
        _homeService = homeService;
    }
    
        public IActionResult Index()
        {
            var model = new HomeVM
            {
                //MostPopularMovies =  _homeService.GetMostPopularMoviesAsync(),
                NewestReleases =  _homeService.GetNewestReleases(),
                ClassicFilms =  _homeService.GetClassicFilms(),
                BestDeals =  _homeService.GetBestDeals(),
                //TopCustomer =  _homeService.GetTopCustomerAsync()
            };

        ViewData["ControllerName"] = "Home";
        ViewData["ActionName"] = "Index";

        return View(model);
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
