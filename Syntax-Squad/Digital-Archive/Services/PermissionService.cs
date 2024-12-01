using Digital_Archive.Models;

namespace Digital_Archive.Services
{
    public class PermissionService
    {
        private readonly AppDbContext _context;
        public PermissionService(AppDbContext context, IConfiguration config)
        {
            _context = context;

        }
        public int EmployPerFolder(int id) {
            int ans = _context.Permissions.Count(x => x.FoldedId == id && x.CanRead == true);
            return ans;
        }

         
    }
}
