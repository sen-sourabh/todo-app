using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListAPI.Models
{
    public class TodoItem
    {
        [Key]
        public long todoId { get; set; }    // Primary key in relational database
        [ForeignKey("userId")]
        public long todoUserId { get; set; }
        public string todoTitle { get; set; }
        public string todoDesc { get; set; }
        public string todoColor { get; set; }
        public int todoStatus { get; set; }
        public DateTime todoCreateDate { get; set; }
        public DateTime todoDate { get; set; }
        public string todoImage { get; set; }
        public int IsComplete { get; set; }
        public int todoPriority { get; set; }
    }
}
