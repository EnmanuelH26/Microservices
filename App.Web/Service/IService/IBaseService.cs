using App.Web.Models;

namespace App.Web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto>SendAsync(RequestDto requestDto, bool withBearer = true);   
    }
}
