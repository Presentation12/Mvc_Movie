using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcMovieDAL.Entities;
using MvcMovieDAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MvcMovie.Controllers
{
    public class FavouriteController : Controller
    {
        // GET: Favourite
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Favourite/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var viewModelMovie = new MovieViewModel();

            viewModelMovie.Id = id;

            return View(viewModelMovie);
        }

    }
}
