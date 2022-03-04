using UserService.DAL;

namespace UserService
{ 
    class Program
    {
        static void Main(string[] args)
        {
            UserInitializer userInit = new UserInitializer();
            userInit.SeedData();
        }
    }
}