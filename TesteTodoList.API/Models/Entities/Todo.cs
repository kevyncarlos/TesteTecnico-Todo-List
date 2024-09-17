using TesteTodoList.API.Models.Enums;

namespace TesteTodoList.API.Models.Entities;

public class Todo : EntityBase
{
    public Todo()
    {
        Status = TodoStatusEnum.Pending;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? FinishDate { get; set; }
    public TodoStatusEnum Status { get; set; }
}
