namespace LocalLens.WebApi.Contracts.UserDailyActivities
{
    public class UserDailyActivityResponse
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string FileName { get; set; }
        public string S3Url { get; set; }
    }
}
