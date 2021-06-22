using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Commands;
using AutoMapper;
using Infra.Models;
using Microsoft.AspNetCore.Http;

namespace Application.AuthoMapper
{
    public class AuthoMapperProfile : Profile
    {

        public AuthoMapperProfile()
        {

           

            CreateMap<RegisterCommand, ApplicationUser>()
                    .ForMember(a => a.UserName, a => a.MapFrom(b => b.Email));

            CreateMap<RegisterAdminCommand, ApplicationUser>()
                   .ForMember(a => a.UserName, a => a.MapFrom(b => b.Email));

            CreateMap<AddCategoryCommand, Category>();

            CreateMap<AddTaskCommand, UserTask>()
                .ForMember(a => a.Status, a => a.MapFrom(src => "ToDo"))
                .ForMember(a => a.Date, a => a.MapFrom(src => DateTime.Now));
             

        }



    }

}

