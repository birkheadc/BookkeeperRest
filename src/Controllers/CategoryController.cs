using System.ComponentModel.DataAnnotations;
using BookkeeperRest.New.Filters;
using BookkeeperRest.New.Models;
using BookkeeperRest.New.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookkeeperRest.New.Controllers;

// I don't think this controller is necessary any longer, consider removing.

[ApiController]
[Route("api/category/break")]
[PasswordAuth]
public class CategoryController : ControllerBase
{

    private readonly ICategoryService categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok(categoryService.GetAllCategories());
        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }

    

    [HttpPut]
    public IActionResult UpdateAll([FromBody, Required] CategoriesWrapper categories)
    {
        try
        {
            categoryService.UpdateAllCategories(categories);
            return Ok();

        }
        catch
        {
            return BadRequest("Something went wrong...");
        }
    }
}