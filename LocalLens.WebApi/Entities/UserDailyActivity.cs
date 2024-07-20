using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalLens.WebApi.Entities
{

    public class UserDailyActivity : BaseEntity
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }
        public string S3Url { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}
