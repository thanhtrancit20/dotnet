﻿using Microsoft.AspNetCore.Identity;
using MyApiNetCore8.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyApiNetCore8.Model
{
    public class User : IdentityUser
    {
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string FirstName { get; set; }

        [Column(TypeName = "ENUM('FEMALE', 'MALE', 'OTHER')")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; } = Gender.MALE;

        public string LastName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }

        public ICollection<TaskMember> TaskMembers { get; set; }
    }
}