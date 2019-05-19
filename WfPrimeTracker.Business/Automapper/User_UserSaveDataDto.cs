using AutoMapper;
using System.Collections.Generic;
using WfPrimeTracker.Domain.Users;
using WfPrimeTracker.Dtos.UserData;

namespace WfPrimeTracker.Business.Automapper {
    internal class User_UserSaveDataDto : ITypeConverter<User, UserSaveDataDto> {
        public UserSaveDataDto Convert(User source, UserSaveDataDto destination, ResolutionContext context) {
            if (source == null)
                return null;
            if (destination == null)
                destination = new UserSaveDataDto();

            destination.PrimeItems = new Dictionary<int, UserPrimeItemSaveDataDto>();
            foreach (var userPrimeItem in source.UserPrimeItemSaveData) {
                destination.PrimeItems.Add(userPrimeItem.PrimeItemId, new UserPrimeItemSaveDataDto() {
                    PrimePartIngredients = new Dictionary<int, UserPrimePartSaveDataDto>(),
                    IsChecked = userPrimeItem.IsChecked,
                });
            }
            foreach (var userPrimePart in source.UserPrimePartIngredientSaveData) {
                if (!destination.PrimeItems.TryGetValue(userPrimePart.PrimePartIngredient.PrimeItemId, out var ppi)) {
                    ppi = new UserPrimeItemSaveDataDto() {
                        PrimePartIngredients = new Dictionary<int, UserPrimePartSaveDataDto>(),
                        IsChecked = false,
                    };
                    destination.PrimeItems.Add(userPrimePart.PrimePartIngredient.PrimeItemId, ppi);
                }
                ppi.PrimePartIngredients.Add(userPrimePart.PrimePartIngredientId, new UserPrimePartSaveDataDto() {
                    CheckedCount = userPrimePart.CheckedCount,
                });
            }

            return destination;
        }
    }
}
