namespace SmgAlumni.ServiceLayer.Interfaces
{
    public interface INewsLetterService
    {
        void Unsubscribe(string token, string username);
    }
}
