using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BestChoice.API.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string PicUrl { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }
    }
}