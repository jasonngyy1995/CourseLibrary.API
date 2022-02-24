﻿using AutoMapper;

namespace CourseLibrary.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            // <Source, Destination type>
            CreateMap<Entities.Course, Models.CourseDto>();
        }
    }
}