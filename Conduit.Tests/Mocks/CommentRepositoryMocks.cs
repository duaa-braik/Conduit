using Conduit.Domain.Entities;
using Conduit.Domain.Interfaces;
using Moq;

namespace Conduit.Tests.Mocks
{
    public class CommentRepositoryMocks : FakeDatabase
    {
        public Mock<ICommentRepository> CommentRepositoryMock { get; set; }

        public CommentRepositoryMocks()
        {
            CommentRepositoryMock = new Mock<ICommentRepository>();

            CommentRepositoryMock.Setup(x => x.GetCommentById(It.IsAny<int>()))
                .ReturnsAsync((int id) => comments.First(c => c.CommentId == id));

            CommentRepositoryMock.Setup(x => x.GetComments(It.IsAny<Article>()))
                .ReturnsAsync((Article article) => article.Comments.ToList());
        }
    }
}
