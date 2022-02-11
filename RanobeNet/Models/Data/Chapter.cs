using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Data
{
    public class Chapter
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public long NovelId { get; set; }
        public virtual Novel Novel { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
