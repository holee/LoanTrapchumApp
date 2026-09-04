using Application.Features.Journals.Dtos;
using Domains.Models.Journals;

namespace Application.Commons.Mappers;

public static class JournalMappers
{
    extension(JournalType value)
    {
        public JournalTypeDto ToDto()
        {
            return new JournalTypeDto
            {
                Id = value.Id,
                Code = value.Code,
                Description = value.Description
            };
        }
    }
}