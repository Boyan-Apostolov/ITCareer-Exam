﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITCareerProject.Data;
using ITCareerProject.Services.DTOs.Users;
using ITCareerProject.Services.RolesService;
using ITCareerProject.Services.UsersService;

namespace ITCareerProject.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;

        public UsersController(IUsersService usersService,
            IRolesService rolesService)
        {
            _usersService = usersService;
            _rolesService = rolesService;
        }

        public async Task<IActionResult> Index(string keyword = null)
        {
            var users = await _usersService.GetFilteredUsers(keyword);
            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _usersService.CreateAsync(userDto);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }

            if (ModelState.ErrorCount != 0)
            {
                return View(userDto);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.AvailableRoles = _rolesService.GetAllAsKeyValuePairs();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BaseUserDto userDto)
        {
            ModelState.Remove(nameof(BaseUserDto.RoleName));

            if (id != userDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _usersService.EditAsync(userDto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableRoles = _rolesService.GetAllAsKeyValuePairs();
            return View(userDto);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _usersService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
