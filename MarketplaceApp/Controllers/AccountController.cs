using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MarketplaceApp.Models;
using MarketplaceApp.ViewModels;

namespace MarketplaceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Services.INotificationService _notificationService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            Services.INotificationService notificationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notificationService = notificationService;
        }

        // ---------------- REGISTER ----------------

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            string fileName = "default-avatar.png";

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                var directory = Path.GetDirectoryName(uploadPath);
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Location = model.Location,
                PhoneNumber = model.PhoneNumber,
                ProfileImage = "/uploads/" + fileName,
                CreatedAt = DateTime.Now,
                IsBlocked = false // التأكد من أنه غير محظور عند التسجيل
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Profile");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // ---------------- LOGIN ----------------


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "This email address is not registered.");
                return View(model);
            }


            if (user.IsBlocked)
            {
                ModelState.AddModelError("Email", "Your account has been blocked by the admin.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                try { await _notificationService.SendAsync(user.Id, Enums.NotificationType.Info, null, "Welcome back!"); } catch { }
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
                ModelState.AddModelError(string.Empty, "Account locked.");
            else
                ModelState.AddModelError("Password", "Incorrect password.");

            return View(model);
        }

        // ---------------- LOGOUT ----------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}