using UserService.Models;
using Microsoft.EntityFrameworkCore;
namespace UserService.DAL
{
    public static class UserInitializer
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
                   new User("sebas", "bakker"){ Id = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", Biography= "Student at Fontys Hogeschool Eindhoven. Creator of Kwetter and everything else.", Location = "Amsterdam", Website = " www.kwetter.nl", Image = "./assets/test.jpg"
                       },
                        
                   new User("Sebas2", "Bakker"){ Id = "c888f6c2-d4ce-442f-b630-52a91150f22a", Biography= "Hallo2", Location = "Papendrecht2", Website = "utube.com2", Image = "./assets/randomPerson1.png" },
                   new User("Rick", "Paans"){ Id = "1", Biography= "Hallo3", Location = "Papendrecht3", Website = "utube.com3", Image = "./assets/randomPerson2.png" },
                   new User("Pim", "Paans"){ Id = "2", Biography= "Hallo4", Location = "Papendrecht4", Website = "utube.com4", Image = "./assets/randomPerson3.png" },
                   new User("Sebas5", "Bakker"){ Id = "3", Biography= "Hallo5", Location = "Papendrecht5", Website = "utube.com5", Image = "./assets/randomPerson4.png" },
                   new User("Sebas6", "Bakker"){ Id = "4", Biography= "Hallo6", Location = "Papendrecht6", Website = "utube.com6", Image = "./assets/randomPerson5.png" },
                   new User("Sebas7", "Bakker"){ Id = "5", Biography= "Hallo7", Location = "Papendrecht7", Website = "utube.com7", Image = "./assets/randomPerson6.png" }
            };
            context.Users.AddRange(users);

            List<FriendsLink> fl = new List<FriendsLink>
            {
                new FriendsLink{UserFollowerId = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", UserFollowingId = "c888f6c2-d4ce-442f-b630-52a91150f22a"},
                new FriendsLink{UserFollowerId = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", UserFollowingId = "3"},
                new FriendsLink{UserFollowerId = "7cc35fc6-0eaf-4df8-aaef-773077b4f3c9", UserFollowingId = "4"},
                new FriendsLink{UserFollowerId = "c888f6c2-d4ce-442f-b630-52a91150f22a", UserFollowingId = "4"},
            };
            context.FriendsLinks.AddRange(fl);
            context.SaveChanges();

        }
    }
}
