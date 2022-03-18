#nullable disable

namespace RanobeNet.Models.Data
{
    public class UserLink
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }

        public string Link { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
