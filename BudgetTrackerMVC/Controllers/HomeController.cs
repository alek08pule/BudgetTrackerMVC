
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BudgetTrackerMVC.Domains;
using BudgetTrackerMVC.ViewModels;
using BudgetTrackerMVC.ViewModels.BudgetTrackerMVC.ViewModels;
using BudgetTrackerMVC.DataAccess;
using BudgetTrackerMVC.Service;

public class HomeController : Controller
{
  

    public IActionResult Index()
    {
        return View();
    }
}
