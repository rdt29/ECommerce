using AutoMapper;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap <source(entity) , destination(dto)> ().ReverseMAp();
            //CreateMap<RoleDTO, Roles>().ReverseMap();
            //? -----------------------------------Role----------------------------------------
            CreateMap<RoleDTO, Roles>()
             .ForMember(dest => dest.users, opt => opt.Ignore()).ReverseMap();

            //?--------------------------------Products-----------------------------------------
            CreateMap<ProductDTO, Products>()
                .ForMember(dest => dest.OrderDetail, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ProductResponceDTO, Products>()
               .ForMember(dest => dest.OrderDetail, opt => opt.Ignore())
               .ReverseMap();

            //?--------------------------------User-----------------------------------------
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.products, opt => opt.Ignore())
                .ForMember(dest => dest.OrdersTables, opt => opt.Ignore())
                    .ReverseMap();
        }
    }
}