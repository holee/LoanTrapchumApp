using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.AdditionalInformations.Queries.GetOccupationDetailsById;

public sealed record GetOccupationByIdQueryHandler : IRequestHandler<GetOccupationDetailsByIdQuery, ErrorOr<OccupationDto>>
{
    private readonly IOccupationService _occupationService;
    public GetOccupationByIdQueryHandler(IOccupationService occupationService)
    {
        _occupationService = occupationService;
    }
    public async Task<ErrorOr<OccupationDto>> Handle(GetOccupationDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var occ = await _occupationService.GetOccupationDetailByIdAsync(request.Id, cancellationToken);

            if (occ is null)
            {
                return Error.NotFound(description: $"Occupation with ID {request.Id} was not found.");
            }

            return occ;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
       
    }
}
