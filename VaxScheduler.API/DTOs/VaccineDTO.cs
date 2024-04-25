using VaxScheduler.Core.Errors;

namespace VaxScheduler.API.DTOs
{
    public class VaccineDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DurationBetweenDoses { get; set; }

        public string Precautions { get; set; }

        public StatuseOfResonse Status { get; set; }

    }
}
