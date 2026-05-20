using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingLot1._0.Application.Contracts.Repositories;
using ParkingLot1._0.Web.DTOs.Users;
using Microsoft.AspNetCore.Authorization;

namespace ParkingLot1._0.Web.Controllers;

[Authorize(Roles = "Administrador")]
public class UsersController : Controller
{
    private readonly IUsersRepository _usersRepository;

    public UsersController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _usersRepository.GetUsersListAsync();
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadRolesAsync();
        return View(new CreateUserDTO());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDTO dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadRolesAsync();
            return View(dto);
        }

        await _usersRepository.CreateAsync(
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.PhoneNumber,
            dto.RoleName);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _usersRepository.GetUserByIdAsync(id);

        EditUserDTO dto = new()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RoleName = user.RoleName
        };

        await LoadRolesAsync();
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserDTO dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadRolesAsync();
            return View(dto);
        }

        await _usersRepository.UpdateAsync(
            dto.Id,
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.PhoneNumber,
            dto.RoleName);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _usersRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadRolesAsync()
    {
        var roles = await _usersRepository.GetRoleOptionsAsync();
        ViewBag.Roles = new SelectList(roles, "Name", "Name");
    }
}