using Amazon.Runtime.Internal;
using LocalLens.WebApi.Constants.Configurations;
using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.Contracts.UserDailyActivities;
using LocalLens.WebApi.Database;
using LocalLens.WebApi.DependencyInjection;
using LocalLens.WebApi.Entities;
using LocalLens.WebApi.Errors.Auth;
using LocalLens.WebApi.Messages.Auth;
using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace LocalLens.WebApi.Services.UserDailyActivities
{
    public class UserDailyActivityService : IUserDailyActivityService
    {
        private readonly LocalLensDbContext _dbContext;

        public UserDailyActivityService(LocalLensDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResultT<string>> AddUserDailyActivity(UserDailyActivityRequest request)
        {
            
            var userId = Guid.NewGuid();
            var userDailyActivity = new UserDailyActivity
            {
                Id = userId,
                FileName = request.FileName,
                S3Url = request.S3Url,
                UserId = request.UserId
            };

            _dbContext.UserDailyActivities.Add(userDailyActivity);
            _dbContext.SaveChanges();


            string message = "User activity recorded successfully.";
            return (message, "Daily Activity");

        }
        public async Task<UserDailyActivityResponse> GetUserDailyActivityByUserId(string userId)
        {
            var userDailyActivityDetails = _dbContext.UserDailyActivities.FirstOrDefault(x => x.UserId == new Guid(userId) && x.IsDeleted == false);

            var userDailyActivity = new UserDailyActivityResponse
            {
                Id = userDailyActivityDetails.Id.ToString(),
                FileName  = userDailyActivityDetails.FileName,
                S3Url = userDailyActivityDetails.S3Url,
                UserId = userDailyActivityDetails.UserId.ToString()
            };
            return userDailyActivity;
        }
    }
}
