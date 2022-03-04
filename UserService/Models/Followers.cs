namespace UserService.Models
{
    public class Followers
    {
        //User id of the User being followed. I.e. Ariana
        public int UserId { get; set; }
        //User id of the follower. I.e. Sebas
        public int FollowerId { get; set; }

    }
}
