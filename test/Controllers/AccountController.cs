using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace test.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            int i = 0;
            return View();
        }
        
    }
    
}
