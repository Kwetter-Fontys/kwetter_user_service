using UserService.Models;
using Microsoft.EntityFrameworkCore;
namespace UserService.DAL
{
    public class UserInitializer
    {
        public static void Initialize(UserContext context)
        {
            if (context.Users.Any())
            {
                return; //DB has been seeded already
            }

            // Creates the database if not exists, should only be used in production
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //Add some users
            List<User> users = new List<User>
            {
                   new User{ Id = 1, Name = "Sebas", Biography= "Hallo", Location = "Papendrecht", Website = "utube.com"},
                   new User{ Id = 2, Name = "Sebas2", Biography= "Hallo2", Location = "Papendrecht2", Website = "utube.com2"},
                   new User{ Id = 3, Name = "Sebas3", Biography= "Hallo3", Location = "Papendrecht3", Website = "utube.com3"},
                   new User{ Id = 4, Name = "Sebas4", Biography= "Hallo4", Location = "Papendrecht4", Website = "utube.com4"},
                   new User{ Id = 5, Name = "Sebas5", Biography= "Hallo5", Location = "Papendrecht5", Website = "utube.com5"},
                   new User{ Id = 6, Name = "Sebas6", Biography= "Hallo6", Location = "Papendrecht6", Website = "utube.com6"},
                   new User{ Id = 7, Name = "Sebas7", Biography= "Hallo7", Location = "Papendrecht7", Website = "utube.com7"}
            };
            context.Users.AddRange(users);
            context.SaveChanges();

        }
    }
}
