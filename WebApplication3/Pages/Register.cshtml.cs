using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.ViewModels;
using WebApplication3.Model;
using Microsoft.AspNetCore.DataProtection;
using System.Text.RegularExpressions;
using System.Text.Encodings.Web;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        private readonly IWebHostEnvironment _environment;

        private HtmlEncoder _htmlEncoder;
        private JavaScriptEncoder _javaScriptEncoder;
        private UrlEncoder _urlEncoder;


        [BindProperty]
        public MembershipRegister MyRegisterModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IWebHostEnvironment env,
        HtmlEncoder htmlEncoder, JavaScriptEncoder javascriptEncoder, 
        UrlEncoder urlEncoder)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = env;
            _htmlEncoder = htmlEncoder;
            _javaScriptEncoder = javascriptEncoder;
            _urlEncoder = urlEncoder;
        }

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                // Upload (Image)
                if (Upload != null)
                {
                    if (Regex.IsMatch(Upload.FileName, @"^[\w,\s-]+\.jpg$") )
                    {
                        var imageFile = Guid.NewGuid() + Path.GetExtension(Upload.FileName);
                        var file = Path.Combine(_environment.ContentRootPath, "wwwroot\\uploads", imageFile);
                        using var fileStream = new FileStream(file, FileMode.Create); await Upload.CopyToAsync(fileStream);
                        MyRegisterModel.Photo = "/uploads/" + imageFile;
                        Console.WriteLine("Assigned photo value" + MyRegisterModel.Photo);
                    }
                    else
                    {
                        TempData["FlashMessage.Type"] = "danger";
                        TempData["FlashMessage.Text"] = string.Format("Invalid file name: {0}. Only .jpg file is accepted", Upload.FileName);
                        return Page();
                    }
                    
                }

                //Sanitize AboutMe 
                var EncodedAboutMe = _htmlEncoder.Encode(MyRegisterModel.AboutMe);

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("ProtecKey");
                var user = new ApplicationUser()
                {   
                    /*UserName = protector.Protect(MyRegisterModel.EmailAddress),
                    Email = protector.Protect(MyRegisterModel.EmailAddress),*/
                    UserName = MyRegisterModel.EmailAddress,
                    Email = MyRegisterModel.EmailAddress,
                    FullName = protector.Protect(MyRegisterModel.FullName),
                    CreditCard = protector.Protect(MyRegisterModel.CreditCard),
                    Gender = protector.Protect(MyRegisterModel.Gender),
                    MobileNumber = protector.Protect(MyRegisterModel.MobileNumber),
                    DeliveryAddress = protector.Protect(MyRegisterModel.DeliveryAddress),
                    Photo = protector.Protect(MyRegisterModel.Photo),
                    AboutMe = protector.Protect(EncodedAboutMe)

                };
                var result = await userManager.CreateAsync(user, MyRegisterModel.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);

                    TempData["FlashMessage.Type"] = "success";
                    TempData["FlashMessage.Text"] = string.Format("User, {0} has been registered", MyRegisterModel.FullName);
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }

    }
}
