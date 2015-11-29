namespace SmgAlumni.EF.Models
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        public string SettingKey { get; set; }
        public string SettingName { get; set; }
    }
}
