using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcMovieDAL.Entities;
using MvcMovieDAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class GenreController : Controller
    {
        private readonly IRepository<Genre> _genreRepository;

        public GenreController(IRepository<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        // GET: GenreController
        public async Task<IActionResult> Index(string searchString)
        {
            // Use LINQ to get list of genres.
            var genreQuery = (from m in _genreRepository.Get()
                                             orderby m.Name
                                             select m).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                genreQuery = genreQuery.Where(s => s.Name!.StartsWith(searchString));
            }

            var genresView = await genreQuery.Select(x => new GenreViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync())
            };

            return View(movieGenreVM);
        }

        // GET: GenreController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // GET: GenreController/Delete/5
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (id < 0 || !_genreRepository.Exists(id))
        //    {
        //        return NotFound();
        //    }

        //    var movie = _genreRepository.Get().Where(x => x.Id == id).Select(x => new
        //    {
        //        x.Id,
        //        x.Name
        //    }).SingleOrDefault();

        //    GenreViewModel genreView = new GenreViewModel();
        //    genreView.Name = movie.Name;

        //    return View(genreView);
        //}

    }
}
