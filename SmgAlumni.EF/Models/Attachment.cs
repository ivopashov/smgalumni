using System;

namespace SmgAlumni.EF.Models
{
    public class Attachment : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public byte[] Data { get; set; }
    }
}
