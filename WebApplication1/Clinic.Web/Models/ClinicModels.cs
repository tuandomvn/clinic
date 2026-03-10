using System;

namespace Clinic.Web.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime Birth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
    }

    public class Visit
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string Doctor { get; set; }
        public string Summary { get; set; }
    }

    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string Doctor { get; set; }
        public string Type { get; set; }
    }
}
