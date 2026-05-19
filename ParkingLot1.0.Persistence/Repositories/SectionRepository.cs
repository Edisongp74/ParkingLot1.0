using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;

namespace ParkingLot1._0.Persistence.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly ApplicationDbContext _context;

        public SectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Section> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var totalCount = await _context.Sections.CountAsync(cancellationToken);

            var items = await _context.Sections
                .OrderBy(s => s.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<int> AddAsync(Section section, CancellationToken cancellationToken)
        {
            _context.Sections.Add(section);
            await _context.SaveChangesAsync(cancellationToken);

            return section.Id; 
        }
    }
}