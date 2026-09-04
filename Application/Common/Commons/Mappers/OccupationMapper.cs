using Domains.Models.Lists;
using Application.Features.AdditionalInformations.Dto;


namespace Application.Commons.Mappers
{
    public static class OccupationMapper
    {
        public static OccupationDto Map(this Occupation source)
        {

            return new OccupationDto
            {
                OccupationId = source.OccupationId,
                OccupationTitle = source.OccupationTitle,
                Khmer = source.Khmer,
                TypeOf = source.TypeOf,
            };
        }


        public static Occupation MapCreate(this OccupationDtoCreate source)
        {
            return new Occupation
            {
                OccupationTitle = source.OccupationTitle!,
                Khmer = source.Khmer,
                TypeOf = source.TypeOf,
            };
        }

        public static Occupation MapEdit(this Occupation source)
        {
            return new Occupation
            {
                OccupationId = source.OccupationId,
                OccupationTitle = source.OccupationTitle,
                Khmer = source.Khmer,
                TypeOf = source.TypeOf,
            };
        }

        public static Occupation MapEdit(this OccupationDto source)
        {
            return new Occupation
            {
                OccupationId = source.OccupationId,
                OccupationTitle = source.OccupationTitle,
                Khmer = source.Khmer,
                TypeOf = source.TypeOf,
            };
        }

    }
}
