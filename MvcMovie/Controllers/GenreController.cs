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

        // POST: GenreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] GenreViewModel genre)
        {
            if (ModelState.IsValid)
            {
                if (_genreRepository.Get().Where(x => x.Name == genre.Name).SingleOrDefault() == null)
                {
                    // dont exist, so create the new
                    Genre genreEntity = new Genre();
                    genreEntity.Name = genre.Name;
                    genreEntity.CreatedDate = DateTime.Now;
                    genreEntity.UpdatedDate = DateTime.Now;
                    _genreRepository.Insert(genreEntity);
                    _genreRepository.Save();
                }
                else
                {
                    // alert user
                    return Problem("Value inserted already exists.");
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: GenreController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0 || !_genreRepository.Exists(id))
            {
                return NotFound();
            }

            var movie = _genreRepository.Get().Where(x => x.Id == id).Select(x => new
            {
                x.Id,
                x.Name
            }).SingleOrDefault();

            GenreViewModel genreView = new GenreViewModel();
            genreView.Name = movie.Name;

            return View(genreView);
        }

        // POST: GenreController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (_genreRepository.Get() == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var genre = _genreRepository.Get().Where(x => x.Id == id).SingleOrDefault();

            if (genre != null)
            {
                _genreRepository.Delete(id);
            }

            _genreRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<JsonResult> Paginate([FromBody] BootstrapModel model)
        {
            model.Limit = model.Limit.HasValue ? model.Limit.Value : int.MaxValue;

            var query = _genreRepository.Get();

            var genreName = model.Search.FirstOrDefault(x => x.Name == "Genre");
            if (genreName != null && !string.IsNullOrEmpty(genreName.Value))
            {
                query = query.Where(x => x.Name.ToUpper().StartsWith(genreName.Value.ToUpper()));
            }

            var result = await query.Skip(model.Offset).Take(model.Limit.Value).Select(x => new
            {
                x.Id,
                x.Name
            }).ToListAsync();


            var _count = query.Count();

            return Json(new
            {
                rows = result,
                total = _count,
                totalNotFiltered = _count
            });
        }
    }
}
