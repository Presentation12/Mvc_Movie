using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcMovieDAL.Entities;
using MvcMovieDAL;
using MvcMovie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Principal;

namespace MvcMovie.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _userRepository;

        public UserController(IConfiguration configuration, IRepository<User> userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public User GetUserByEmailAsync(string email)
        {
            return _userRepository.Get().Where(x => x.Email == email).SingleOrDefault();
        }

        // GET: UserController
        public ActionResult Index()
        {
            //// Verificar se o usuário está autenticado
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "User");
            //}

            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));

            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            var user = GetUserByEmailAsync(userMail);

            if (user == null)
            {
                return NotFound();
            }

            var userModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return View(userModel);

        }

        // GET: UserController/Profile/id
        public async Task<IActionResult> Profile()
        {
            return View();
        }

        // GET: UserController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // GET: UserController/Edit/id
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        // GET: UserController/Delete/id
        //public async Task<IActionResult> Delete()
        //{
        //    var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));

        //    var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        //    string userMail = claimMail?.Value;

        //    var user = GetUserByEmailAsync(userMail);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var userModel = new UserViewModel
        //    {
        //        Id = user.Id,
        //        Name = user.Name,
        //        Email = user.Email
        //    };

        //    return View(userModel);
        //}

        public IActionResult Login(/*string returnUrl*/)
        {
            //// Verifica se o usuário já está autenticado
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

    }
}
