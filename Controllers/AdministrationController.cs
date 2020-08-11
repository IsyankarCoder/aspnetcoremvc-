using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace test_mvc_app.Controllers
{

    [Authorize(Roles="Admin,User")]
    //[Authorize(Roles="User")]
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
        public IActionResult ListUser()
        {
            var users = this._userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id) {
            var user = await this._userManager.FindByIdAsync(id);
            if(user == null) {
                ViewBag.ErrorMessage=$"User with Id = {id} cannot be found";
                return View("NotFound");
            } 

            var userClaims = await this._userManager.GetClaimsAsync(user);
            var userRoles = await this._userManager.GetRolesAsync(user);

            var model = new EditUserViewModel{
                Id= user.Id,
                Email= user.Email,
                UserName=user.UserName,
                City = user.City,
                Claims= userClaims.Select(c=>c.Value).ToList(),
                Roles = userRoles.ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model){
            var user = await this._userManager.FindByIdAsync(model.Id);
            if(user == null) {
                ViewBag.ErrorMessage=$"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }else{

                user.Email=model.Email;
                user.UserName=model.UserName;
                user.City =model.City;

                var result = await this._userManager.UpdateAsync(user);
                if(result.Succeeded){
return RedirectToAction("ListUser");
                }

                foreach(var error in result.Errors){
                    ModelState.AddModelError("",error.Description);
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id){
            var user = await this._userManager.FindByIdAsync(id);
            if(user==null){
ViewBag.ErrorMessage=$"User with Id = {id} cannot be found";
return View("NotFound");
            }
            else{
                var result = await this._userManager.DeleteAsync(user);
                if(result.Succeeded){
                    return RedirectToAction("ListUser");
                }

                foreach(var error in result.Errors){
                    ModelState.AddModelError("",error.Description);
                }

                return View("ListUser");

            }

        }


        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id){
            var role = await this._roleManager.FindByIdAsync(id);
            if(role== null) {
                ViewBag.ErrorMessage=$"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else{
                var result  = await this._roleManager.DeleteAsync(role);
                if(result.Succeeded){
                    return RedirectToAction("ListRole");
                }
                
                foreach (var error in result.Errors){
                    ModelState.AddModelError("",error.Description);
                }
              
            }

            return View("ListRole");
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
        [Authorize(Roles="Admin")]
        public IActionResult ListRole()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
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