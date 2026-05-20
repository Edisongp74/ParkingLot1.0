using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Persistence.Identity;
using ParkingLot1._0.Web.DTOs.Account;

namespace ParkingLot1._0.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginDTO());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _signInManager.PasswordSignInAsync(
            dto.Email,
            dto.Password,
            dto.RememberMe,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Credenciales inválidas.");
            return View(dto);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
