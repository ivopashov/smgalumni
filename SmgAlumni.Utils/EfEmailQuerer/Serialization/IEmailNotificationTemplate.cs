namespace SmgAlumni.Utils.EfEmailQuerer.Serialization
{
    public interface IEmailNotificationTemplate
    {
        string Subject { get; }
        string Template { get; }
        object Data { get; }
    }
}
