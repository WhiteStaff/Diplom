using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.DbModels
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string ProtectedData { get; set; }
    }
}