using App.Web.Models;

namespace App.Web.Service.IService
{
    public interface ICouponService
    {

        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponAsync(string couponCode);
        Task<ResponseDto?> GetCouponByIdAsync(int Id);
        Task<ResponseDto?> CreateCouponAsync(CouponDto couponCode);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto couponCode);
        Task<ResponseDto?> DeleteCouponAsync(int id);



    }
}
