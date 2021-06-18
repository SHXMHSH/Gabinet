using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gabinet.Data;
using Gabinet.Models;
using Gabinet.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gabinet.Controllers
{
    public class UserController : Controller
    {
        private readonly GabinetDbContext _dbContext;
        private readonly UserManager<IdentityModel> _userManager;
        private readonly SignInManager<IdentityModel> _signIn;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(GabinetDbContext dbContext, UserManager<IdentityModel> userManager, SignInManager<IdentityModel> signInManager, RoleManager<IdentityRole> roleManager)
        {

            _dbContext = dbContext;
            _userManager = userManager;
            _signIn = signInManager;
            _roleManager = roleManager;
        }

        [TempData]
        public string ResultAction { get; set; }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            IdentityModel currentUser = await _userManager.GetUserAsync(this.User);
            int userId = (int)currentUser?.UserId;
            UserModel user = await _dbContext.GetUsers.FindAsync(userId);


            return View(
                    user.PlaceId == 0 ?
                        _dbContext.GetUsers :
                        _dbContext.GetUsers.Where(x=>x.PlaceId == user.PlaceId)
                );
        }

        public ViewResult Login() => View(new LoginVModel());

        [HttpPost]
        public async Task<IActionResult> AuthUser(LoginVModel login, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityModel { UserName = login.Email, Email = login.Email };

                var result = await _signIn.PasswordSignInAsync(login.Email, login.Password, false, true);
                if (result.Succeeded)
                {
                    return returnUrl is null ? RedirectToAction("Index", "Appointment") : LocalRedirect(returnUrl);
                }
            }
            return View("Login");
        }
        public IActionResult CreateUser() => View(
                            new UserVModel { GetPlaces = _dbContext.GetPlaces.Where(x=>x.Active == true),
                             RoleIdentity = _roleManager.Roles.AsEnumerable()}
                               );

        //PW 123Pw123!Pw
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserVModel userVModel)
        {
            userVModel.UserModel.BirthDate = DateTime.Now;
            if (ModelState.IsValid)
            {

                var register = await _userManager.CreateAsync(new IdentityModel
                {
                    Email = userVModel.Email,
                    UserName = userVModel.Email
                }, userVModel.Password);


                if (register.Succeeded)
                {
                    
                    await  _dbContext.AddAsync<UserModel>(userVModel.UserModel);
                    await _dbContext.SaveChangesAsync();

                    var identityUser = await _userManager.FindByEmailAsync(userVModel.Email);
                    await _userManager.AddToRoleAsync(identityUser, userVModel.Role);

                    identityUser.UserId = userVModel.UserModel.UserId;
                    await _userManager.UpdateAsync(identityUser);

                    ResultAction = "Operacja przebiegła pomyślnie";
                    return RedirectToAction(nameof(Index));
                }

                ResultAction = "Błąd podczas wykonywania operacji";
                ViewBag.ErrorRegister = register.Errors;
                
            }
            userVModel.GetPlaces = _dbContext.GetPlaces.Where(x => x.Active == true);
            userVModel.RoleIdentity = _roleManager.Roles.AsEnumerable();
            return View(userVModel);
        }

        public async Task<IActionResult> UpdateUser(int userId = 0)
        {
            if (userId == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager?.FindByIdAsync(userId.ToString());
            return user is null ? RedirectToAction(nameof(Index)) : View("CreateUser", new IdentityVModel { IdentityModel = user, Password = null });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(IdentityVModel identityM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager?.FindByIdAsync(identityM.IdentityModel.Id);
                if (user is null)
                {
                    ResultAction = " Błąd podczas wykonywania operacji";
                    return RedirectToAction(nameof(Index));
                }
                await _userManager.UpdateAsync(identityM.IdentityModel);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, identityM.Password);

                ResultAction = "Operacja przebiegła pomyślnie";
                return RedirectToAction(nameof(Index));
            }
            return View("CreateUser", identityM);
        }

        public ViewResult Roles() => View(_roleManager.Roles);

        public async Task<RedirectToActionResult> RemoveRole(string id)
        {
           var role = await _roleManager.FindByIdAsync(id);
            if(role is not null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction(nameof(role));
            
        }

        public ViewResult CreateRole() => View(new RoleVModel { });

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleVModel roleModel)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = roleModel.Name });
                return RedirectToAction(nameof(Index));
            }
            
            return View(roleModel);
        }

    }
}
