using System;
using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models
{
    public class User
    {
        [Key]
        public long userId { get; set; }  // Primary key in relational database
        public string userUserName { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public long userMobile { get; set; }
        public int userStatus { get; set;  }
        public int userBlockStatus { get; set;  }
        public DateTime userCreateDate { get; set; }
        public string userImages { get; set; }

    }
}
