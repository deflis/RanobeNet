#nullable disable
namespace RanobeNet.Models.Data
{
    public class Episode
    {
        public long Id { get; set; }
        public long NovelId { get; set; }
        public virtual Novel Novel { get; set; }
        public long? ChapterId { get; set; }
        public virtual Chapter Chapter { get; set; }
        public string Title { get; set; }
        public string Story { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
