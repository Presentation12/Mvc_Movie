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
using Newtonsoft.Json;

namespace MvcMovie.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: User/Profile/id
        public async Task<IActionResult> Profile()
        {
            return View();
        }

        // GET: User/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // GET: User/Edit/id
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        // GET: User/Login
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
