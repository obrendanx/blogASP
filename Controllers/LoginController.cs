using Microsoft.AspNetCore.Mvc;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;

        public LoginController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowUsers()
        {
            IEnumerable<User> objLoginUsers = _db.Users;
            return View(objLoginUsers);
        }
    }
}
