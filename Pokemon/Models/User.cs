using System;
using System.Collections.Generic;

namespace Pokemon.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Logins = new HashSet<Login>();
        }

        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? BirthDate { get; set; }
        public string? Adrres { get; set; }
        public string? ProfileIamge { get; set; }
        public int? UserTypeId { get; set; }

        public virtual UserType? UserType { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Login> Logins { get; set; }
    }
}
