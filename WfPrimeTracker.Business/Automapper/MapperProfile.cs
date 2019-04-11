using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Dtos;

namespace WfPrimeTracker.Business.Automapper {
    internal class MapperProfile : Profile {
        public MapperProfile() {
            // From domain to dto
            CreateMap<PrimeItem, PrimeItemDto>(MemberList.Destination);
            CreateMap<Ingredient, IngredientDto>(MemberList.Destination);
            CreateMap<PrimePart, PrimePartDto>(MemberList.Destination);
            CreateMap<RelicDrop, RelicDropDto>(MemberList.Destination);
            CreateMap<Relic, RelicDto>(MemberList.Destination);
        }
    }
}
