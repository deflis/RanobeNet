using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class UserDtoForPublic
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<NovelDtoForPublicListing> Novels { get; set; }
        [Required]
        public List<UserLinkDto> Links { get; set; }
    }
}
