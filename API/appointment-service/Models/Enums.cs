namespace appointment_service.Models
{
    public enum ServiceType
    {
        Funeral,
        Baptism,
        Counsel,
        Wedding,
        ChildDedication // Use PascalCase here; C# will handle the string conversion
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Rescheduled,
        Cancelled,
        Complete
    }
}