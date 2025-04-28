using System.ComponentModel.DataAnnotations;

namespace UploadProject.Models
{
    public class AdminUploadedFile
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        [Required]
        public Guid UserID { get; set; }
        [Required]
        public Guid CompetitionSessionID { get; set; }
        
        public string FileName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }

        public virtual CompetitionSession? CompetitionSession { get; set; }
    }
}
