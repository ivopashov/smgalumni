using SmgAlumni.EF.Models;
using System;
using System.Collections.Generic;

namespace SmgAlumni.Data.Interfaces
{
    public interface IAttachmentRepository : IRepository<Attachment>
    {
        IEnumerable<Attachment> AttachmentsWithoutParentAndAtLeastOneHourOld();
        Attachment FindByTempKey(Guid guid);
    }
}
