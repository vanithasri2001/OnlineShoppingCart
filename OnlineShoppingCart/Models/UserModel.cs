using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingCart.Models
{
    public class UserModel
    {
       [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public long PhoneNo { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
