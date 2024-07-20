namespace LocalLens.WebApi.Contracts.UserDailyActivities
{
    public class UserDailyActivityRequest
    {
        public string FileName { get; set; }
        public string S3Url { get; set; }
        public Guid UserId { get; set; }
    }
}
