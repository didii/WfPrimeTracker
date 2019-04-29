using System;
using System.ComponentModel.DataAnnotations.Schema;
using WfPrimeTracker.Domain.Contracts;

namespace WfPrimeTracker.Domain.Users {
    public class UserPrimeItemSaveData : ITrackable {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(PrimeItem))]
        public int PrimeItemId { get; set; }

        public PrimeItem PrimeItem { get; set; }

        public bool IsChecked { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }
    }
}
