namespace TesteTodoList.API.Models.Entities;

public class EntityBase
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsActive { get; set; }
}
