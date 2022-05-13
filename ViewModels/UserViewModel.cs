using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Location { get; set; }
        public string? Website { get; set; }
        public string? Biography { get; set; }
        public string? Image { get; set; }
    }
}
