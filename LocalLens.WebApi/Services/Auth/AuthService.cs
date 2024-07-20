using LocalLens.WebApi.Result;

namespace LocalLens.WebApi.Services.Auth;

public class AuthService : IAuthService
{
    public async Task<ResultT<string>> Get()
    {
        await Task.Delay(1000);
        return ("Hello world", "Success");
    }

    public async Task<ResultT<string>> Get1()
    {
        await Task.Delay(1000);
        var error = Error.NotFound("can not found");
        return error;
    }
}
