using SmgAlumni.ServiceLayer.Models;

namespace SmgAlumni.ServiceLayer.Interfaces
{
    public interface INewsLetterGenerator
    {
        string GenerateNewsLetter(BiMonthlyNewsLetterDto newsLetterModel);
    }
}
