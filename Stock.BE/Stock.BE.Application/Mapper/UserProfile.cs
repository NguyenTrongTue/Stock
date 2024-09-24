﻿using AutoMapper;
using ESP.Cloud.BE.Application.Dto;
using ESP.Cloud.BE.Application.Param;
using ESP.Cloud.BE.Core.Model;

namespace ESP.Cloud.BE.Application.Service.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
           
            CreateMap<UserRegisterParam, UserEntity>();
       
            CreateMap<UserEntity, UserDto>();
        }
    }
}

