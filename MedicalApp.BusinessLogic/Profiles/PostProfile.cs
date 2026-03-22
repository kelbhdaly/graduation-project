namespace MedicalApp.BusinessLogic.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDto>().ForMember(dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor.ApplicationUser.UserName))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.PostImages));

            CreateMap<PostImage, PostImageDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<PostImageUrlResolver>());
        }
    }
}
