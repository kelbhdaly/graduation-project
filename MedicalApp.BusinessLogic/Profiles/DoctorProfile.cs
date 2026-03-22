namespace MedicalApp.BusinessLogic.Profiles
{
    internal class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<DoctorRegisterDto , Doctor>().ReverseMap();
        }
    }
}
