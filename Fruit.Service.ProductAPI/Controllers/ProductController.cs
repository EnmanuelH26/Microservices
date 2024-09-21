using AutoMapper;
using Azure;
using Fruit.Service.ProductAPI.Data;
using Fruit.Service.ProductAPI.Models;
using Fruit.Service.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fruit.Service.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
            _response = new ResponseDto();
        }


        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> productDtos = _db.Product.ToList().OrderBy(e => e.ProductId); //el IEnumerable es una lista de solo lectura, aqui se obtiene todos los registros de la tabla product

                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(productDtos);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;   

        }

        [HttpGet]
        [Route("GetByCode/{id}")]
        public ResponseDto GetByCose(int id)
        {
            try
            {
                var product = _db.Product.SingleOrDefault(p => p.ProductId == id); //el FirstOrDefault es para obtener un solo registro de la tabla product, se le pasa el id que se quiere obtener
                
                _response.Result = _mapper.Map<ProductDto>(product); //se mapena el objeto de la entidad product a la entidad productDto


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto); //se mapena el objeto de la entidad productDto a la entidad product
                _db.Add(product); //se agrega el objeto a la tabla product
                _db.SaveChanges(); //se guardan los cambios en la db    
                _response.Result = _mapper.Map<ProductDto>(product); //se mapena el objeto de la entidad product a la entidad productDto

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _db.Update(product);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.Result = false;
                _response.Message = ex.Message;
            }
            return _response;   
        }


        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        [Route("{id}")]

        public ResponseDto Delete(int id)
        {
            try
            {
                var product = _db.Product.FirstOrDefault(p => p.ProductId == id);
                _db.Remove(product);
                _db.SaveChanges();

                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
