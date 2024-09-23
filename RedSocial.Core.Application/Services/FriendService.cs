using AutoMapper;
using Microsoft.AspNetCore.Http;
using RedSocial.Core.Application.Dtos.Account;
using RedSocial.Core.Application.Helpers;
using RedSocial.Core.Application.Interfaces.Repositories;
using RedSocial.Core.Application.Interfaces.Services;
using RedSocial.Core.Application.ViewModels.Friend;
using RedSocial.Core.Domain.Entity;


namespace RedSocial.Core.Application.Services
{
    
    public class FriendService : GenericService<SaveFriendViewModel, FriendViewModel, Friend>, IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public FriendService(IFriendRepository friendRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IAccountService accountService) : base(friendRepository, mapper)
        {
            _friendRepository = friendRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _accountService = accountService;
        }

        public async Task<SaveFriendViewModel> Add(SaveFriendViewModel vm)
        {
            vm.User_Id1 = userViewModel != null ? userViewModel.Id : vm.User_Id1;

            var friendUserId = await _accountService.GetUserIdByUsernameAsync(vm.FriendUser);
            vm.User_Id2 = friendUserId;

            var friendEntity = _mapper.Map<Friend>(vm);
            var result = await _friendRepository.AddAsync(friendEntity);

            return _mapper.Map<SaveFriendViewModel>(result);
        }

        public async Task<List<FriendViewModel>> GetAllViewModelWithInclude()
        {
            var friendList = await _friendRepository.GetAllAsync();

            var userId = userViewModel?.Id; // add null conditional operator

            if (userId == null)
            {
                // return an empty list or handle the null case differently
                return new List<FriendViewModel>();
            }

            var friendViewModels = new List<FriendViewModel>();

            foreach (var friend in friendList.Where(f => f.User_Id1 == userId || f.User_Id2 == userId))
            {
                var friendId = friend.User_Id1 == userId ? friend.User_Id2 : friend.User_Id1;

                var friendUser = await _accountService.GetUserByIdAsync(friendId);

                var friendViewModel = new FriendViewModel
                {
                    Id = friend.Id,
                    ProfilePictureUrl = friendUser.ProfilePictureUrl,
                    UserName = friendUser.UserName,
                    FirstName = friendUser.FirstName,
                    LastName = friendUser.LastName
                };

                friendViewModels.Add(friendViewModel);
            }

            return friendViewModels;
        }





    }
}
