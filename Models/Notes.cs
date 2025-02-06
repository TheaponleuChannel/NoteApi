using System;
using System.ComponentModel.DataAnnotations;

namespace NoteApi.Models
{
    public class Note
    {
        public int Id { get; set; }
        
        public string Title { get; set; } 
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}