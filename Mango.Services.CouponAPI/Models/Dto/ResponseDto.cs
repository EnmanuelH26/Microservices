namespace Mango.Services.CouponAPI.Models.Dto
{
    public class ResponseDto
    {
        //modelo de respuesta generico
        public object Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
