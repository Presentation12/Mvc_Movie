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
        // GET: Genre
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Genre/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

    }
}
