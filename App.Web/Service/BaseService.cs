using App.Web.Models;
using App.Web.Service.IService;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using static App.Web.Utility.SD;

namespace App.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;    
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; 
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try { 

            HttpClient client = _httpClientFactory.CreateClient("MangoApi");
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Headers.Add("Accept", "application/json");

            //Token
            requestMessage.RequestUri = new Uri(requestDto.Url);

            if (requestDto.Url != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "aplication/json");
            }
            HttpResponseMessage? apiResponse = null;

            switch(requestDto.ApiType)
            {
                case ApiType.POST:
                    requestMessage.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    requestMessage.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    requestMessage.Method = HttpMethod.Delete;
                    break;
                default:
                    requestMessage.Method = HttpMethod.Get;
                    break;
            }

            apiResponse = await client.SendAsync(requestMessage);

            switch (apiResponse.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return new ResponseDto { IsSuccess = false, Message = "Not Found", Result = null };
                case System.Net.HttpStatusCode.Forbidden:
                    return new ResponseDto { IsSuccess = false, Message = "Access Denied", Result = null };
                case System.Net.HttpStatusCode.Unauthorized:
                    return new ResponseDto { IsSuccess = false, Message = "Unauthorized", Result = null };
                case System.Net.HttpStatusCode.InternalServerError:
                    return new ResponseDto { IsSuccess = false, Message = "Internal Error", Result = null };
                default:
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return responseDto;
            }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto { IsSuccess = false, Message = ex.Message, Result = null };
                return dto;
            }

        }
    }
}
