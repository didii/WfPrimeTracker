using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WfPrimeTracker.Data;
using WfPrimeTracker.Domain.Users;
using WfPrimeTracker.Dtos.UserData;
using WfPrimeTracker.Infrastructure;

namespace WfPrimeTracker.Business.Services {
    internal class UserService : IUserService {
        private readonly PrimeContext _context;
        private readonly IUserData _userData;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public UserService(PrimeContext context, IUserData userData, IMapper mapper) {
            _context = context;
            _userData = userData;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<UserSaveDataDto> GetUserData() {
            // Setup includes
            var baseQuery = _context.Users
                                    .Include(u => u.UserPrimeItemSaveData)
                                    .Include(u => u.UserPrimePartIngredientSaveData)
                                    .ThenInclude(p => p.PrimePartIngredient);
            // Get the user based on user data
            User user;
            if (_userData.IsAnonymous) {
                user = await baseQuery.FirstOrDefaultAsync(u => u.AnonymousId == _userData.AnonymousId);
            } else {
                user = await baseQuery.FirstOrDefaultAsync(u => u.Email == _userData.Email);
            }

            // Map and return
            var result = _mapper.Map<UserSaveDataDto>(user);
            return result;
        }

        /// <inheritdoc />
        public async Task SaveUserData(UserSaveDataDto data) {
            // Get the user
            User user;
            var baseQuery = _context.Users.Include(u => u.UserPrimeItemSaveData).Include(u => u.UserPrimePartIngredientSaveData);
            if (_userData.IsAnonymous) {
                user = await baseQuery.FirstOrDefaultAsync(u => u.AnonymousId == _userData.AnonymousId);
                if (user == null) {
                    user = new User() {
                        AnonymousId = _userData.AnonymousId,
                        UserPrimeItemSaveData = new List<UserPrimeItemSaveData>(),
                        UserPrimePartIngredientSaveData = new List<UserPrimePartIngredientSaveData>(),
                    };
                    _context.Users.Add(user);
                }
            } else {
                user = await baseQuery.FirstOrDefaultAsync(u => u.Email == _userData.Email);
            }

            // Clear all user data
            user.UserPrimeItemSaveData.Clear();
            user.UserPrimePartIngredientSaveData.Clear();

            // Fill 'er up again
            foreach (var primeItem in data.PrimeItems) {
                if (primeItem.Value.IsChecked) {
                    user.UserPrimeItemSaveData.Add(new UserPrimeItemSaveData() {
                        UserId = user.Id,
                        PrimeItemId = primeItem.Key,
                        IsChecked = primeItem.Value.IsChecked,
                    });
                }
                foreach (var primePart in primeItem.Value.PrimePartIngredients) {
                    if (primePart.Value.CheckedCount > 0) {
                        user.UserPrimePartIngredientSaveData.Add(new UserPrimePartIngredientSaveData() {
                            UserId = user.Id,
                            PrimePartIngredientId = primePart.Key,
                            CheckedCount = primePart.Value.CheckedCount,
                        });
                    }
                }
            }

            // Save
            await _context.SaveChangesAsync();
        }
    }
}
