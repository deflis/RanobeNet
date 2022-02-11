using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class UserDtoForPublicListing
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
