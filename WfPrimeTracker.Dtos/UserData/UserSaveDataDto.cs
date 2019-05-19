using System;
using System.Collections.Generic;
using System.Text;

namespace WfPrimeTracker.Dtos.UserData {
    /// <summary>
    /// Data specific to a user
    /// </summary>
    public class UserSaveDataDto {
        /// <summary>
        /// If the user is anonymous, this is their ID. Otherwise we check the token for its ID
        /// </summary>
        public Guid AnonymousUserId { get; set; }

        /// <summary>
        /// Contains the ID's of the prime items as key and the item state as value
        /// </summary>
        public Dictionary<int, UserPrimeItemSaveDataDto> PrimeItems { get; set; }
    }
}
