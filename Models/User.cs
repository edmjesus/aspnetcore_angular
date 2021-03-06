using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace dotnetFun.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastActive { get; set; }

        public string About { get; set; }

        public string LookingFor { get; set; }

        public string Interest { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public User() {
            Photos = new Collection<Photo>();
        }

    }
}