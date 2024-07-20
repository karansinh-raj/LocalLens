using LocalLens.WebApi.Contracts.PlacesTypes;
using LocalLens.WebApi.Contracts.UserDailyActivities;
using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Services.UserDailyActivities
{
    public interface IUserDailyActivityService
    {
        Task<ResultT<string>> AddUserDailyActivity(UserDailyActivityRequest userDailyActivity);
        Task<UserDailyActivityResponse> GetUserDailyActivityByUserId(string userId);
    }
}
