using System;
using System.Text;
using System;
using System.Collections.Generic;
using ParkingLot1._0.Application.SimpleMediator;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Application.Interfaces;

namespace ParkingLot1._0.Application.Features.Sections.Queries.GetSectionsList
{
    public class GetSectionsListQuery : IRequest<PagedResult<SectionDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class GetSectionsListQueryHandler : IRequestHandler<GetSectionsListQuery, PagedResult<SectionDto>>
    {
        private readonly ISectionRepository _sectionRepository;

        public GetSectionsListQueryHandler(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<PagedResult<SectionDto>> Handle(GetSectionsListQuery request)
        {
            var (items, totalCount) = await _sectionRepository.GetPagedAsync(request.PageNumber, request.PageSize, CancellationToken.None);

            var dtos = new List<SectionDto>();
            foreach (var s in items)
            {
                dtos.Add(new SectionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    IsActive = s.IsActive
                });
            }

            return new PagedResult<SectionDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PagedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
