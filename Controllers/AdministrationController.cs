using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace test_mvc_app.Controllers
{
    public class AdministrationController
: Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
         [AllowAnonymous]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel identityRoleViewModel)
        {
            if (ModelState.IsValid)
            {

                IdentityRole role = new IdentityRole { Name = identityRoleViewModel.RoleName };
                IdentityResult result = await this._roleManager.CreateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("index", "home");
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(identityRoleViewModel);
        }
    }
}