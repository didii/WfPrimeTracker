using System.Collections.Generic;

namespace WfPrimeTracker.Dtos.UserData {
    public class UserPrimeItemSaveDataDto {
        public bool IsChecked { get; set; }
        public Dictionary<int, UserPrimePartSaveDataDto> PrimePartIngredients { get; set; }
    }
}
