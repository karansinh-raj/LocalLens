using LocalLens.WebApi.Result;

namespace LocalLens.WebApi.Services.Auth;

public interface IAuthService : IBaseService
{
    Task<ResultT<string>> Get();
    Task<ResultT<string>> Get1();

}
