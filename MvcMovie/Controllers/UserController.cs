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

        // GET: UserController/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                User userInsert = new User();

                userInsert.Name = user.Name;
                userInsert.Email = user.Email;

                CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                userInsert.PassHash = Convert.ToBase64String(passwordHash);
                userInsert.PassSalt = Convert.ToBase64String(passwordSalt);

                userInsert.CreatedDate = DateTime.Now;
                userInsert.UpdatedDate = DateTime.Now;
                userInsert.Active = true;

                _userRepository.Insert(userInsert);
                _userRepository.Save();
                return RedirectToAction("Login", "User");
            }
            return View(user);
        }

        // GET: UserController/Edit/id
        public async Task<IActionResult> Edit()
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password")] UserViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    User userEdit = _userRepository.Get().FirstOrDefault(x => x.Id == user.Id);
                    if (userEdit == null)
                    {
                        return NotFound();
                    }

                    userEdit.Name = user.Name;
                    userEdit.Email = user.Email;

                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        userEdit.PassHash = Convert.ToBase64String(passwordHash);
                        userEdit.PassSalt = Convert.ToBase64String(passwordSalt);
                    }

                    userEdit.UpdatedDate = DateTime.Now;

                    _userRepository.Update(userEdit);
                    _userRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userRepository.Exists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Profile", "User", new { id = user.Id });
            }
            return View(user);
        }

        // GET: UserController/Delete/id
        public async Task<IActionResult> Delete()
        {
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

        // POST: UserController/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_userRepository.Get() == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var user = _userRepository.Get().Where(x => x.Id == id).SingleOrDefault();

            if (user != null)
            {
                _userRepository.Delete(id);
            }

            _userRepository.Save();
            return RedirectToAction("Login", "User");
        }

        #region Password
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        #endregion

        // LOGIN
        #region Login
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(100),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public IActionResult Login([Bind("Email, Password")] UserViewModel account)
        {
            //Procurar na database user com email
            var user = _userRepository.Get().Where(x => x.Email == account.Email).FirstOrDefault();

            if (user != null)
            {
                if (!VerifyPasswordHash(account.Password, Convert.FromBase64String(user.PassHash), Convert.FromBase64String(user.PassSalt)))
                {
                    return new JsonResult("Wrong Pass");
                }

                string token = CreateToken(user);

                // Armazenar token de sessão
                HttpContext.Session.SetString("token", token);

                return RedirectToAction("Index", "User");
            }

            return new JsonResult("Wrong Email");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Check if the session token exists
            if (HttpContext.Session.TryGetValue("token", out byte[] token))
            {
                // Remove the session token
                HttpContext.Session.Remove("token");
            }

            // Redirect to the login page or any other page as needed
            return RedirectToAction("Login", "User");
        }

        #endregion
    }
}
