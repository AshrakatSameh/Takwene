namespace Takwene.Domain.Models
{
    public class Dsp //Digital Service Provider = DSP
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<TrackDistribution> Distributions { get; set; } = new List<TrackDistribution>();
    }
}
