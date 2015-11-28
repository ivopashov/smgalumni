using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface IForumCommentsRepository : IRepository<ForumComment>
    {
        IEnumerable<ForumComment> GetCommentsForAnswer(int answerID);
    }
}
