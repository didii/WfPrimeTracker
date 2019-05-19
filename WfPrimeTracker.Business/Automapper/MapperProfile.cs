using AutoMapper;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Domain.Users;
using WfPrimeTracker.Dtos;
using WfPrimeTracker.Dtos.UserData;

namespace WfPrimeTracker.Business.Automapper {
    internal class MapperProfile : Profile {
        public MapperProfile() {
            // From domain to dto
            CreateMap<IngredientsGroup, IngredientsGroupDto>(MemberList.Destination);
            CreateMap<PrimeItem, PrimeItemDto>(MemberList.Destination);
            CreateMap<PrimePart, PrimePartDto>(MemberList.Destination);
            CreateMap<PrimePartIngredient, PrimePartIngredientDto>(MemberList.Destination);
            CreateMap<Relic, RelicDto>(MemberList.Destination);
            CreateMap<RelicDrop, RelicDropDto>(MemberList.Destination);
            CreateMap<Resource, ResourceDto>(MemberList.Destination);
            CreateMap<ResourceIngredient, ResourceIngredientDto>(MemberList.Destination);

            CreateMap<User, UserSaveDataDto>().ConvertUsing<User_UserSaveDataDto>();
        }
    }
}