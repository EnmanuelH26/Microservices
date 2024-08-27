﻿using App.Web.Models;
using App.Web.Service;
using App.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace App.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async  Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list = new();

            ResponseDto? response = await _couponService.GetAllCouponsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(response.Result.ToString());
            }
            return View(list);
        }

        public async Task<IActionResult> CreateCoupon()
        {

        return View();
		}


        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
            }
            return View(model);
        }
    }
}
