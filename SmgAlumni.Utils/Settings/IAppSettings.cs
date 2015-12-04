namespace SmgAlumni.Utils.Settings
{
    public interface IAppSettings
    {
        AuthenticationSettings Authentication { get; }
        EmailSettings Email { get; }
        MessagingSettings Messaging { get; }
        MailgunSettings MailgunSettings { get; }
        NewsLetterSettings NewsLetterSettings { get; }
    }
}