namespace Conduit.Domain.DTOs
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserProfileDto UserProfile { get; set; }
    }
}
