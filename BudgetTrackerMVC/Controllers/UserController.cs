using BudgetTrackerMVC.DataAccess;
using BudgetTrackerMVC.Domains;
using BudgetTrackerMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetTrackerMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly BudgetTrackerDbContext dbContext;
        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, BudgetTrackerDbContext dbContext)

        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this. dbContext = dbContext;    
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if(ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email!, model.Password!, false,false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "UserBalance");
                }
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    UserName = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                  
                    UserBalance userBalance = new UserBalance
                    {
                        UserId = user.Id, 
                        TotalIncome = 0,
                        TotalExpense = 0,
                     
                    };

                    dbContext.UserBalances.Add(userBalance);
                    await dbContext.SaveChangesAsync();

                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
    }
}
