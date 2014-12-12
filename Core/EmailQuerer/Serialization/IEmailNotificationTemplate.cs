namespace Core.EmailQuerer.Serialization
{
    public interface IEmailNotificationTemplate
    {
        string Subject { get; }
        string Template { get; }
        object Data { get; }
    }
}
