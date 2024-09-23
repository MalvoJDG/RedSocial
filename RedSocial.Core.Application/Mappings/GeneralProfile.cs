using RedSocial.Core.Application.ViewModels.Users;
using RedSocial.Core.Domain.Entity;
using AutoMapper;
using RedSocial.Core.Application.Dtos.Account;
using RedSocial.Core.Application.ViewModels.Post;
using RedSocial.Core.Application.ViewModels.Friend;

namespace RedSocial.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region Athentication Request
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError , opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            #endregion

            #region Register Request
            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.File, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region Forgotviewmodel
            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region reset password
            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region PostModel
            CreateMap<PostViewModel, SavePostViewModel>()
               .ForMember(x => x.File, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(x => x.UserName, opt => opt.Ignore())
               .ForMember(x => x.UserPhoto, opt => opt.Ignore())
               .ForMember(x => x.Comments, opt => opt.Ignore());

            CreateMap<Post, SavePostViewModel>()
               .ForMember(x => x.File, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(x => x.Comments, opt => opt.Ignore());

            CreateMap<Post, PostViewModel>()
               .ForMember(x => x.UserPhoto, opt => opt.Ignore())
               .ForMember(x => x.UserName, opt => opt.Ignore())
               .ReverseMap();
            #endregion

            #region Friend
            CreateMap<Friend, FriendViewModel>()
               .ForMember(x => x.UserName, opt => opt.Ignore())
               .ForMember(x => x.FirstName, opt => opt.Ignore())
               .ForMember(x => x.LastName, opt => opt.Ignore())
               .ForMember(x => x.ProfilePictureUrl, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<Friend, SaveFriendViewModel>()
               .ForMember(x => x.FriendUser, opt => opt.Ignore())
               .ReverseMap();

            #endregion


        }

    }
}
