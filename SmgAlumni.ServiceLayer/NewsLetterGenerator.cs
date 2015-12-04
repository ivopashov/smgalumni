using SmgAlumni.Data.Interfaces;
using SmgAlumni.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmgAlumni.ServiceLayer
{
    public class NewsLetterGenerator : INewsLetterGenerator
    {
        private readonly INewsLetterCandidateRepository _newsLetterRepository;

        public NewsLetterGenerator(INewsLetterCandidateRepository newsLetterRepository)
        {
            _newsLetterRepository = newsLetterRepository;
        }

        public string GenerateNewsLetter()
        {
            throw new NotImplementedException();
        }
    }
}
