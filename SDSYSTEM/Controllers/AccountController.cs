using Microsoft.AspNetCore.Mvc;
using SDSYSTEM.Data;
using SDSYSTEM.Models;
using System.Threading.Tasks;

namespace SDSYSTEM.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Implement your login logic here
                // Example: Check user credentials, set authentication cookie, etc.
                // Redirect to appropriate page after login
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
