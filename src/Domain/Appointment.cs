namespace Domain;

public class Appointment
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public ushort Price { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public bool IsBooked { get; set; }
    
    public Guid? UserId { get; set; }
    
    public AppUser AppUser { get; set; }
}