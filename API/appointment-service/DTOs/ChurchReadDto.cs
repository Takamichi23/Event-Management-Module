using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appointment_service.DTOs
{
    public class ChurchReadDto
    {
        public int ChurchId { get; set; }
        public string Name { get; set; } = null!;   
        public string Address { get; set; } = null!;

        public int AssignedToId { get; set; }
    }
}