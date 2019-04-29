using System;
using System.Collections.Generic;
using WfPrimeTracker.Domain.Contracts;

namespace WfPrimeTracker.Domain.Users {
    public class User : ITrackable {
        public int Id { get; set; }

        public Guid AnonymousId { get; set; }
        
        public string Email { get; set; }
        
        public DateTimeOffset CreatedOn { get; set; }
        
        public DateTimeOffset ModifiedOn { get; set; }

        public virtual ICollection<UserPrimeItemSaveData> UserPrimeItemSaveData { get; set; }

        public virtual ICollection<UserPrimePartIngredientSaveData> UserPrimePartIngredientSaveData { get; set; }
    }
}
