﻿using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BudgetManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Get: Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //Get: NotFound
        public IActionResult ViewNotFound()
        {
            return View();
        }

        [HttpGet]
        //Get: Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}