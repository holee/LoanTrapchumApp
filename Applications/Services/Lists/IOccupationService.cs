
using Applications.Commons.Mappers;
using Applications.Features.Settings.Requests;
using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels;
using Domains.Shared.Enums;

namespace Applications.Services.Lists
{

    public sealed class OccupationService(IOccupationService oService) : IOccupationServiceImpl
    {
        public async Task<IEnumerable<Occupation>> GetOccupationsAsync(string? expression = null)
        {
            return await oService.GetOccupationsAsync(expression);
        }

        public async Task<Result<OccupationVm>> CreateAsync(OccupationDtoCreate req)
        {

            var model = req.MapCreate();

            return await oService.CreateAsync(model);

        }
        public async Task<Pager<OccupationVm>> GetOccupationsAsync(int? id, QueryParam param, CancellationToken cancellation = default)
        {
            return await oService.GetOccupationsAsync(id, param, cancellation);
        }

        public async Task<Occupation?> GetOccupationByIdAsync(int? id)
        {
            return await oService.GetOccupationByIdAsync(id);
        }

        public async Task<Pager<T>> GetOccupations<T>(int? id, string? term = null)
        {
            return await oService.GetOccupations<T>(id, term);
        }

        public async Task<Pager<T>> GetOccupations<T>(int? id, TypeOfEnum typeOf, string? term = null)
        {
            return await oService.GetOccupations<T>(id, typeOf, term);
        }


    }
}
