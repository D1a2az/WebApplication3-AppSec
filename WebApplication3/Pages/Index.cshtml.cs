using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.ViewModels;
using WebApplication3.Model;
using System.Security.Claims;

namespace WebApplication3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor contxt;



        [BindProperty]
        public MembershipRegister MyRegisterModel { get; set; }

        public IndexModel(ILogger<IndexModel> logger, SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            contxt = httpContextAccessor;
        }

        public void OnGet()
        {
            /*var user_id = userManager.GetUserId(User);
            if (user_id != null)
            {
                Console.WriteLine("Current User ID" + user_id);
                ApplicationUser current_user = userManager.FindByIdAsync(user_id).Result;
                *//*MyRegisterModel.FullName = current_user.FullName;*//*

            }*/
            var current_userName = HttpContext.Session.GetString("_UserName");
            var current_user = userManager.FindByEmailAsync(current_userName);
        }


    }
}