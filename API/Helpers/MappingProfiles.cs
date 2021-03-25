using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Disease, DiseaseToReturnDto>()
                .ForMember(d => d.Specialization, o => o.MapFrom(s => s.Specialization.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<DiseaseUrlResolver>());
         
        }
        
    }
}