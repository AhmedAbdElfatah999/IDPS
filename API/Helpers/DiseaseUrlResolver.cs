using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;
namespace API.Helpers
{
    public class DiseaseUrlResolver : IValueResolver<Disease, DiseaseToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public DiseaseUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Disease source, DiseaseToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}