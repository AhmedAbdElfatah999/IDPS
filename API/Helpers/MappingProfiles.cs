using API.Dtos;
using AutoMapper;
using Core.Entities;
using System;

namespace API.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<Disease, DiseaseToReturnDto>()
                .ForMember(d => d.Specialization, o => o.MapFrom(s => s.Specialization.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<DiseaseUrlResolver>());

            CreateMap<Message,MessageForCreationDto>().ReverseMap();

             CreateMap<Message,MessageToReturnDto>()
             .ForMember(m => m.DoctorPhotoUrl, opt => opt
                    .MapFrom(u => u.Doctor.PictureUrl))

                .ForMember(m => m.SenderId, opt => opt
                    .MapFrom(u => u.SenderId  )) 

                .ForMember(m => m.DoctorName, opt => opt
                    .MapFrom(u =>u.Doctor.Name)) 

                .ForMember(m => m.RecipientId, opt => opt
                    .MapFrom(u =>u.ReceieverId )) 

                 .ForMember(m => m.PatientName, opt => opt
                    .MapFrom(u =>u.Patient.Name)) 

                 .ForMember(m => m.PatientPhotoUrl, opt => opt
                    .MapFrom(u =>u.Patient.PictureUrl))  

                 .ForMember(m => m.PatientId, opt => opt
                    .MapFrom(u =>u.Patient.Id)) 

                 .ForMember(m => m.DoctorId, opt => opt
                    .MapFrom(u =>u.Doctor.Id)) 

                .ForMember(m => m.Content, opt => opt
                    .MapFrom(u =>u.Content))        
                 .ForMember(m => m.IsRead, opt => opt
                    .MapFrom(u => u.IsRead)) 
                .ForMember(m => m.DateRead, opt => opt
                    .MapFrom(u => u.DateRead))  
                .ForMember(m => m.MessageSent, opt => opt
                    .MapFrom(u => u.MessageSent));
                                              
        }
        
    }
}