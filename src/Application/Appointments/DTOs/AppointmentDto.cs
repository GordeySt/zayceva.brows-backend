using Application.Common.Mappings;
using Domain;

namespace Application.Appointments.DTOs;

public class AppointmentDto : IMapFrom<Appointment>
{ 
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public bool IsBooked { get; set; }
    
    public Guid UserId { get; set; }
}