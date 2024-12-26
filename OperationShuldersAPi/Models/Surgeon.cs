namespace OperationShuldersAPi.Models
{
    public class Surgeon
    {
        public int SurgeonId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int? SpecializationId { get; set; }
        public string ContactInfo { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public Users User { get; set; }
        public Specialization Specialization { get; set; }
        public ICollection<OperationSchedule> OperationSchedules { get; set; }
    }

}
