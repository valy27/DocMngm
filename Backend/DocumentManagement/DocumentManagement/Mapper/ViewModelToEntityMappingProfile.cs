using AutoMapper;
using DocumentManagement.Repository.Models;
using DocumentManagement.Repository.Models.Identity;
using DocumentManagement.ViewModels;

namespace DocumentManagement.Mapper
{
  public class ViewModelToEntityMappingProfile : Profile
  {
    public ViewModelToEntityMappingProfile()
    {
      CreateMap<RegisterViewModel, User>().ForMember(usr => usr.FirstName, map => map.MapFrom(vm => vm.FirstName))
        .ForMember(usr => usr.LastName, map => map.MapFrom(vm => vm.LastName));


      CreateMap<RegisterViewModel, ApplicationUser>()
        .ForMember(au => au.UserName, map => map.MapFrom(us => us.UserName));
    }
  }
}