namespace Coms.Domain.Entities
{
    public class User_Access
    {
        //public int? UserId { get; set; }
        //public virtual User User { get; set; }

        public int? AccessId { get; set; }
        public virtual Access Access { get; set; } 
    }
}
