namespace EmailSender.Data.Entities.Users
{
    public class EmailSendingGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPending { get; set; }
    }
}
