namespace SmgAlumni.Utils
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        void Fatal(string message);
    }
}
