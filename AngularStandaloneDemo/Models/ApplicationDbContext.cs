using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AngularStandaloneDemo.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Allergy> Allergies { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Availability> Availabilities { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Immunization> Immunizations { get; set; }

    public virtual DbSet<LabResult> LabResults { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Medication> Medications { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientDetail> PatientDetails { get; set; }

    public virtual DbSet<PatientInfoDto> PatientInfoDtos { get; set; }

    public virtual DbSet<PatientTask> PatientTasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<WaitingList> WaitingLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=JALAL_BAKIR\\MSSQLSERVER01;Database=AngularStandaloneDemo;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.ToTable("Allergy");

            entity.HasIndex(e => e.PatientId, "IX_Allergy_PatientId");

            entity.HasOne(d => d.Patient).WithMany(p => p.Allergies).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasIndex(e => e.PatientId, "IX_Appointments_PatientId");

            entity.HasIndex(e => e.ProviderId, "IX_Appointments_ProviderId");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments).HasForeignKey(d => d.PatientId);

            entity.HasOne(d => d.Provider).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Availabilities_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.Availabilities).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.ToTable("Diagnosis");

            entity.HasIndex(e => e.VisitId, "IX_Diagnosis_VisitId");

            entity.HasOne(d => d.Visit).WithMany(p => p.Diagnoses).HasForeignKey(d => d.VisitId);
        });

        modelBuilder.Entity<Immunization>(entity =>
        {
            entity.ToTable("Immunization");

            entity.HasIndex(e => e.PatientId, "IX_Immunization_PatientId");

            entity.Property(e => e.AdministeringProvider).HasMaxLength(100);
            entity.Property(e => e.LotNumber).HasMaxLength(50);
            entity.Property(e => e.VaccineName).HasMaxLength(100);

            entity.HasOne(d => d.Patient).WithMany(p => p.Immunizations).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<LabResult>(entity =>
        {
            entity.ToTable("LabResult");

            entity.HasIndex(e => e.PatientId, "IX_LabResult_PatientId");

            entity.HasOne(d => d.Patient).WithMany(p => p.LabResults).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasIndex(e => e.PatientId, "IX_MedicalRecords_PatientId");

            entity.HasIndex(e => e.UserID, "IX_MedicalRecords_UserID");

           // entity.HasIndex(e => e.UserId1, "IX_MedicalRecords_UserID1");

            entity.Property(e => e.Bmi).HasColumnName("BMI");
            entity.Property(e => e.UserID).HasColumnName("UserID");
          //  entity.Property(e => e.UserId1).HasColumnName("UserID1");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicalRecords).HasForeignKey(d => d.PatientId);

            entity.HasOne(d => d.User).WithMany(p => p.MedicalRecordUsers)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.ClientSetNull);

           // entity.HasOne(d => d.UserID).WithMany(p => p.MedicalRecordUserId1Navigations).HasForeignKey(d => d.UserId1);
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.ToTable("Medication");

            entity.HasIndex(e => e.PatientId, "IX_Medication_PatientId");

            entity.HasOne(d => d.Patient).WithMany(p => p.Medications).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.Property(e => e.GenderID).HasColumnName("GenderID");
            entity.Property(e => e.NursID).HasColumnName("NursID");
            entity.Property(e => e.PatientDoctorID).HasColumnName("PatientDoctorID");
        });

        modelBuilder.Entity<PatientDetail>(entity =>
        {
            entity.Property(e => e.BedNumber).HasMaxLength(10);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PrimaryDiagnosis).HasMaxLength(200);
            entity.Property(e => e.RoomNumber).HasMaxLength(10);
        });

        modelBuilder.Entity<PatientInfoDto>(entity =>
        {
            entity.ToTable("PatientInfoDto");

            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.NursId).HasColumnName("NursID");
            entity.Property(e => e.PatientDoctorId).HasColumnName("PatientDoctorID");
        });

        modelBuilder.Entity<PatientTask>(entity =>
        {
            entity.HasIndex(e => e.AssignedToNurseId, "IX_PatientTasks_AssignedToNurseId");

            entity.HasIndex(e => e.CreatedByNurseId, "IX_PatientTasks_CreatedByNurseId");

            entity.HasIndex(e => e.PatientId, "IX_PatientTasks_PatientId");

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.AssignedToNurse).WithMany(p => p.PatientTaskAssignedToNurses)
                .HasForeignKey(d => d.AssignedToNurseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CreatedByNurse).WithMany(p => p.PatientTaskCreatedByNurses)
                .HasForeignKey(d => d.CreatedByNurseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Patient).WithMany(p => p.PatientTasks)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserID).HasColumnName("UserID");
            entity.Property(e => e.GenderID).HasColumnName("GenderID");
            entity.Property(e => e.JobTitleID).HasColumnName("JobTitleID");
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.ToTable("Visit");

            entity.HasIndex(e => e.MedicalRecordId, "IX_Visit_MedicalRecordId");

            entity.HasIndex(e => e.PatientId, "IX_Visit_PatientId");

            entity.Property(e => e.FollowUpRequired).HasColumnName("followUpRequired");

            entity.HasOne(d => d.MedicalRecord).WithMany(p => p.Visits).HasForeignKey(d => d.MedicalRecordId);

            entity.HasOne(d => d.Patient).WithMany(p => p.Visits).HasForeignKey(d => d.PatientId);
        });

        modelBuilder.Entity<WaitingList>(entity =>
        {
            entity.ToTable("WaitingList");

            entity.HasIndex(e => e.PatientId, "IX_WaitingList_PatientId");

            entity.HasIndex(e => e.ProviderId, "IX_WaitingList_ProviderId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.WaitingList)
                .HasForeignKey<WaitingList>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Patient).WithMany(p => p.WaitingLists).HasForeignKey(d => d.PatientId);

            entity.HasOne(d => d.Provider).WithMany(p => p.WaitingLists).HasForeignKey(d => d.ProviderId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
