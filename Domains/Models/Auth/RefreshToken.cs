using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models.Auth
{

    [Table("AspNetRefreshToken")]
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime? Expires { get; set; }
        public bool IsExpired => DateTime.Now >= Expires;
        public DateTime? Created { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
        public string ReplacedByToken { get; set; } = string.Empty;

        [ForeignKey("ApplicationUser")]   
        public string UserId { get; set; } = string.Empty;
    }
}
