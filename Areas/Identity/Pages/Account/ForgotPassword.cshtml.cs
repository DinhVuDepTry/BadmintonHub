using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BadmintonHub.Models;

namespace BadmintonHub.Areas.Identity.Pages.Account;

public class ForgotPasswordModel(
    UserManager<ApplicationUser> userManager,
    IEmailSender emailSender,
    ILogger<ForgotPasswordModel> logger) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await userManager.FindByEmailAsync(Input.Email);
        if (user is not null && await userManager.IsEmailConfirmedAsync(user))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code = encodedToken, email = Input.Email },
                protocol: Request.Scheme);

            await emailSender.SendEmailAsync(
                Input.Email,
                "Đặt lại mật khẩu BadmintonHub",
                $"<p>Xin chào {System.Net.WebUtility.HtmlEncode(user.FullName)},</p><p>Nhấn vào liên kết sau để đặt lại mật khẩu:</p><p><a href=\"{callbackUrl}\">Đặt lại mật khẩu</a></p><p>Liên kết này chỉ dành cho bạn. Nếu bạn không yêu cầu, hãy bỏ qua email này.</p>");
            logger.LogInformation("Password reset requested for a confirmed account.");
        }

        return RedirectToPage("./ForgotPasswordConfirmation");
    }
}
