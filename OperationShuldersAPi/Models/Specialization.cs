namespace OperationShuldersAPi.Models
{
    public class Specialization
    {
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }

        // Навигационное свойство
        public ICollection<Surgeon> Surgeons { get; set; }
    }

}
