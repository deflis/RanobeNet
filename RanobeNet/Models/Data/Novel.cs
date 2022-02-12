#nullable disable
namespace RanobeNet.Models.Data
{
    public class Novel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Author { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
