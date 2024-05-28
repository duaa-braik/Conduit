using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Moq;

namespace Conduit.Tests.Mocks
{
    public class TagRepositoryMocks : FakeDatabase
    {
        public Mock<ITagRepository> TagRepositoryMock { get; set; }

        List<Tag> tags;

        public TagRepositoryMocks()
        {
            TagRepositoryMock = new Mock<ITagRepository>();

            tags = new List<Tag>
            {
                new Tag { TagId = 1, TagName = "lorem"},
                new Tag { TagId = 2, TagName = "ipsum"}
            };

            TagRepositoryMock.Setup(x => x.CreateTag(It.Is<Tag>(tag => !tags.Select(t => t.TagName).Contains(tag.TagName))))
                .ReturnsAsync((Tag tag) => tag);

            TagRepositoryMock.Setup(x => x.GetTag(It.IsAny<string>()))
                .ReturnsAsync((string tag) => tags.First(t => t.TagName == tag));
        }
    }
}
