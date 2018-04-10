using AutoMapper;
using LoaderApiAndAdmin.DataBase;
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
                cfg.CreateMap<User, User>();

                //cfg.CreateMap<Item, ItemDto>();
                //cfg.CreateMap<ItemDto, Item>();
            });
        }
    }
}