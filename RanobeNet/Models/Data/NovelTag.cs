#nullable disable

namespace RanobeNet.Models.Data
{
    public class NovelTag
    {
        public long NovelId { get; set; }
        public virtual Novel Novel { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

