using MyApiNetCore8.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyApiNetCore8.Model
{
    public class TaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "ENUM('URGENT', 'HIGH', 'MEDIUM', 'LOW')")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; set; } = Priority.LOW;

        public DateTime DueDate { get; set; }

        [Column(TypeName = "ENUM('TODO', 'INPROCESS', 'DONE')")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; } = Status.TODO;

        public IList<string> Labels { get; set; }
        [JsonIgnore]
        public ICollection<TaskMember> TaskMembers { get; set; }
        public ChatGroup ChatGroup { get; set; }
    }
}