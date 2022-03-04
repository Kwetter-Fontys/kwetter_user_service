namespace UserService.Models
{
    public class Following
    {
        //User id of the User following. I.e. Sebas
        public int UserId { get; set; }

        //User id of the user being followed. I.e. Ariana
        public int FollowId { get; set; }
    }
}
