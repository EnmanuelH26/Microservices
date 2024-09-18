using App.Web.Models;
using App.Web.Service.IService;
using Newtonsoft.Json;
using System.Text;
using static App.Web.Utility.SD;

namespace App.Web.Service
{
    public class BaseService : IBaseService
    {
        // el IHttpClientFactory sirve para crear un cliente http, es una interfaz que nos permite crear un cliente http
        private readonly IHttpClientFactory _httpClientFactory;  
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory; 
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try { 


                HttpClient client = _httpClientFactory.CreateClient("MangoApi"); //se crea un cliente http, se le pasa el nombre del cliente.
                HttpRequestMessage message = new(); // sirve para enviar la peticion al servidor 
                message.Headers.Add("Accept", "application/json"); //se le agrega un header a la peticion, en este caso se le agrega el header de aceptar json

                //Token

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data != null)
                {
                        message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json"); //se serializa el objeto a json, serelizar es convertir un objeto a json y deserializar es convertir un json a objeto
                }
                HttpResponseMessage? apiResponse = null; //el httpresponsemessage es la respuesta que nos da el servidor, puede ser un 200, 404, 500, etc

                //segun el tipo de peticion se le asigna un metodo a la peticion
                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                            message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                            message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                            message.Method = HttpMethod.Delete;
                        break;
                    default:
                            message.Method = HttpMethod.Get;
                        break;
                }

                //se envia la peticion al servidor
                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new () { IsSuccess = false, Message = "Access Denied"};
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new () { IsSuccess = false, Message = "Unauthorized" };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new () { IsSuccess = false, Message = "Internal Error"};
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync(); //se lee la respuesta del servidor
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent); //se deserializa la respuesta del servidor
                        return apiResponseDto; //se retorna la respuesta del servidor
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
