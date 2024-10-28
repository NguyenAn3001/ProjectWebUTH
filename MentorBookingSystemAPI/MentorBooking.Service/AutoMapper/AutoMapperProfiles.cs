﻿using AutoMapper;
using MentorBooking.Repository.Entities;
using MentorBooking.Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.AutoMapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Skill, string>().ConvertUsing(source => source.Name);
            CreateMap<Mentor, MentorSearchingResponse>()
                .ForMember(des => des.FirstName, act => act.MapFrom(src => src.User.FirstName))
                .ForMember(des => des.LastName, act => act.MapFrom(src => src.User.LastName))
                .ForMember(des => des.Image, act => act.MapFrom(src => src.User.Image))
                .ForMember(des => des.SkillName, act => act.MapFrom(src => src.Skills));
        }
    }
}
