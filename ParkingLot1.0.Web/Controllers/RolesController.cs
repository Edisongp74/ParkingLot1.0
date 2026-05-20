using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Application.Contracts.Repositories;
using ParkingLot1._0.Web.DTOs.Roles;
using Microsoft.AspNetCore.Authorization;

namespace ParkingLot1._0.Web.Controllers;

[Authorize(Roles = "Administrador")]
public class RolesController : Controller
{
    private readonly IRolesRepository _rolesRepository;

    public RolesController(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var roles = await _rolesRepository.GetRolesListAsync();
        return View(roles);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateRoleDTO());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _rolesRepository.CreateAsync(dto.Name);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var role = await _rolesRepository.GetRoleByIdAsync(id);

        EditRoleDTO dto = new()
        {
            Id = role.Id,
            Name = role.Name
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditRoleDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _rolesRepository.UpdateAsync(dto.Id, dto.Name);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        await _rolesRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}