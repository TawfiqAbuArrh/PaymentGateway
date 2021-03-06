using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PaymentGateway_Task.Models.DB
{
    public partial class PaymentGatewayContext : DbContext
    {
        public PaymentGatewayContext()
        {
        }

        public PaymentGatewayContext(DbContextOptions<PaymentGatewayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BusinessProfile> BusinessProfile { get; set; }
        public virtual DbSet<BusinessType> BusinessType { get; set; }
        public virtual DbSet<LoginTokens> LoginTokens { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionTypes> TransactionTypes { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersType> UsersType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=HQ-IT-1728-LAP\\SQLEXPRESS;Database=PaymentGateway;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessProfile>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_BusninessProfile_UserID");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BusinessTypeId).HasColumnName("BusinessTypeID");

                entity.Property(e => e.ContactName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Pdf)
                    .HasColumnName("PDF")
                    .IsUnicode(false);

                entity.Property(e => e.PdfName)
                    .HasColumnName("PDF_Name")
                    .HasMaxLength(255);

                entity.HasOne(d => d.BusinessType)
                    .WithMany(p => p.BusinessProfile)
                    .HasForeignKey(d => d.BusinessTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Business_BusinessTypeID");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.BusinessProfile)
                    .HasForeignKey<BusinessProfile>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Business_UsersID");
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.HasKey(e => e.BusinessId)
                    .HasName("PK__Business__3AA3761ABA12D19A");

                entity.Property(e => e.BusinessId).HasColumnName("Business_Id");

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LoginTokens>(entity =>
            {
                entity.HasKey(e => e.Token)
                    .HasName("PK_Token");

                entity.Property(e => e.Token)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TokenExpiration).HasColumnType("datetime");

                entity.Property(e => e.TokenTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LoginTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_User_ID");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId)
                    .HasColumnName("Transaction_ID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionAmount).HasColumnType("decimal(19, 4)");

                entity.Property(e => e.TransactionName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TransactionTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionType_ID");

                entity.HasOne(d => d.TransactionType)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.TransactionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionType_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_UserID");
            });

            modelBuilder.Entity<TransactionTypes>(entity =>
            {
                entity.HasKey(e => e.TransactionTypeId)
                    .HasName("PK_TransactionType_ID");

                entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionType_ID");

                entity.Property(e => e.TransactionTypeName)
                    .IsRequired()
                    .HasColumnName("TransactionType_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreditBalance).HasColumnType("decimal(19, 4)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersType");
            });

            modelBuilder.Entity<UsersType>(entity =>
            {
                entity.HasKey(e => e.UserTypeId);

                entity.Property(e => e.UserTypeId)
                    .HasColumnName("UserType_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
