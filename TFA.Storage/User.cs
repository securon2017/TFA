﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [MaxLength(20)]
        public string Login { get; set; }

        [MaxLength(120)]
        public string Salt { get; set; }

        [MaxLength(300)]
        public string PasswordHash { get; set; }


        [InverseProperty(nameof(Topic.Author))]
        public ICollection<Topic> Topics { get; set; }


        [InverseProperty(nameof(Comment.Author))]
        public ICollection<Comment> Comments { get; set; }


    }
}