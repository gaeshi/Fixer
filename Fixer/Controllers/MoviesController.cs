using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Fixer.Data;
using Fixer.Tmdb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fixer.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly MovieDbContext _dbContext;

        private Dictionary<Filter, string> PageTitles = new Dictionary<Filter, string>
        {
            {Filter.Popular, "Popular movies"},
            {Filter.Upcoming, "Upcoming movies"},
            {Filter.TopRated, "Top rated movies"}
        };

        public MoviesController(ILogger<MoviesController> logger, MovieDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        // GET
        public IActionResult Index(Filter filter)
        {
            if (!ModelState.IsValid)
            {
                return View(new List<Movie>());
            }

            ViewData["Title"] = PageTitles[filter];
            var list = _dbContext.Movies.Where(m => m.Filters == filter).ToList();
            return View(list);
        }
    }
}