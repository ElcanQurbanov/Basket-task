namespace Entity_Framework_Slider.Models
{
    public class About: BaseEntity
    {
        public string? Header { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public ICollection<Advantage> Advantages { get; set; }
    }
}
