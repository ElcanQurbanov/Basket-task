namespace Entity_Framework_Slider.Models
{
    public class Blog:BaseEntity
    {
        public string? Header { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTime date { get; set; }
    }
}
