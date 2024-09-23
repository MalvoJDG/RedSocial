using AutoMapper;
using Microsoft.AspNetCore.Http;
using RedSocial.Core.Application.Dtos.Account;
using RedSocial.Core.Application.Helpers;
using RedSocial.Core.Application.Interfaces.Repositories;
using RedSocial.Core.Application.Interfaces.Services;
using RedSocial.Core.Application.ViewModels.Post;
using RedSocial.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSocial.Core.Application.Services
{
    
    public class PostService : GenericService<SavePostViewModel, PostViewModel, Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IFriendRepository _FriendRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse userViewModel;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public PostService(IPostRepository postRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IAccountService accountService, IFriendRepository friendRepository) : base(postRepository, mapper)
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _accountService = accountService;
            _FriendRepository = friendRepository;
        }

        public override async Task<SavePostViewModel> Add(SavePostViewModel vm)
        {
            vm.PostDate = DateTime.Now;
            vm.User_Id = userViewModel != null ? userViewModel.Id : vm.User_Id;
            return await base.Add(vm);
        }

        public override async Task Update(SavePostViewModel vm, int id)
        {
            var existingPost = await _postRepository.GetByIdAsync(id);

            vm.PostDate = existingPost.PostDate;
            vm.User_Id = userViewModel != null ? userViewModel.Id : vm.User_Id;
            await base.Update(vm, id);
        }

        public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
        {
            var postList = await _postRepository.GetAllAsync();

            if (userViewModel != null)
            {
                postList = postList
                .Where(post => post.User_Id == userViewModel.Id)
                .OrderByDescending(post => post.PostDate).ToList();

                return postList.Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Body = post.Body,
                    PostDate = post.PostDate,
                    Archive = post.Archive,
                    PublicationType = post.PublicationType,
                    UserName = userViewModel.UserName,
                    UserPhoto = userViewModel.ProfilePictureUrl
                }).ToList();
            }
            // Obtener el ID del usuario del contexto de Identity
            return postList.Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Body = post.Body,
                    PostDate = post.PostDate,
                    Archive = post.Archive,
                    PublicationType = post.PublicationType,
                    UserName = "Fulano",
                    UserPhoto = "C:\\Users\\sylcr\\OneDrive\\Escritorio\\imagen\\orden\\apanelada.jpg"
            }).ToList();
        }

        public async Task<List<PostViewModel>> GetAllFriendsPosts()
        {
            var friendList = await _FriendRepository.GetAllAsync();
            var userId = userViewModel.Id;

            var friendIds = friendList
                .Where(friend => friend.User_Id1 == userId || friend.User_Id2 == userId)
                .Select(friend => friend.User_Id1 == userId ? friend.User_Id2 : friend.User_Id1)
                .ToList();

            var friendsPosts = await _postRepository.GetAllAsync();
            var posts = friendsPosts.Where(post => friendIds.Contains(post.User_Id)).OrderByDescending(post => post.PostDate).ToList();

            var userIds = posts.Select(post => post.User_Id).Distinct().ToList();
            var users = await _accountService.GetUsersByIdsAsync(userIds);

            var result = posts.Select(post =>
            {
                var user = users.FirstOrDefault(u => u.Id == post.User_Id);
                return new PostViewModel
                {
                    Id = post.Id,
                    User_Id = post.User_Id,
                    Body = post.Body,
                    PostDate = post.PostDate,
                    Archive = post.Archive,
                    PublicationType = post.PublicationType,
                    UserName = user.UserName,
                    UserPhoto = user.ProfilePictureUrl
                };
            }).ToList();

            return result;
        }





    }
}
