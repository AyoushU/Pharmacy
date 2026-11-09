using MedicalShop.Application.DTOs.Categories;
using MedicalShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MedicalShop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        // create constructor comment
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

       
        public async Task<IActionResult> Index()
        {
            var categories =
                await _categoryService.GetAllAsync();

            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
        {
            try
            {
                await _categoryService.CreateAsync(createCategoryDto);

                TempData["Success"] =
                    "Category created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(createCategoryDto);
            }
        }

    }
    }

