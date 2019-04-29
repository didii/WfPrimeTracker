using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WfPrimeTracker.Domain.Contracts;

namespace WfPrimeTracker.Domain.Users {
    public class UserPrimePartIngredientSaveData : ITrackable {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(PrimePart))]
        public int PrimePartIngredientId { get; set; }

        public PrimePartIngredient PrimePartIngredient { get; set; }

        public int CheckedCount { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }
    }
}
