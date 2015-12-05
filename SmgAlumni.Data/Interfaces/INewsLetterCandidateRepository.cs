using SmgAlumni.EF.Models;
using SmgAlumni.EF.Models.enums;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface INewsLetterCandidateRepository : IRepository<NewsLetterCandidate>
    {
        IEnumerable<NewsLetterCandidate> GetUnsent();
        IEnumerable<NewsLetterCandidate> GetUnsentOfType(NewsLetterItemType type);
        IEnumerable<NewsLetterCandidate> GetOfType(NewsLetterItemType type);
    }
}
