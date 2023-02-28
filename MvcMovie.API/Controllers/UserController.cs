using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MvcMovie.API.Models;
using MvcMovieDAL;
using MvcMovieDAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Authorization;

namespace MvcMovie.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _userRepository;

        public UserController(IConfiguration configuration, IRepository<User> userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        #region GET

        // GET: User
        [HttpGet("Index")]
        public ActionResult Index(string token)
        {
            //Console.WriteLine("Token: " + HttpContext.Session.GetString("token"));
            //var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));
            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            var user = GetUserByEmailAsync(userMail);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return new JsonResult(userModel);
        }

        // GET: User/Profile/id
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile(string token)
        {
            //var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));
            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            var user = GetUserByEmailAsync(userMail);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return new JsonResult(userModel);
        }

        // GET: User/Edit/id
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(string token)
        {
            //var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));
            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            var user = GetUserByEmailAsync(userMail);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
                //password
            };

            return new JsonResult(userModel);
        }

        #endregion

        #region POST

        // POST: User/GetUserByToken
        [HttpPost("GetUserByToken")]
        public async Task<User> GetUserByToken([FromBody] JObject data)
        {
            var token = data["token"].ToString();
            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            //string userMail = User.FindFirstValue(ClaimTypes.Email);

            return _userRepository.Get().Where(x => x.Email == userMail && x.Active == true).SingleOrDefault();
        }

        // POST: UserController/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([Bind("Name,Email,Password")] UserViewModel user)
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

                string jsonObject = JsonSerializer.Serialize(_userRepository.Get().OrderBy(x => x.Id).LastOrDefault(),
                    new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });

                if (!jsonObject.IsNullOrEmpty())
                {
                    return new JsonResult(jsonObject);
                }
                return NotFound(new { error = "User not found." });
            }
            return BadRequest(new { error = "User was not created." });
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Email,Password")] UserViewModel user)
        {
            //if (id != user.Id)
            //{
            //    return NotFound(new { error = "User not found." });
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    User userEdit = _userRepository.Get().FirstOrDefault(x => x.Id == user.Id);
                    if (userEdit == null)
                    {
                        return NotFound(new { error = "User not found." });
                    }

                    userEdit.Name = user.Name;
                    userEdit.Email = user.Email;

                    if (!string.IsNullOrEmpty(user.Password) && user.Password != "undefined")
                    {
                        CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                        userEdit.PassHash = Convert.ToBase64String(passwordHash);
                        userEdit.PassSalt = Convert.ToBase64String(passwordSalt);
                    }

                    userEdit.UpdatedDate = DateTime.Now;

                    _userRepository.Update(userEdit);
                    _userRepository.Save();

                    string jsonObject = JsonSerializer.Serialize(userEdit,
                    new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });

                    // Redirects to the list of movies (Movies/Index)
                    return new JsonResult(jsonObject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userRepository.Exists(user.Id))
                    {
                        return NotFound(new { error = "User not found." });
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(user);
        }

        // POST: UserController/Delete/id
        [HttpPost("Delete"), ActionName("Delete")]
        public async Task<IActionResult> Delete([FromBody] JObject data)
        {
            var token = data["token"].ToString();

            if (_userRepository.Get() == null)
            {
                return NotFound(new { error = "User list empty." });
            }

            //var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));
            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var claimMail = tokenDecoded.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            string userMail = claimMail?.Value;

            var user = GetUserByEmailAsync(userMail);

            if (user != null)
            {
                _userRepository.Delete(user.Id);
            }

            _userRepository.Save();
            //Logout();
            return new JsonResult(true) { StatusCode = 200 };
        }

        [HttpPost("Login")]
        public IActionResult Login([Bind("Email, Password")] LoginViewModel account)
        {
            //Procurar na database user com email
            var user = _userRepository.Get().Where(x => x.Email == account.Email).FirstOrDefault();

            if (user != null)
            {
                if (!VerifyPasswordHash(account.Password, Convert.FromBase64String(user.PassHash), Convert.FromBase64String(user.PassSalt)))
                {
                    return Unauthorized();
                }

                string token = CreateToken(user);

                // Armazenar token de sessão
                HttpContext.Session.SetString("token", token);

                return new JsonResult(token) { StatusCode = 200 };
            }

            return NotFound("Wrong Email");
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Check if the session token exists
            if (HttpContext.Session.TryGetValue("token", out byte[] token))
            {
                // Remove the session token
                HttpContext.Session.Remove("token");

                return new JsonResult(true) { StatusCode = 200 };
            }

            return NotFound("Session token not found");
        }

        #endregion

        #region SECONDARY METHODS

        private User GetUserByEmailAsync(string email)
        {
            return _userRepository.Get().Where(x => x.Email == email).SingleOrDefault();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
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

        #endregion
    }
}
