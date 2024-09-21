using App.Web.Models;
using App.Web.Service;
using App.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace App.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> productList = new();

            ResponseDto? response = await _productService.GetAllProductsAsync();

            if(response != null && response.IsSuccess)
            {
                 productList = JsonConvert.DeserializeObject<List<ProductDto>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(productList);
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {

                ResponseDto? response = await _productService.CreateProductAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? response = await _productService.GetProductAsync(Convert.ToString(productId));

            if(response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";   
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return NoContent();
        }
    }
}
