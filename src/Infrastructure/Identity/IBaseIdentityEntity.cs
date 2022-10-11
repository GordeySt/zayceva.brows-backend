namespace Infrastructure.Identity;

public interface IBaseIdentityEntity
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsDeleted { get; set; }
}