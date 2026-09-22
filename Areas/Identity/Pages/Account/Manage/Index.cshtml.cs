using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BadmintonHub.Models;

namespace BadmintonHub.Areas.Identity.Pages.Account.Manage;

public class IndexModel(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<IndexModel> logger) : PageModel
{
    [BindProperty]
    public ProfileInput Profile { get; set; } = new();

    [BindProperty]
    public PasswordInput Password { get; set; } = new();

    public string Email { get; private set; } = string.Empty;
    public bool EmailConfirmed { get; private set; }
    public bool PhoneConfirmed { get; private set; }
    public string RoleName { get; private set; } = "Customer";
    public bool HasPassword { get; private set; }
    public string? StatusMessage { get; set; }

    public sealed class ProfileInput
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự.")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }
    }

    public sealed class PasswordInput
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Mật khẩu xác nhận chưa khớp.")]
        [Display(Name = "Xác nhận mật khẩu mới")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync(string? statusMessage = null)
    {
        StatusMessage = statusMessage;
        var user = await GetUserAsync();
        if (user is null) return NotFound("Không thể tải tài khoản người dùng.");

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostProfileAsync()
    {
        var user = await GetUserAsync();
        if (user is null) return NotFound("Không thể tải tài khoản người dùng.");

        ModelState.Clear();
        if (!TryValidateModel(Profile, nameof(Profile)))
        {
            await LoadAsync(user, loadProfile: false);
            return Page();
        }

        user.FullName = Profile.FullName.Trim();
        user.PhoneNumber = string.IsNullOrWhiteSpace(Profile.PhoneNumber)
            ? null
            : Profile.PhoneNumber.Trim();

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            AddErrors(result);
            await LoadAsync(user, loadProfile: false);
            return Page();
        }

        await signInManager.RefreshSignInAsync(user);
        logger.LogInformation("User updated their profile.");
        return RedirectToPage(new { statusMessage = "Thông tin tài khoản đã được cập nhật." });
    }

    public async Task<IActionResult> OnPostPasswordAsync()
    {
        var user = await GetUserAsync();
        if (user is null) return NotFound("Không thể tải tài khoản người dùng.");

        ModelState.Clear();
        if (!TryValidateModel(Password, nameof(Password)))
        {
            await LoadAsync(user, loadPassword: false);
            return Page();
        }

        var result = await userManager.ChangePasswordAsync(user, Password.CurrentPassword, Password.NewPassword);
        if (!result.Succeeded)
        {
            AddErrors(result);
            await LoadAsync(user, loadPassword: false);
            return Page();
        }

        await signInManager.RefreshSignInAsync(user);
        logger.LogInformation("User changed their password.");
        return RedirectToPage(new { statusMessage = "Mật khẩu đã được thay đổi." });
    }

    private async Task<ApplicationUser?> GetUserAsync() =>
        await userManager.GetUserAsync(User);

    private async Task LoadAsync(ApplicationUser user, bool loadProfile = true, bool loadPassword = true)
    {
        Email = user.Email ?? string.Empty;
        EmailConfirmed = user.EmailConfirmed;
        PhoneConfirmed = user.PhoneNumberConfirmed;
        RoleName = (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Customer";
        HasPassword = await userManager.HasPasswordAsync(user);
        if (loadProfile)
        {
            Profile = new ProfileInput
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber
            };
        }
        if (loadPassword) Password = new PasswordInput();
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
    }
}
