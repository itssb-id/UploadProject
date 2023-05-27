namespace UploadProject.ViewModels
{
    public class CompetitorAnswerHistory
    {
        public Guid? UserId { get; set; } = null;
        public Guid CompetitionSessionID { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> UploadedFileZipExtractList { get; set; } = new List<string>();
    }
}
