using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class EpisodeDtoForPublicParsed
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public List<List<dynamic>> Story { get; set; }
    }
}
