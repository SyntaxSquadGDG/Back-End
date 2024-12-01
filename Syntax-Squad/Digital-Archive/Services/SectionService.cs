using Digital_Archive.Dto;
using Digital_Archive.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital_Archive.Services
{
    public class SectionService
    {

        private readonly AppDbContext _context;
        public SectionService(AppDbContext context, IConfiguration config)
        {
            _context = context;
        }
        public async Task<List<Section>> getallassync()
        {
            var sections = _context.Sections.ToList();
            return sections;
        }
        public async Task<List<PathDto>> BuildPathsync(int id)
        {
            Section section = await _context.Sections.FirstAsync(x => x.Id == id);

            List<PathDto> ans = new List<PathDto>();
           
            ans.Add(new PathDto
            {
                type = "section",
                Name = section.Name,
                Id = section.Id
            });
            return ans;
        }
    }
}
