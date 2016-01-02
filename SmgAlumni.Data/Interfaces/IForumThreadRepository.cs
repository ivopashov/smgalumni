using SmgAlumni.EF.Models;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface IForumThreadRepository : IRepository<ForumThread>
    {
        IEnumerable<ForumThread> Page(int skip, int take, bool orderByDescDate = true);
        int GetCount();
    }
}
