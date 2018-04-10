using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactManagementSystem26_10_17
{
    public static class MappingInitializer
    {
        public static void InitializeMapping()
        {
            Mapper.Initialize(cfg =>
            {
                //cfg.CreateMap<User, UserDto>();
                //cfg.CreateMap<UserDto, User>();

                //cfg.CreateMap<Item, ItemDto>();
                //cfg.CreateMap<ItemDto, Item>();
            });
        }
    }
}