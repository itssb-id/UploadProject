using System.ComponentModel.DataAnnotations;

namespace UploadProject.Models
{
    public class CompetitionSession
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public int DayNumber { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public virtual List<User> Competitors { get; set; } = [];
    }
}
