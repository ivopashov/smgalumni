using System.Collections.Generic;

namespace SmgAlumni.App.Models
{
    public class ListingUpdateVm
    {
        public ListingUpdateVm()
        {
            Attachments = new List<AttachmentViewModel>();
        }

        public int Id { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
        public List<AttachmentViewModel> Attachments{ get; set; }
    }
}