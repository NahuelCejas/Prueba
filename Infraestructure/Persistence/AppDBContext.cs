using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Entities.TaskStatus> Taskstatus { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<InteractionType> InteractionTypes { get; set; }
        public DbSet<Domain.Entities.Interaction> Interactions { get; set; }
        public DbSet<CampaignType> CampaignTypes { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserID);

                entity.Property(u => u.Name)
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            }
            );

            modelBuilder.Entity<Domain.Entities.TaskStatus>(entity =>
            {
                entity.ToTable("TaskStatus");
                entity.HasKey(ts => ts.Id);

                entity.Property(ts => ts.Name)
                .HasColumnType("varchar(25)")
                .IsRequired();

            }
            );

            modelBuilder.Entity<InteractionType>(entity =>
            {
                entity.ToTable("InteractionTypes");
                entity.HasKey(it => it.Id);

                entity.Property(ts => ts.Name)
                .HasMaxLength(25)
                .IsRequired();
            }
            );

            modelBuilder.Entity<CampaignType>(entity =>
            {
                entity.ToTable("CampaignTypes");
                entity.HasKey(ct => ct.Id);

                entity.Property(ct => ct.Name)
                .HasColumnType("varchar(25)")
                .IsRequired();
            }
            );

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                entity.HasKey(c => c.ClientID);
                entity.Property(c => c.ClientID).ValueGeneratedOnAdd();

                entity.Property(c => c.Name)
                .HasColumnType("varchar(255)")
                .IsRequired();

                entity.Property(c => c.Email)
                .HasColumnType("varchar(255)")
                .IsRequired();

                entity.Property(c => c.Phone)
                .HasColumnType("varchar(255)")
                .IsRequired();

                entity.Property(c => c.Company)
                .HasColumnType("varchar(100)")
                .IsRequired();

                entity.Property(c => c.Address)
                .HasColumnType("varchar(MAX)")
                .IsRequired();

                entity.Property(p => p.CreateDate)
               .IsRequired();
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");
                entity.HasKey(p => p.ProjectID);
                entity.Property(p => p.ProjectID).ValueGeneratedOnAdd();


                entity.Property(p => p.ProjectName)
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(p => p.StartDate)
                .IsRequired();

                entity.Property(p => p.EndDate)
                .IsRequired();

                entity.Property(p => p.CreateDate)
               .IsRequired();


                entity.HasOne(p => p.CampaignTypes)
               .WithMany(ct => ct.ListProjects)
               .HasForeignKey(p => p.CampaignType);

                entity.HasOne(p => p.Clients)
                .WithMany(c => c.ListProjects)
                .HasForeignKey(p => p.ClientID);


            }
            );

            modelBuilder.Entity<Domain.Entities.Task>(entity =>
            {
                entity.ToTable("Tasks");
                entity.HasKey(t => t.TaskID);
                entity.Property(t => t.TaskID).ValueGeneratedOnAdd();


                entity.Property(t => t.Name)
                .HasMaxLength(int.MaxValue)
                .IsRequired();
                
                entity.Property(t => t.CreateDate)
                .IsRequired();

                entity.Property(t => t.DueDate)
                .IsRequired();


                entity.HasOne(j => j.Users)
                .WithMany(u => u.ListTasks)
                .HasForeignKey(j => j.AssignedTo);

                entity.HasOne(j => j.Projects)
               .WithMany(p => p.ListTasks)
               .HasForeignKey(j => j.ProjectID);

                entity.HasOne(j => j.TaskStatus)
                .WithMany(ts => ts.ListTasks)
                .HasForeignKey(j => j.Status);

            }
            );

            modelBuilder.Entity<Domain.Entities.Interaction>(entity =>
            {
                entity.ToTable("Interactions");
                entity.HasKey(i => i.InteractionID);
                entity.Property(i => i.InteractionID).ValueGeneratedOnAdd();

                entity.Property(i => i.Date)
               .IsRequired();

                entity.Property(i => i.Notes)
                .HasColumnType("varchar(max)")
                .IsRequired();

                entity.HasOne(i => i.Projects)
               .WithMany(p => p.ListInteractions)
               .HasForeignKey(i => i.ProjectID);

                entity.HasOne(i => i.InteractionTypes)
                .WithMany(it => it.ListInteractions)
                .HasForeignKey(i => i.InteractionType);
            }
            );

            SeedData.Seed(modelBuilder);
        }

    }
}
