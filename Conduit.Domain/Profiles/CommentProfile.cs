using AutoMapper;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;

namespace Conduit.Domain.Profiles
{
    public class CommentProfile :Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentCreationDto, Comment>()
                .ForMember(c => c.CreatedAt, o => o.MapFrom(a => DateTime.UtcNow));
            CreateMap<Comment, CommentDto>()
                .ForMember(c => c.UserProfile, o => o.MapFrom(c => c.User));
        }
    }
}
