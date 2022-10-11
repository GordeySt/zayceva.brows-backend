namespace Infrastructure.Identity;

public interface IBaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }
}