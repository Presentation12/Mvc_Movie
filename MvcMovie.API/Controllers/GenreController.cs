using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MvcMovie.API.Models;
using MvcMovieDAL;
using MvcMovieDAL.Entities;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcMovie.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenreController : Controller
    {
        private readonly IRepository<Genre> _genreRepository;

        public GenreController(IRepository<Genre> genreRepository)
        {
            _genreRepository = genreRepository;
        }

        #region GET

        // GET: Genres
        [HttpGet("UsedGenres")]
        public async Task<IActionResult> UsedGenres()
        {
            IQueryable<string> genreQuery = (from m in _genreRepository.Get()
                                             orderby m.Name
                                             select m.Name).AsQueryable();

            var usedGenres = new SelectList(await genreQuery.Distinct().ToListAsync());

            return new JsonResult(usedGenres.Items);
        }

        #endregion

        #region POST

        [HttpPost("Paginate")]
        public async Task<JsonResult> Paginate([FromBody] BootstrapModel model)
        {
            model.Limit = model.Limit.HasValue && model.Limit != 0 ? model.Limit.Value : int.MaxValue;

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

        // POST: GenreController/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([Bind("Name")] GenreViewModel genre)
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

                    string jsonObject = JsonSerializer.Serialize(_genreRepository.Get().OrderBy(x => x.Id).LastOrDefault(),
                    new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });

                    if (!jsonObject.IsNullOrEmpty())
                    {
                        return new JsonResult(jsonObject);
                    }
                    return NotFound(new { error = "Movie not found." });
                }
                else
                {
                    // alert user
                    return BadRequest(new { error = "Genre already exists." });
                }
            }
            return BadRequest(new { error = "Genre was not created." });
        }

        // POST: GenreController/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            if (_genreRepository.Get() == null)
            {
                return NotFound(new { error = "Genre list empty." });
            }

            var genre = _genreRepository.Get().Where(x => x.Id == id).SingleOrDefault();

            if (genre != null)
            {
                _genreRepository.Delete(id);
            }

            _genreRepository.Save();
            return new JsonResult(true) { StatusCode = 200 };
        }

        #endregion
    }
}
