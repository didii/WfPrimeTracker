using System;
using System.Collections.Generic;
using System.Text;

namespace WfPrimeTracker.Dtos.UserData {
    /// <summary>
    /// Data specific to a user
    /// </summary>
    public class UserSaveDataDto {
        /// <summary>
        /// Contains the ID's of the prime items as key and the item state as value
        /// </summary>
        public Dictionary<int, UserPrimeItemSaveDataDto> PrimeItems { get; set; }

        /// <summary>
        /// Last save date
        /// </summary>
        public DateTimeOffset ModifiedOn { get; set; }
    }
}
