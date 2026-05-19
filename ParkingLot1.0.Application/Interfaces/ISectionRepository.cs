using ParkingLot1._0.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingLot1._0.Application.Interfaces
{
    public interface ISectionRepository
    {
        Task<(List<Section> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<int> AddAsync(Section section, CancellationToken cancellationToken);
    }
}