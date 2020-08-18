using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace test_mvc_app.Areas.Admin.Controllers
{
    
   
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
        public string PrintViews([FromServices] ApplicationPartManager manager)
        {
            var feature = new ViewsFeature();
            manager.PopulateFeature(feature);
            return string.Join("\n", feature.ViewDescriptors.Select(d => d.RelativePath));
        }
    }
}
