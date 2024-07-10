using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyApiNetCore8.Model
{
    
    public abstract class BaseEntity
    {
        public string CreatedBy { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreatedDate { get; set; } 
        public string ModifiedBy { get; set; }
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime ModifiedDate { get; set; }

       
    }
}
