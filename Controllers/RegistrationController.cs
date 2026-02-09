using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Services;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IAuthService _AuthService;
        public RegistrationController(IAuthService AuthService)
        {
            _AuthService = AuthService;
        }
        //private static User? user = new();

        [HttpPost("Register")]
        public async Task<ActionResult<User?>> Register(User request) // this request is the data will getting from client then will converting the password into hashpassword and returning user which we save in db
        {
            if (ModelState.IsValid)
            {

                var user = await _AuthService.RegisterAsync(request);
                if (user is null)
                {
                    TempData["warning"] = "User is Already Registered! Please Login";
                    return RedirectToAction("Register", "Home");
                }

                TempData["sucess"] = "Record Created Successfully";
                HttpContext.Session.SetString("Username", user.Username);//session
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Data is Invalid!";
            return RedirectToAction("Register", "Home");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User?>> Login(UserDTO request) // this request is the data will getting from client then will converting the password into hashpassword and returning user which we save in db
        {
            if (ModelState.IsValid)
            {

                var user = await _AuthService.Loginasync(request);
                if (user is null)
                {
                    TempData["warning"] = "User is not exist.Please Register";
                    return RedirectToAction("Register", "Home");
                }

                //HttpContext.Session.SetString("Username", user.Username);//session
                
                var claims = new List<Claim>
                {
                   new Claim(ClaimTypes.Name, user.Username),
                   new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                   new Claim(ClaimTypes.Role,user.Roles)
                };

                //foreach (var role in user.Roles.Split(','))
                //{
                //    claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
                //}

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);
                return RedirectToAction("Index", "Home");
            }
            TempData["error"] = "Data is Invalid!";
            return RedirectToAction("Register", "Home");
        }
    }
}
