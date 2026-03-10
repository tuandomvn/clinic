using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Clinic.Services.Models.Appointment;
using Clinic.Services.Services.Appointments;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public IndexModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public int WeekOffset { get; private set; }
        public DateTime StartOfWeek { get; private set; }
        public DateTime EndOfWeek { get; private set; }

        public IReadOnlyList<AppointmentResponse> Appointments { get; private set; } = Array.Empty<AppointmentResponse>();

        public async Task OnGetAsync(int weekOffset = 0)
        {
            WeekOffset = weekOffset;

            var today = DateTime.Today;
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var mondayThisWeek = today.AddDays(-diff);

            var start = mondayThisWeek.AddDays(7 * WeekOffset);
            StartOfWeek = start;
            EndOfWeek = start.AddDays(6);

            // Tạm lấy lịch của ngày hôm nay trong tuần được chọn (có thể thay bằng filter theo ngày được chọn từ UI)
            var date = DateOnly.FromDateTime(today);
            Appointments = await _appointmentService.GetByDateAsync(date);
        }
    }
}
