using System.ComponentModel.DataAnnotations.Schema;

namespace EmailSender.Data.Entities.Users
{
    public class EmailSendingStatus
    {
        public int Id { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public EmailSendingGroup Group { get; set; }


        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public bool Successful { get; set; }
        // Group


    }
}
