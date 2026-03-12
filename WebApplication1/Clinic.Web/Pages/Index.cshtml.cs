using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Clinic.Services.Models.Appointment;
using Clinic.Services.Services.Appointments;
using Clinic.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ClinicDbContext _dbContext;

        public IndexModel(IAppointmentService appointmentService, ClinicDbContext dbContext)
        {
            _appointmentService = appointmentService;
            _dbContext = dbContext;
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

            //// Tạm lấy lịch của ngày hôm nay trong tuần được chọn (có thể thay bằng filter theo ngày được chọn từ UI)
            //var date = DateOnly.FromDateTime(today);
            //Appointments = await _appointmentService.GetByDateAsync(date);
        }

        public async Task<IActionResult> OnGetAppointmentsAsync(
            int draw = 1, 
            int start = 0, 
            int length = 10,
            CancellationToken ct = default)
        {
            try
            {
                var query = _dbContext.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
                    .AsQueryable();

                // Get sorting parameters from DataTables
                var orderColumn = HttpContext.Request.Query["order[0][column]"].ToString();
                var orderDir = HttpContext.Request.Query["order[0][dir]"].ToString();

                // Default sorting by ScheduledAt descending
                if (string.IsNullOrEmpty(orderColumn))
                {
                    query = query.OrderByDescending(a => a.ScheduledAt);
                }
                else if (int.TryParse(orderColumn, out int colIndex))
                {
                    // 0 = ScheduledAt, 3 = Reason, 4 = Status
                    bool isDesc = orderDir.Equals("desc", StringComparison.OrdinalIgnoreCase);

                    switch (colIndex)
                    {
                        case 0: // ScheduledAt
                            query = isDesc 
                                ? query.OrderByDescending(a => a.ScheduledAt)
                                : query.OrderBy(a => a.ScheduledAt);
                            break;
                        case 3: // Reason
                            query = isDesc
                                ? query.OrderByDescending(a => a.Reason)
                                : query.OrderBy(a => a.Reason);
                            break;
                        case 4: // Status
                            query = isDesc
                                ? query.OrderByDescending(a => a.Status)
                                : query.OrderBy(a => a.Status);
                            break;
                        default:
                            query = query.OrderByDescending(a => a.ScheduledAt);
                            break;
                    }
                }

                var totalCount = await query.CountAsync(ct);
                var pagedAppointments = await query
                    .Skip(start)
                    .Take(length)
                    .ToListAsync(ct);

                var result = new
                {
                    draw,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount,
                    data = pagedAppointments.Select(a => new
                    {
                        id = a.Id,
                        patientName = a.Patient?.FullName ?? "-",
                        patientId = a.PatientId,
                        staffName = a.Staff?.FullName ?? "-",
                        staffId = a.StaffId,
                        reason = a.Reason ?? "-",
                        scheduledAt = a.ScheduledAt.ToString("yyyy-MM-dd HH:mm"),
                        status = a.Status.ToString()
                    }).ToArray()
                };

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while loading appointments" });
            }
        }
    }
}
