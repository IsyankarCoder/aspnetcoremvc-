using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace test_mvc_app.Areas.Admin.Controllers
{
    
   
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
