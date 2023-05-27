using System.ComponentModel.DataAnnotations;

namespace UploadProject.Models
{
    public class User
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        public bool IsAdmin { get; set; } = false;
        
        public string Username { get; set; } = ""; // PC01

        public string Password { get; set; } = ""; // 123

        public virtual List<CompetitorUploadedFile> CompetitorUploadedFiles { get; set; } = new List<CompetitorUploadedFile>();

        public virtual List<AdminUploadedFile> AdminUploadedFiles { get; set; } = new List<AdminUploadedFile>();
    }
}
