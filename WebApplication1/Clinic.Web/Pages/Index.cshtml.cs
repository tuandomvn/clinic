using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        public int WeekOffset { get; private set; }
        public DateTime StartOfWeek { get; private set; }
        public DateTime EndOfWeek { get; private set; }

        public void OnGet(int weekOffset = 0)
        {
            WeekOffset = weekOffset;

            var today = DateTime.Today;

            // Tính thứ Hai của tuần hiện tại (coi tuần bắt đầu từ thứ Hai)
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var mondayThisWeek = today.AddDays(-diff);

            var start = mondayThisWeek.AddDays(7 * WeekOffset);
            StartOfWeek = start;
            EndOfWeek = start.AddDays(6);
        }
    }
}
