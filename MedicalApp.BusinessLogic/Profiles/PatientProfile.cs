namespace MedicalApp.BusinessLogic.Profiles
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<PatientDto , Patient>() .ReverseMap();
        }
    }
}
