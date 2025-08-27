using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JapaneseLearnSystem.Models;

public partial class JapaneseLearnSystemContext : DbContext
{
    public JapaneseLearnSystemContext(DbContextOptions<JapaneseLearnSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<JPLTLevel> JPLTLevel { get; set; }

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
        modelBuilder.Entity<JPLTLevel>(entity =>
        {
            entity.HasKey(e => e.JPLTLevelID).HasName("PK__JPLTLeve__38B55CE991DDE603");

            entity.Property(e => e.JPLTLevelName).HasMaxLength(2);
        });

        modelBuilder.Entity<LearnRecordTable>(entity =>
        {
            entity.HasKey(e => e.RecordID).HasName("PK__LearnRec__FBDF78C989A173C0");

            entity.Property(e => e.RecordID).HasMaxLength(10);
            entity.Property(e => e.Accuracy).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MemberID).HasMaxLength(10);

            entity.HasOne(d => d.Member).WithMany(p => p.LearnRecordTable)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LearnRecordTable_Member");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B38A2A09B4A");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(40);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasOne(d => d.Plan).WithMany(p => p.Member)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Plan");

            entity.HasMany(d => d.Role).WithMany(p => p.Member)
                .UsingEntity<Dictionary<string, object>>(
                    "MemberRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MemberRole_Role"),
                    l => l.HasOne<Member>().WithMany()
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MemberRole_Member"),
                    j =>
                    {
                        j.HasKey("MemberID", "RoleID");
                        j.IndexerProperty<string>("MemberID").HasMaxLength(10);
                        j.IndexerProperty<string>("RoleID").HasMaxLength(10);
                    });
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__MemberAc__B0C3AC476513E2A3");

            entity.Property(e => e.Account).HasMaxLength(50);
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(100);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberAccount)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberAccount_Member");
        });

        modelBuilder.Entity<MemberPlan>(entity =>
        {
            entity.HasKey(e => e.MemberPlanID).HasName("PK__MemberPl__F266CFF4137FA81B");

            entity.Property(e => e.MemberPlanID).HasMaxLength(20);
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.ReMark_Source)
                .HasMaxLength(30)
                .HasColumnName("ReMark/Source");

            entity.HasOne(d => d.Member).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberPlan_Member");

            entity.HasOne(d => d.Plan).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberPlan_Plan");

            entity.HasOne(d => d.PlanStatus).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.PlanStatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberPlan_Status");
        });

        modelBuilder.Entity<MemberTel>(entity =>
        {
            entity.HasKey(e => e.SN).HasName("PK__MemberTe__32151C649F581C94");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberTel)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberTel_Member");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteID).HasName("PK__Note__EACE357F7A65DFD7");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.OriginalArticle).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(10);
            entity.Property(e => e.Translate).HasMaxLength(500);

            entity.HasOne(d => d.JPLTLevel).WithMany(p => p.Note)
                .HasForeignKey(d => d.JPLTLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Note_JPLTLevel");

            entity.HasOne(d => d.Member).WithMany(p => p.Note)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Note_Member");

            entity.HasMany(d => d.Word).WithMany(p => p.Note)
                .UsingEntity<Dictionary<string, object>>(
                    "NoteWordMapping",
                    r => r.HasOne<Word>().WithMany()
                        .HasForeignKey("WordID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_NoteWordMapping_Word"),
                    l => l.HasOne<Note>().WithMany()
                        .HasForeignKey("NoteID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_NoteWordMapping_Note"),
                    j =>
                    {
                        j.HasKey("NoteID", "WordID");
                    });
        });

        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasKey(e => e.PaymentID).HasName("PK__PaymentR__9B556A580EE6B806");

            entity.Property(e => e.PaymentID).HasMaxLength(20);
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Method).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.PaymentRecord)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentRecord_Member");

            entity.HasOne(d => d.Plan).WithMany(p => p.PaymentRecord)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentRecord_Plan");
        });

        modelBuilder.Entity<PlanStatus>(entity =>
        {
            entity.HasKey(e => e.PlanStatusID).HasName("PK__PlanStat__007DABE3062C5FDE");

            entity.Property(e => e.PlanStatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<QuestionInstance>(entity =>
        {
            entity.HasKey(e => e.QuestionInstanceID).HasName("PK__Question__05FDF52FD065326A");

            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);
            entity.Property(e => e.AnswerOptionID).HasMaxLength(10);
            entity.Property(e => e.QuestionContent).HasMaxLength(100);
            entity.Property(e => e.QuestionTemplateID).HasMaxLength(10);

            entity.HasOne(d => d.QuestionTemplate).WithMany(p => p.QuestionInstance)
                .HasForeignKey(d => d.QuestionTemplateID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionInstance_Template");
        });

        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.HasKey(e => new { e.QuestionInstanceID, e.OptionID });

            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);
            entity.Property(e => e.OptionID).HasMaxLength(10);
            entity.Property(e => e.OptionContent).HasMaxLength(20);

            entity.HasOne(d => d.QuestionInstance).WithMany(p => p.QuestionOption)
                .HasForeignKey(d => d.QuestionInstanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionOption_Instance");
        });

        modelBuilder.Entity<QuestionTemplate>(entity =>
        {
            entity.HasKey(e => e.QuestionTemplateID).HasName("PK__Question__1AD09B2CE3A1EC63");

            entity.Property(e => e.QuestionTemplateID).HasMaxLength(10);
            entity.Property(e => e.QuestionTemplate1)
                .HasMaxLength(100)
                .HasColumnName("QuestionTemplate");
            entity.Property(e => e.QuestionType).HasMaxLength(100);

            entity.HasOne(d => d.Word).WithMany(p => p.QuestionTemplate)
                .HasForeignKey(d => d.WordID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionTemplate_Word");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => new { e.MemberID, e.QuestionInstanceID });

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);

            entity.HasOne(d => d.Member).WithMany(p => p.Record)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Record_Member");

            entity.HasOne(d => d.QuestionInstance).WithMany(p => p.Record)
                .HasForeignKey(d => d.QuestionInstanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Record_QuestionInstance");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK__Role__8AFACE3A6B504D52");

            entity.Property(e => e.RoleID).HasMaxLength(10);
            entity.Property(e => e.RoleName).HasMaxLength(40);
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.PlanID).HasName("PK__Subscrip__755C22D7ED37763D");

            entity.Property(e => e.FeeInfo).HasMaxLength(100);
            entity.Property(e => e.PlanName).HasMaxLength(50);
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.WordID).HasName("PK__Word__2C20F0462859A6AC");

            entity.Property(e => e.PartOfSpeech).HasMaxLength(5);
            entity.Property(e => e.Word1)
                .HasMaxLength(20)
                .HasColumnName("Word");
            entity.Property(e => e.WordTranslate).HasMaxLength(20);

            entity.HasOne(d => d.JPLTLevel).WithMany(p => p.Word)
                .HasForeignKey(d => d.JPLTLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Word_JPLTLevel");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
