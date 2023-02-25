using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using MvcMovieDAL;
using MvcMovieDAL.Entities;
using System.Linq;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Movies/Details/X
        public async Task<IActionResult> Details(int id)
        {
            var viewModelMovie = new MovieViewModel();

            viewModelMovie.Id = id;

            return View(viewModelMovie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var viewModelMovie = new MovieAllGenresViewModel();

            viewModelMovie.Id = id;

            return View(viewModelMovie);
        }
    }
}
