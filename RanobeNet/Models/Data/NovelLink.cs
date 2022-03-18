#nullable disable

namespace RanobeNet.Models.Data
{
    public class NovelLink
    {
        public long NovelId { get; set; }
        public virtual Novel Novel { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

