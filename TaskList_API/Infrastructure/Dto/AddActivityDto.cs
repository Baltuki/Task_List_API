namespace TaskList_API.Infrastructure.Dto
{
    public class AddActivityDto
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ? FinishDate { get; set; }
        public bool ? IsCompleted { get; set; }

        public int UserId { get; set; }
    }
}
