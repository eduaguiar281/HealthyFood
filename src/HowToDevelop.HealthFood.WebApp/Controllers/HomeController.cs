﻿using Microsoft.AspNetCore.Mvc;

namespace HowToDevelop.HealthFood.WebApp.Controllers
{
    public class HomeController: Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

    }
}
