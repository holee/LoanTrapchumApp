using Applications.Responses.ViewModels;
using Domains.Models.Lists;
using Applications.Features.Settings.Requests;


namespace Applications.Commons.Mappers
{
    public static class OccupationMapper
    {
        public static OccupationVm Map(this Occupation source)
        {

            return new OccupationVm
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

        public static Occupation MapEdit(this OccupationVm source)
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
