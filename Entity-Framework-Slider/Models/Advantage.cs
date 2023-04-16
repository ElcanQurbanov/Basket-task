namespace Entity_Framework_Slider.Models
{
    public class Advantage: BaseEntity
    {
        public string? Image { get; set; }
        public int AboutId { get; set; }
        public About About { get; set; }
    }
}
