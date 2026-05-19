using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Sections.Commands.CreateSection
{
    public class CreateSectionCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, int>
    {
        private readonly ISectionRepository _sectionRepository;

        public CreateSectionCommandHandler(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<int> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
        {
            var newSection = new Section
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = true 
            };

            var id = await _sectionRepository.AddAsync(newSection, cancellationToken);

            return id;
        }
    }
}