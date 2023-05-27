using System.ComponentModel.DataAnnotations;

namespace UploadProject.Models
{
    public class CompetitorUploadedFile
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public Guid UserID { get; set; }

        public Guid CompetitionSessionID { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual User User { get; set; }

        public virtual CompetitionSession CompetitionSession { get; set; }
    }
}