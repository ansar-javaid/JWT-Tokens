using AutoMapper;
using JWT_Practice.Models;

namespace JWT_Practice.Mapper
{
    public class AutoMap: Profile
    {
        public AutoMap()
        {
            CreateMap<AppUser, UserView>();
        }
    }
}
