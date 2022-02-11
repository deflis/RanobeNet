﻿using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class NovelDtoForMe
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Author { get; set; }
    }
}
