using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using LocalLens.WebApi.Contracts.UserDailyActivities;
using LocalLens.WebApi.DependencyInjection;
using LocalLens.WebApi.Entities;
using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.UserDailyActivities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers
{

    [Route("user-activity")]
    public class UserDailyActivityController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserDailyActivityService _userDailyActivityService;

        public UserDailyActivityController(IConfiguration configuration, IUserDailyActivityService userDailyActivityService)
        {
            _configuration = configuration;
            _userDailyActivityService = userDailyActivityService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadUserDailyActivity([FromForm] UploadUserDailyActivity request)
        {
            //if (request.File == null || request.File.Length == 0)
            //{
            //    return result.Match(Ok,Problem);
            //}

            string bucketName = _configuration.GetValue<string>("AWS:AWSS3BucketName");
            string bucketRegion = _configuration.GetValue<string>("AWS:AWSS3BucketRegion");
            string accessKey = _configuration.GetValue<string>("AWS:AWSAccessKeyId");
            string secretKey = _configuration.GetValue<string>("AWS:AWSSecretAccessKey");
            var newRegion = RegionEndpoint.GetBySystemName(bucketRegion);
    
            try
            {
                using (var amazonS3client = new AmazonS3Client(accessKey, secretKey, newRegion))
                {

                    var fileName = Path.GetFileNameWithoutExtension(request.File.FileName) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(request.File.FileName);


                    using (var newMemoryStream = new MemoryStream())
                    {
                        request.File.CopyTo(newMemoryStream);

                        var putRequest = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = fileName,
                            InputStream = newMemoryStream,
                            ContentType = request.File.ContentType
                        };

                        await amazonS3client.PutObjectAsync(putRequest);
                    }

                    var s3Url = $"https://{bucketName}.s3.{bucketRegion}.amazonaws.com/{fileName}";

                    var fileRecord = new UserDailyActivityRequest
                    {
                        FileName = fileName,
                        S3Url = s3Url,
                        UserId = User.GetUserId(),
                    };

                   var result =   await _userDailyActivityService.AddUserDailyActivity(fileRecord);
                   return result.Match(Ok,Problem);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return StatusCode(500, "Please check the AWS Credentials.");
                }
                else
                {
                    return StatusCode(500, amazonS3Exception.Message);
                }
            }
         
        }

        [HttpGet("{userid}")]
        public async Task<IActionResult> GetUserDailyActivity(string userid)
        {
            string bucketName = _configuration.GetValue<string>("AWS:AWSS3BucketName");
            string bucketRegion = _configuration.GetValue<string>("AWS:AWSS3BucketRegion");
            string accessKey = _configuration.GetValue<string>("AWS:AWSAccessKeyId");
            string secretKey = _configuration.GetValue<string>("AWS:AWSSecretAccessKey");
            var region = RegionEndpoint.GetBySystemName(bucketRegion);

            try
            {
                using (var s3Client = new AmazonS3Client(accessKey, secretKey, region))
                {
                    var userDailyActivity = await _userDailyActivityService.GetUserDailyActivityByUserId(userid);
                    var getRequest = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = userDailyActivity.FileName
                    };

                    using (var response = await s3Client.GetObjectAsync(getRequest))
                    {
                        if (response.ResponseStream == null)
                        {
                            return NotFound("File not found.");
                        }

                        var memoryStream = new MemoryStream();
                        await response.ResponseStream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        return File(memoryStream, response.Headers["Content-Type"], userDailyActivity.FileName);
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                     amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return StatusCode(500, "Please check the AWS Credentials.");
                }
                else
                {
                    return StatusCode(500, amazonS3Exception.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
