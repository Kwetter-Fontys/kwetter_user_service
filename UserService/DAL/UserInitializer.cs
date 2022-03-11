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
                   new User{ Id = 1, FirstName = "Sebas", LastName = "Bakker", Biography= "Student at Fontys Hogeschool Eindhoven. Creator of Kwetter and everything else.", Location = "Amsterdam", Website = " www.kwetter.nl"},
                   new User{ Id = 2, FirstName = "Sebas2", LastName = "Bakker", Biography= "Hallo2", Location = "Papendrecht2", Website = "utube.com2"},
                   new User{ Id = 3, FirstName = "Sebas3", LastName = "Bakker", Biography= "Hallo3", Location = "Papendrecht3", Website = "utube.com3"},
                   new User{ Id = 4, FirstName = "Sebas4", LastName = "Bakker", Biography= "Hallo4", Location = "Papendrecht4", Website = "utube.com4"},
                   new User{ Id = 5, FirstName = "Sebas5", LastName = "Bakker", Biography= "Hallo5", Location = "Papendrecht5", Website = "utube.com5"},
                   new User{ Id = 6, FirstName = "Sebas6", LastName = "Bakker", Biography= "Hallo6", Location = "Papendrecht6", Website = "utube.com6"},
                   new User{ Id = 7, FirstName = "Sebas7", LastName = "Bakker", Biography= "Hallo7", Location = "Papendrecht7", Website = "utube.com7"}
            };
            context.Users.AddRange(users);
            context.SaveChanges();

        }
    }
}
