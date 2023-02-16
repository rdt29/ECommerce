﻿using BusinessLayer.Repository;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategories _categories;

        public CategoriesController(ICategories categories)
        {
            _categories = categories;
        }

        [HttpPost("Add Categories"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategories(String Name, int Id)
        {
            var res = await _categories.CategoryAdd(Name, Id);
            return Ok(res);
        }
    }
}