namespace OperationShuldersAPi.Models
{
    public class OperatingRoom
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int Capacity { get; set; }
        public string EquipmentDetails { get; set; }

        // Навигационное свойство
        public ICollection<OperationSchedule> OperationSchedules { get; set; }
    }

}
