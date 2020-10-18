using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using test.Models;
using System.Diagnostics;

namespace test.Controllers
{
    public class Error : Controller
    {
        private readonly ILogger<Error> _logger;

        public Error(ILogger<Error> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? statusCode = null)
        {
            ViewData["Error"] = statusCode;
            _logger.LogError($"Error. Status code {statusCode}");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorPro()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
