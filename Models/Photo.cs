using System;

namespace dotnetFun.API.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string About { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool isMain { get; set; }

        public string PublicId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}