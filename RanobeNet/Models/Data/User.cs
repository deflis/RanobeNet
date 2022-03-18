#nullable disable
using RanobeNet;

namespace RanobeNet.Models.Data
{
    public class User
    {
        public long Id { get; set; }
        public string FirebaseUid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserLink> Links { get; set; }
        public virtual ICollection<Novel> Novels { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
