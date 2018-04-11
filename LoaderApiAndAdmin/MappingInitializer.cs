using AutoMapper;
using LoaderApiAndAdmin.DataBase;
using LoaderAppApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoaderApiAndAdmin
{
    public static class MappingInitializer
    {
        public static void InitializeMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<User, UserDto>();



                //cfg.CreateMap<User, SignInInput>();
                //cfg.CreateMap<SignInInput, User>();

                //cfg.CreateMap<Item, ItemDto>();
                //cfg.CreateMap<ItemDto, Item>();
            });
        }
    }
}