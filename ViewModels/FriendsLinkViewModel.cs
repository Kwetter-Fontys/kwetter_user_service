using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.ViewModels
{
    public class FriendsLinkViewModel
    {
        public int Id { get; set; }
        public string UserFollowerId { get; set; }

        public string UserFollowingId { get; set; }
    }
}
