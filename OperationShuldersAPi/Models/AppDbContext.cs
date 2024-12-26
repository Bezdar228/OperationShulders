using Microsoft.EntityFrameworkCore;

namespace OperationShuldersAPi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Surgeon> Surgeons { get; set; }
        public DbSet<OperationSchedule> OperationSchedules { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OperatingRoom> OperatingRooms { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка уникальных ограничений и связей

            // Для User (пользователь) - добавление уникальности по Username
            modelBuilder.Entity<Users>()
                .HasKey(u => u.User_Id);  // Первичный ключ для User

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Для Surgeon (хирург) - добавление связи с User и Specialization
            modelBuilder.Entity<Surgeon>()
                .HasKey(s => s.SurgeonId);  // Первичный ключ для Surgeon

            modelBuilder.Entity<Surgeon>()
                .HasOne(s => s.User)
                .WithOne(u => u.Surgeon)
                .HasForeignKey<Surgeon>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Surgeon>()
                .HasOne(s => s.Specialization)
                .WithMany(sp => sp.Surgeons)
                .HasForeignKey(s => s.SpecializationId);

            // Для OperationSchedule (расписание операции) - добавление связи с Surgeon и OperatingRoom
            modelBuilder.Entity<OperationSchedule>()
                .HasKey(os => os.Operation_Id);  // Первичный ключ для OperationSchedule

            modelBuilder.Entity<OperationSchedule>()
                .HasOne(os => os.Surgeon)
                .WithMany(s => s.OperationSchedules)
                .HasForeignKey(os => os.SurgeonId);

            modelBuilder.Entity<OperationSchedule>()
                .HasOne(os => os.OperatingRoom)
                .WithMany(or => or.OperationSchedules)
                .HasForeignKey(os => os.OperatingRoomId);

            // Для Role (роли) - добавление первичного ключа
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);  // Первичный ключ для Role

            // Для OperatingRoom (операционная) - добавление первичного ключа
            modelBuilder.Entity<OperatingRoom>()
                .HasKey(or => or.RoomId);  // Первичный ключ для OperatingRoom

            // Для Specialization (специализация) - добавление первичного ключа
            modelBuilder.Entity<Specialization>()
                .HasKey(sp => sp.SpecializationId);  // Первичный ключ для Specialization
        }
    }
}
