using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class couponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;   
        public couponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> couponsList =_db.Coupon.ToList();
     

                _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(couponsList);
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
            
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Coupon coupon = _db.Coupon.FirstOrDefault(u => u.CouponId == id);
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);  

                //mapeo right-left
                //CouponDto couponDto = new CouponDto()
                //{
                //    CouponId = coupon.CouponId,
                //    CouponCode = coupon.CouponCode,
                //    DiscountAmount = coupon.DiscountAmount,
                //    MinAmount = coupon.MinAmount
                //};
                //_responseDto.Result = coupon;
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess=false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }


        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Coupon coupon = _db.Coupon.FirstOrDefault(u => u.CouponCode.ToLower().Trim() == code.ToLower().Trim());

                if (coupon == null)
                {
                    _responseDto.IsSuccess = false;
                }
                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }
        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                _db.Coupon.Add(_mapper.Map<Coupon>(couponDto)); //le pasamos el dto y lo mapea a la entidad
                _db.SaveChanges();
                _responseDto.Result = _mapper.Map<CouponDto>(couponDto);
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }
        [HttpPut]
        public ResponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                _db.Coupon.Update(_mapper.Map<Coupon>(couponDto)); //le pasamos el dto y lo mapea a la entidad
                _db.SaveChanges();
                _responseDto.Result = _mapper.Map<CouponDto>(couponDto);
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }
        [HttpDelete]
        public ResponseDto Delete(int id)
        {
            try
            {
                Coupon coupon = _db.Coupon.FirstOrDefault(c => c.CouponId == id); //le pasamos el dto y lo mapea a la entidad
                _db.Coupon.Remove(coupon);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;

        }
    }
}
