#nullable disable

namespace RanobeNet.Models.Data
{
    public class NovelAttribute
    {
        public long Id { get; set; }
        public long NovelId { get; set; }
        public virtual Novel Novel { get; set; }

        public long? EpisodeId { get; set; }
        public virtual Episode Episode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
