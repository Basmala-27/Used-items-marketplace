using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using MarketplaceApp.Models;

namespace MarketplaceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ---------------- REGISTER ----------------

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }
        [HttpPost]
        // ضيفي ده في الـ Constructor فوق مع الـ UserManager
        // private readonly IWebHostEnvironment _webHostEnvironment;

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // الصورة افتراضية في البداية
            string fileName = "default-avatar.png";

            // التحقق لو اليوزر رفع صورة (Optional)
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                // التأكد من وجود الفولدر
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
                // لو مرفعش صورة، الـ fileName هيفضل default-avatar.png
                ProfileImage = "/uploads/" + fileName,
                CreatedAt = DateTime.Now
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

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "This email address is not registered.");
                return View(model);
            }


            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }


            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account locked due to too many failed attempts.");
            }
            else
            {
                ModelState.AddModelError("Password", "Incorrect password. Please try again.");
            }

            return View(model);
        }
        // ---------------- LOGOUT ----------------

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}