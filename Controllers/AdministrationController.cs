using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace test_mvc_app.Controllers
{

    [Authorize(Roles="Admin")]
    [Authorize(Roles="User")]
    public class AdministrationController
: Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public readonly UserManager<ApplicationUser> _userManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager
        )
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
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

        [HttpGet]
        public IActionResult ListRole()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {Id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users)
            {
                bool res = await _userManager.IsInRoleAsync(user, role.Name);

                if (res)
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id  = {model.Id} cannot be founded";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.RoleId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else { userRoleViewModel.IsSelected = false; }


                model.Add(userRoleViewModel);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await this._roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await this._userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await this._userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await this._userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await this._userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await this._userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if(result.Succeeded){
                    if(i<(model.Count-1)){
                        continue;
                    }
                    else{
                        return RedirectToAction("EditRole",new {Id=roleId });
                    }
                }
            }
             return RedirectToAction("EditRole",new {Id=roleId });

        }

       
    }
}