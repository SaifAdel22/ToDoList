using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Models;
namespace ToDoList.Configurations
{
    public class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
    {
        public void Configure(EntityTypeBuilder<ToDoTask> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Title)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Description)
                   .HasMaxLength(200);
            builder.Property(t => t.Deadline)
                    .IsRequired()
                    .HasColumnType("datetime2");

            builder.Property(t => t.FilePath)
                   .HasMaxLength(260);

            builder.HasOne(t => t.User)
                   .WithMany(u => u.ToDoTasks)
                   .HasForeignKey(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
