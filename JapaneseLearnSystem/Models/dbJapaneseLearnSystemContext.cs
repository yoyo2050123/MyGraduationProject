using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearnSystem.Models;

public partial class dbJapaneseLearnSystemContext : DbContext
{
    public dbJapaneseLearnSystemContext(DbContextOptions<dbJapaneseLearnSystemContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         => optionsBuilder.UseSqlServer("Data Source=C501A24;Database=dbJapaneseLearnSystem;TrustServerCertificate=True;User ID=abc;Password=123");

    public virtual DbSet<JLPTLevel> JLPTLevel { get; set; }

    public virtual DbSet<LearnRecordTable> LearnRecordTable { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<MemberAccount> MemberAccount { get; set; }

    public virtual DbSet<MemberPlan> MemberPlan { get; set; }

    public virtual DbSet<MemberTel> MemberTel { get; set; }

    public virtual DbSet<Note> Note { get; set; }

    public virtual DbSet<PaymentRecord> PaymentRecord { get; set; }

    public virtual DbSet<PlanStatus> PlanStatus { get; set; }

    public virtual DbSet<QuestionInstance> QuestionInstance { get; set; }

    public virtual DbSet<QuestionOption> QuestionOption { get; set; }

    public virtual DbSet<QuestionTemplate> QuestionTemplate { get; set; }

    public virtual DbSet<Record> Record { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlan { get; set; }

    public virtual DbSet<Word> Word { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JLPTLevel>(entity =>
        {
            entity.HasKey(e => e.JLPTLevelID).HasName("PK__JLPTLeve__8536E55573FDEAD2");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.JLPTLevelName).HasMaxLength(2);
        });

        modelBuilder.Entity<LearnRecordTable>(entity =>
        {
            entity.HasKey(e => e.RecordID).HasName("PK__LearnRec__FBDF78C965508809");

            entity.Property(e => e.RecordID).HasMaxLength(10);
            entity.Property(e => e.Accuracy).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MemberID).HasMaxLength(10);

            entity.HasOne(d => d.Member).WithMany(p => p.LearnRecordTable)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearnReco__Membe__5DCAEF64");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B3868134AB6");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(40);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasMany(d => d.Role).WithMany(p => p.Member)
                .UsingEntity<Dictionary<string, object>>(
                    "MemberRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MemberRol__RoleI__3C69FB99"),
                    l => l.HasOne<Member>().WithMany()
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MemberRol__Membe__3B75D760"),
                    j =>
                    {
                        j.HasKey("MemberID", "RoleID").HasName("PK__MemberRo__B45FE7DBFF1C31B2");
                        j.IndexerProperty<string>("MemberID").HasMaxLength(10);
                        j.IndexerProperty<string>("RoleID").HasMaxLength(10);
                    });
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__MemberAc__B0C3AC4796B8A96C");

            entity.Property(e => e.Account).HasMaxLength(50);
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(100);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberAccount)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberAcc__Membe__4222D4EF");
        });

        modelBuilder.Entity<MemberPlan>(entity =>
        {
            entity.HasKey(e => e.MemberPlanID).HasName("PK__MemberPl__F266CFF45BA99722");

            entity.Property(e => e.MemberPlanID).HasMaxLength(50);
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.RemarkSource).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPla__Membe__68487DD7");

            entity.HasOne(d => d.Plan).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPla__PlanI__693CA210");

            entity.HasOne(d => d.PlanStatus).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.PlanStatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPla__PlanS__6A30C649");
        });

        modelBuilder.Entity<MemberTel>(entity =>
        {
            entity.HasKey(e => e.SN).HasName("PK__MemberTe__32151C64CFFEDD05");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberTel)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberTel__Membe__3F466844");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteID).HasName("PK__Note__EACE357FCE9ECA36");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.OriginalArticle).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(10);
            entity.Property(e => e.Translate).HasMaxLength(500);

            entity.HasOne(d => d.JLPTLevel).WithMany(p => p.Note)
                .HasForeignKey(d => d.JLPTLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Note__JLPTLevelI__49C3F6B7");

            entity.HasOne(d => d.Member).WithMany(p => p.Note)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Note__MemberID__4AB81AF0");

            entity.HasMany(d => d.Word).WithMany(p => p.Note)
                .UsingEntity<Dictionary<string, object>>(
                    "NoteWordMapping",
                    r => r.HasOne<Word>().WithMany()
                        .HasForeignKey("WordID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__NoteWordM__WordI__4E88ABD4"),
                    l => l.HasOne<Note>().WithMany()
                        .HasForeignKey("NoteID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__NoteWordM__NoteI__4D94879B"),
                    j =>
                    {
                        j.HasKey("NoteID", "WordID").HasName("PK__NoteWord__980C3A7B6A1C49F3");
                    });
        });

        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasKey(e => e.PaymentID).HasName("PK__PaymentR__9B556A58DAF2C7F1");

            entity.Property(e => e.PaymentID).HasMaxLength(20);
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Method).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.PaymentRecord)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentRe__Membe__628FA481");

            entity.HasOne(d => d.Plan).WithMany(p => p.PaymentRecord)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentRe__PlanI__6383C8BA");
        });

        modelBuilder.Entity<PlanStatus>(entity =>
        {
            entity.HasKey(e => e.PlanStatusID).HasName("PK__PlanStat__007DABE3DA5AB93D");

            entity.Property(e => e.PlanStatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<QuestionInstance>(entity =>
        {
            entity.HasKey(e => e.QuestionInstanceID).HasName("PK__Question__05FDF52F70D4ADB6");

            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);
            entity.Property(e => e.AnswerOptionID).HasMaxLength(10);
            entity.Property(e => e.QuestionContent).HasMaxLength(100);
            entity.Property(e => e.QuestionTemplateID).HasMaxLength(10);

            entity.HasOne(d => d.QuestionTemplate).WithMany(p => p.QuestionInstance)
                .HasForeignKey(d => d.QuestionTemplateID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestionI__Quest__5441852A");
        });

        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.HasKey(e => new { e.QuestionInstanceID, e.OptionID }).HasName("PK__Question__ECD18F3274323620");

            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);
            entity.Property(e => e.OptionID).HasMaxLength(10);
            entity.Property(e => e.OptionContent).HasMaxLength(20);

            entity.HasOne(d => d.QuestionInstance).WithMany(p => p.QuestionOption)
                .HasForeignKey(d => d.QuestionInstanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestionO__Quest__571DF1D5");
        });

        modelBuilder.Entity<QuestionTemplate>(entity =>
        {
            entity.HasKey(e => e.QuestionTemplateID).HasName("PK__Question__1AD09B2CDF33040A");

            entity.Property(e => e.QuestionTemplateID).HasMaxLength(10);
            entity.Property(e => e.QuestionTemplate1)
                .HasMaxLength(100)
                .HasColumnName("QuestionTemplate");
            entity.Property(e => e.QuestionType).HasMaxLength(100);

            entity.HasOne(d => d.Word).WithMany(p => p.QuestionTemplate)
                .HasForeignKey(d => d.WordID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestionT__WordI__5165187F");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => new { e.MemberID, e.QuestionInstanceID }).HasName("PK__Record__ECAF946A0101B296");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);

            entity.HasOne(d => d.Member).WithMany(p => p.Record)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Record__MemberID__59FA5E80");

            entity.HasOne(d => d.QuestionInstance).WithMany(p => p.Record)
                .HasForeignKey(d => d.QuestionInstanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Record__Question__5AEE82B9");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK__Role__8AFACE3A604BBB46");

            entity.Property(e => e.RoleID).HasMaxLength(10);
            entity.Property(e => e.RoleName).HasMaxLength(40);
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.PlanID).HasName("PK__Subscrip__755C22D7CB188262");

            entity.Property(e => e.FeeInfo).HasMaxLength(100);
            entity.Property(e => e.PlanName).HasMaxLength(50);
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.WordID).HasName("PK__Word__2C20F046E11B5554");

            entity.Property(e => e.PartOfSpeech).HasMaxLength(5);
            entity.Property(e => e.Word1)
                .HasMaxLength(20)
                .HasColumnName("Word");
            entity.Property(e => e.WordTranslate).HasMaxLength(20);

            entity.HasOne(d => d.JLPTLevel).WithMany(p => p.Word)
                .HasForeignKey(d => d.JLPTLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Word__JLPTLevelI__46E78A0C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
