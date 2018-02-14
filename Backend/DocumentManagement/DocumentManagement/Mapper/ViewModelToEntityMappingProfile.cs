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
            CreateMap<RegisterViewModel, Account>()
                .ForMember(usr => usr.FirstName, map => map.MapFrom(vm => vm.FirstName))
                .ForMember(usr => usr.LastName, map => map.MapFrom(vm => vm.LastName))
                .ForMember(usr => usr.Age, map => map.MapFrom(vm => vm.Age))
                .ForMember(usr => usr.Registerd, map => map.MapFrom(vm => vm.Registerd));


            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(au => au.UserName, map => map.MapFrom(us => us.UserName));

            CreateMap<CreateDocumentViewModel, Document>()
                .ForMember(doc => doc.Descripion, map => map.MapFrom(vm => vm.Description));

            CreateMap<Document, DocumentViewModel>()
                .ForMember(vm => vm.Id, map => map.MapFrom(doc => doc.Id))
                .ForMember(vm => vm.Name, map => map.MapFrom(doc => doc.Name.Remove(0, doc.Name.IndexOf('_')+1)))
                .ForMember(vm => vm.Created, map => map.MapFrom(doc => doc.Created))
                .ForMember(vm => vm.FileSize, map => map.MapFrom(doc => doc.FileSize))
                .ForMember(vm => vm.Description, map => map.MapFrom(doc => doc.Descripion));

        }
    }
}