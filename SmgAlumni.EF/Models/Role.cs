namespace SmgAlumni.EF.Models
{
    public class Role : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual User User { get; set; }
    }
}
