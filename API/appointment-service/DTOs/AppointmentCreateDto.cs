namespace appointment_service.DTOs
{
    public class AppointmentCreateDto
    {
        // 1. Remove AppointmentId! The database generates this automatically.
        // public int AppointmentId { get; set; } 

        public int RequesterId { get; set; }
        public int AssignedToId { get; set; }

        // 2. Use strings for the "Incoming" data
        public string ServiceType { get; set; } // e.g., "Wedding"
        public string Status { get; set; }      // e.g., "Pending"
        
        public string StartTime { get; set; }   // e.g., "09:00:00"
        public string EndTime { get; set; }     // e.g., "10:00:00"
        public string Date { get; set; }        // e.g., "2023-12-25"
    }
}