using AutoMapper;
using Conduit.Domain.DTOs;
using Conduit.Domain.Entities;

namespace Conduit.Domain.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleCreationDto, Article>()
                .ForMember(a => a.DatePublished, o => o.MapFrom(a => DateTime.UtcNow))
                .ForMember(a => a.LastModified, o => o.MapFrom(a => DateTime.UtcNow))
                .ForMember(a => a.Slug, o => o.MapFrom(a => a.Title.Trim().Replace(" ", "-")));
            CreateMap<Article, ArticleDto>()
                .ForMember(a => a.UserProfile, o => o.MapFrom(a => a.User))
                .ForMember(a => a.TagList, o => o.MapFrom(a => a.Tags.Select(t => t.TagName)));
        }
    }
}
