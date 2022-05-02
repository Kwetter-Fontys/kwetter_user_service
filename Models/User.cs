using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Location { get; set; }
        public string? Website { get; set; }
        public string? Biography { get; set; }
        public string? Image { get; set; }

        public User(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;  
        }
    }
}
