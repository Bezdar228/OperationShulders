using System.ComponentModel.DataAnnotations;

namespace OperationShuldersAPi.Models
{
    public class OperationSchedule
    {
        public int Operation_Id { get; set; }
        public int SurgeonId { get; set; }
        public DateTime OperationDate { get; set; }
        public TimeSpan StartTime { get; set; } 
        public TimeSpan? EndTime { get; set; }
        public int OperatingRoomId { get; set; }
        public string Status { get; set; } = "Запланировано";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public Surgeon Surgeon { get; set; }
        public OperatingRoom OperatingRoom { get; set; }
    }

}
