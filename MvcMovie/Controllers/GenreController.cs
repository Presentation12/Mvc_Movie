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
        // GET: GenreController
        public async Task<IActionResult> Index()
        {
            return View();
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
