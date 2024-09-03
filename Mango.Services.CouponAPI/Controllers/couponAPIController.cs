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
        //se inyecta el contexto de la db, el mapper y el responseDto
        private readonly AppDbContext _db;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;
        //se inyecta el contexto de la db y el mapper
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
                //el IEnumerable es una lista de solo lectura, aqui se obtiene todos los registros de la tabla coupon
                IEnumerable<Coupon> couponsList =_db.Coupon.ToList();
     
                //aqui se mapean los objetos de la entidad coupon a la entidad couponDto    
                _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(couponsList);
            }
            catch (Exception ex)
            {
                //si hay un error se setean los valores de la respuesta
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            //retornamos la respuesta
            return _responseDto;
            
        }

        [HttpGet]
        [Route("{id:int}")] //se le pasa el id que se quiere obtener
        public ResponseDto Get(int id)
        {
            try
            {
                //el FirstOrDefault es para obtener un solo registro de la tabla coupon,
                //se le pasa el id que se quiere obtener
                //Aqui se obtiene un solo registro de la tabla coupon
                Coupon coupon = _db.Coupon.FirstOrDefault(u => u.CouponId == id);
                //se mapena el objeto de la entidad coupon a la entidad couponDto
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
                //si hay un error se setea los valores del response
                _responseDto.IsSuccess=false;
                _responseDto.Message = ex.Message;
            }
            //retornamos la respuesta
            return _responseDto;

        }


        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                //Se obtiene el codigo del cupon
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
        [Route("{id:int}")]
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
