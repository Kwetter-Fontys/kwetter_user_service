using UserService.Models;
using Microsoft.EntityFrameworkCore;
namespace UserService.DAL
{
    public class UserInitializer
    {
        public static void Initialize(UserContext context)
        {
            //if (context.Users.Any())
            //{
                //return; //DB has been seeded already
            //}

            // Drops and creates new database with filler code. Useful for now but should be done differently later.
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //Add some users

            List<User> users = new List<User>
            {
                   new User("Sebas", "Bakker"){ Id = 1, Biography= "Student at Fontys Hogeschool Eindhoven. Creator of Kwetter and everything else.", Location = "Amsterdam", Website = " www.kwetter.nl", 
                       },
                        
                   new User("Sebas2", "Bakker"){ Id = 2, Biography= "Hallo2", Location = "Papendrecht2", Website = "utube.com2"},
                   new User("Rick", "Paans"){ Id = 3, Biography= "Hallo3", Location = "Papendrecht3", Website = "utube.com3"},
                   new User("Pim", "Paans"){ Id = 4, Biography= "Hallo4", Location = "Papendrecht4", Website = "utube.com4"},
                   new User("Sebas5", "Bakker"){ Id = 5, Biography= "Hallo5", Location = "Papendrecht5", Website = "utube.com5"},
                   new User("Sebas6", "Bakker"){ Id = 6, Biography= "Hallo6", Location = "Papendrecht6", Website = "utube.com6"},
                   new User("Sebas7", "Bakker"){ Id = 7, Biography= "Hallo7", Location = "Papendrecht7", Website = "utube.com7"}
            };
            context.Users.AddRange(users);

            List<FriendsLink> fl = new List<FriendsLink>
            {
                new FriendsLink{UserFollowerId = 1, UserFollowingId = 2},
                new FriendsLink{UserFollowerId = 1, UserFollowingId = 3},
                new FriendsLink{UserFollowerId = 1, UserFollowingId = 4},
                new FriendsLink{UserFollowerId = 2, UserFollowingId = 4},
            };
            context.FriendsLinks.AddRange(fl);
            context.SaveChanges();

        }
    }
}
