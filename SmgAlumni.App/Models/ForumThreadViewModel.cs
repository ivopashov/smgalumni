using System.Collections.Generic;

namespace SmgAlumni.App.Models
{
    public class ForumThreadViewModel
    {
        public ForumThreadViewModel()
        {
            Tags = new List<TagDto>();
        }

        public string Heading { get; set; }
        public string Body { get; set; }
        public List<TagDto> Tags{ get; set; }
    }
}