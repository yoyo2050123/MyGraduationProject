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

    public virtual DbSet<JLPTLevel> JLPTLevel { get; set; }

    public virtual DbSet<LearnRecordTable> LearnRecordTable { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<MemberAccount> MemberAccount { get; set; }

    public virtual DbSet<MemberPlan> MemberPlan { get; set; }

    public virtual DbSet<MemberRole> MemberRole { get; set; }

    public virtual DbSet<MemberTel> MemberTel { get; set; }

    public virtual DbSet<MemberUsageLog> MemberUsageLog { get; set; }

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
            entity.HasKey(e => e.JLPTLevelID).HasName("PK__JLPTLeve__8536E555E56DA6D4");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.JLPTLevelName).HasMaxLength(2);
        });

        modelBuilder.Entity<LearnRecordTable>(entity =>
        {
            entity.HasKey(e => e.RecordID).HasName("PK__LearnRec__FBDF78C9BD2BDBC7");

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
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B38515D6719");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(40);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasOne(d => d.Plan).WithMany(p => p.Member)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_SubscriptionPlan");
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__MemberAc__B0C3AC47C8891AAB");

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
            entity.HasKey(e => e.MemberPlanID).HasName("PK__MemberPl__F266CFF4DA71B447");

            entity.Property(e => e.MemberPlanID).HasMaxLength(10);
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.RemarkSource).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberPlan_Member");

            entity.HasOne(d => d.Plan).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberPlan_SubscriptionPlan");

            entity.HasOne(d => d.PlanStatus).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.PlanStatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberPlan_PlanStatus");
        });

        modelBuilder.Entity<MemberRole>(entity =>
        {
            entity.HasKey(e => new { e.MemberID, e.RoleID }).HasName("PK__MemberRo__B45FE7DBA9E52A9B");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.RoleID).HasMaxLength(10);
            entity.Property(e => e.Empty)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.MemberRole)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberRole_Member");

            entity.HasOne(d => d.Role).WithMany(p => p.MemberRole)
                .HasForeignKey(d => d.RoleID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberRole_Role");
        });

        modelBuilder.Entity<MemberTel>(entity =>
        {
            entity.HasKey(e => e.SN).HasName("PK__MemberTe__32151C64306C4415");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberTel)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberTel_Member");
        });

        modelBuilder.Entity<MemberUsageLog>(entity =>
        {
            entity.HasKey(e => e.UsageLogID).HasName("PK__MemberUs__8F97EC831878006A");

            entity.HasIndex(e => new { e.MemberID, e.UsageLogDate }, "IX_MemberUsageLog_Member_UsageLogDate");

            entity.Property(e => e.UsageLogID).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.NoteCount).HasDefaultValue(0);
            entity.Property(e => e.QuestionCount).HasDefaultValue(0);
            entity.Property(e => e.WordCount).HasDefaultValue(0);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberUsageLog)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemberUsageLog_Member");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteID).HasName("PK__Note__EACE357F1E4FB7AA");

            entity.Property(e => e.CreateNoteAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.OriginalArticle).HasMaxLength(500);
            entity.Property(e => e.Reading).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Translate).HasMaxLength(500);

            entity.HasOne(d => d.JLPTLevel).WithMany(p => p.Note)
                .HasForeignKey(d => d.JLPTLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Note_JLPTLevel");

            entity.HasOne(d => d.Member).WithMany(p => p.Note)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Note_Member");
        });

        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasKey(e => e.PaymentID).HasName("PK__PaymentR__9B556A58AFBFA647");

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
                .HasConstraintName("FK_PaymentRecord_SubscriptionPlan");
        });

        modelBuilder.Entity<PlanStatus>(entity =>
        {
            entity.HasKey(e => e.PlanStatusID).HasName("PK__PlanStat__007DABE36DFB16DB");

            entity.Property(e => e.PlanStatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<QuestionInstance>(entity =>
        {
            entity.HasKey(e => e.QuestionInstanceID).HasName("PK__Question__05FDF52F35D03698");

            entity.Property(e => e.QuestionInstanceID).HasMaxLength(50);
            entity.Property(e => e.AnswerOptionID).HasMaxLength(10);
            entity.Property(e => e.QuestionContent).HasMaxLength(100);
            entity.Property(e => e.QuestionTemplateID).HasMaxLength(10);

            entity.HasOne(d => d.QuestionTemplate).WithMany(p => p.QuestionInstance)
                .HasForeignKey(d => d.QuestionTemplateID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionInstance_Template");

            entity.HasOne(d => d.Word).WithMany(p => p.QuestionInstance)
                .HasForeignKey(d => d.WordID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionInstance_Word");
        });

        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.HasKey(e => new { e.QuestionInstanceID, e.OptionID }).HasName("PK__Question__ECD18F3261CA27DE");

            entity.Property(e => e.QuestionInstanceID).HasMaxLength(50);
            entity.Property(e => e.OptionID).HasMaxLength(50);
            entity.Property(e => e.OptionContent).HasMaxLength(20);

            entity.HasOne(d => d.QuestionInstance).WithMany(p => p.QuestionOption)
                .HasForeignKey(d => d.QuestionInstanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionOption_Instance");
        });

        modelBuilder.Entity<QuestionTemplate>(entity =>
        {
            entity.HasKey(e => e.QuestionTemplateID).HasName("PK__Question__1AD09B2CCC2B8705");

            entity.Property(e => e.QuestionTemplateID).HasMaxLength(10);
            entity.Property(e => e.QuestionTemplateText).HasMaxLength(100);
            entity.Property(e => e.QuestionType).HasMaxLength(100);
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => new { e.MemberID, e.QuestionInstanceID }).HasName("PK__Record__ECAF946A92B11E9E");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.QuestionInstanceID).HasMaxLength(50);

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
            entity.HasKey(e => e.RoleID).HasName("PK__Role__8AFACE3A7F5AAEA0");

            entity.Property(e => e.RoleID).HasMaxLength(10);
            entity.Property(e => e.RoleName).HasMaxLength(40);
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.PlanID).HasName("PK__Subscrip__755C22D7A48E3D3A");

            entity.Property(e => e.FeeInfo).HasMaxLength(100);
            entity.Property(e => e.PlanName).HasMaxLength(50);
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.WordID).HasName("PK__Word__2C20F046A4521691");

            entity.Property(e => e.PartOfSpeech).HasMaxLength(50);
            entity.Property(e => e.Reading).HasMaxLength(100);
            entity.Property(e => e.Vocabulary).HasMaxLength(50);
            entity.Property(e => e.WordTranslate).HasMaxLength(20);

            entity.HasOne(d => d.JLPTLevel).WithMany(p => p.Word)
                .HasForeignKey(d => d.JLPTLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Word_JLPTLevel");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
