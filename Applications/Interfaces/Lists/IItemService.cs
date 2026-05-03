using Applications.Features.Items.Dto;
using Applications.Responses.Paginations;
using ErrorOr;

namespace Applications.Interfaces.Lists;

public interface IItemService
{
    Task<Pager<ItemDto>> GetItemsAsync(QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<ItemDto>> GetItemByIdAsync(int id, CancellationToken cancellation = default);
    Task<ErrorOr<ItemDto>> CreateAsync(ItemDtoCreate request, CancellationToken cancellation = default);
    Task<ErrorOr<ItemDto>> UpdateAsync(ItemDtoUpdate request, CancellationToken cancellation = default);
    Task<ErrorOr<string>> DeleteAsync(int id, CancellationToken cancellation = default);
}
