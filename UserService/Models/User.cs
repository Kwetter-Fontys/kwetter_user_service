﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string Biography { get; set; }

        //public virtual ICollection<Followers> Followers { get; set; }
        //public virtual ICollection<Following> Following { get; set; }
    }
}