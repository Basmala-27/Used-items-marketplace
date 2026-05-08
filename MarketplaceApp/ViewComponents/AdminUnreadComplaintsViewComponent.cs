using MarketplaceApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MarketplaceApp.ViewComponents
{
    public class AdminUnreadComplaintsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public AdminUnreadComplaintsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int unreadCount = await _context.Complaints
                .Where(c => !c.IsReadByAdmin)
                .CountAsync();

            return View(unreadCount);
        }
    }
}
