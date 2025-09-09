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
            entity.HasKey(e => e.JLPTLevelID).HasName("PK__JLPTLeve__8536E555BE00EC0B");

            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.JLPTLevelName).HasMaxLength(2);
        });

        modelBuilder.Entity<LearnRecordTable>(entity =>
        {
            entity.HasKey(e => e.RecordID).HasName("PK__LearnRec__FBDF78C97764B982");

            entity.Property(e => e.RecordID).HasMaxLength(10);
            entity.Property(e => e.Accuracy).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MemberID).HasMaxLength(10);

            entity.HasOne(d => d.Member).WithMany(p => p.LearnRecordTable)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearnReco__Membe__628FA481");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B385012964D");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(40);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasOne(d => d.Plan).WithMany(p => p.Member)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Member__PlanID__3B75D760");
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__MemberAc__B0C3AC47D78E76D7");

            entity.Property(e => e.Account).HasMaxLength(50);
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(100);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberAccount)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberAcc__Membe__46E78A0C");
        });

        modelBuilder.Entity<MemberPlan>(entity =>
        {
            entity.HasKey(e => e.MemberPlanID).HasName("PK__MemberPl__F266CFF4E55D44BA");

            entity.Property(e => e.MemberPlanID).HasMaxLength(50);
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.RemarkSource).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPla__Membe__693CA210");

            entity.HasOne(d => d.Plan).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPla__PlanI__6A30C649");

            entity.HasOne(d => d.PlanStatus).WithMany(p => p.MemberPlan)
                .HasForeignKey(d => d.PlanStatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberPla__PlanS__6B24EA82");
        });

        modelBuilder.Entity<MemberRole>(entity =>
        {
            entity.HasKey(e => new { e.MemberID, e.RoleID }).HasName("PK__MemberRo__B45FE7DB387981C7");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.RoleID).HasMaxLength(10);
            entity.Property(e => e.Empty)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.MemberRole)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberRol__Membe__403A8C7D");

            entity.HasOne(d => d.Role).WithMany(p => p.MemberRole)
                .HasForeignKey(d => d.RoleID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberRol__RoleI__412EB0B6");
        });

        modelBuilder.Entity<MemberTel>(entity =>
        {
            entity.HasKey(e => e.SN).HasName("PK__MemberTe__32151C64DCE92D6A");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Tel).HasMaxLength(20);

            entity.HasOne(d => d.Member).WithMany(p => p.MemberTel)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberTel__Membe__440B1D61");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteID).HasName("PK__Note__EACE357FBAFBC97B");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.OriginalArticle).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(10);
            entity.Property(e => e.Translate).HasMaxLength(500);

            entity.HasOne(d => d.JLPTLevel).WithMany(p => p.Note)
                .HasForeignKey(d => d.JLPTLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Note__JLPTLevelI__4E88ABD4");

            entity.HasOne(d => d.Member).WithMany(p => p.Note)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Note__MemberID__4F7CD00D");

            entity.HasMany(d => d.Word).WithMany(p => p.Note)
                .UsingEntity<Dictionary<string, object>>(
                    "NoteWordMapping",
                    r => r.HasOne<Word>().WithMany()
                        .HasForeignKey("WordID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__NoteWordM__WordI__534D60F1"),
                    l => l.HasOne<Note>().WithMany()
                        .HasForeignKey("NoteID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__NoteWordM__NoteI__52593CB8"),
                    j =>
                    {
                        j.HasKey("NoteID", "WordID").HasName("PK__NoteWord__980C3A7B731F707E");
                    });
        });

        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasKey(e => e.PaymentID).HasName("PK__PaymentR__9B556A5806A7FD82");

            entity.Property(e => e.PaymentID).HasMaxLength(20);
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.Method).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithMany(p => p.PaymentRecord)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentRe__Membe__656C112C");

            entity.HasOne(d => d.Plan).WithMany(p => p.PaymentRecord)
                .HasForeignKey(d => d.PlanID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentRe__PlanI__66603565");
        });

        modelBuilder.Entity<PlanStatus>(entity =>
        {
            entity.HasKey(e => e.PlanStatusID).HasName("PK__PlanStat__007DABE3C428BE66");

            entity.Property(e => e.PlanStatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<QuestionInstance>(entity =>
        {
            entity.HasKey(e => e.QuestionInstanceID).HasName("PK__Question__05FDF52FB21A51DE");

            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);
            entity.Property(e => e.AnswerOptionID).HasMaxLength(10);
            entity.Property(e => e.QuestionContent).HasMaxLength(100);
            entity.Property(e => e.QuestionTemplateID).HasMaxLength(10);

            entity.HasOne(d => d.QuestionTemplate).WithMany(p => p.QuestionInstance)
                .HasForeignKey(d => d.QuestionTemplateID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestionI__Quest__59063A47");
        });

        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.HasKey(e => new { e.QuestionInstanceID, e.OptionID }).HasName("PK__Question__ECD18F32610B33ED");

            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);
            entity.Property(e => e.OptionID).HasMaxLength(10);
            entity.Property(e => e.OptionContent).HasMaxLength(20);

            entity.HasOne(d => d.QuestionInstance).WithMany(p => p.QuestionOption)
                .HasForeignKey(d => d.QuestionInstanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestionO__Quest__5BE2A6F2");
        });

        modelBuilder.Entity<QuestionTemplate>(entity =>
        {
            entity.HasKey(e => e.QuestionTemplateID).HasName("PK__Question__1AD09B2C79852D22");

            entity.Property(e => e.QuestionTemplateID).HasMaxLength(10);
            entity.Property(e => e.QuestionTemplate1)
                .HasMaxLength(100)
                .HasColumnName("QuestionTemplate");
            entity.Property(e => e.QuestionType).HasMaxLength(100);

            entity.HasOne(d => d.Word).WithMany(p => p.QuestionTemplate)
                .HasForeignKey(d => d.WordID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__QuestionT__WordI__5629CD9C");
        });

        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => new { e.MemberID, e.QuestionInstanceID }).HasName("PK__Record__ECAF946A5E94B557");

            entity.Property(e => e.MemberID).HasMaxLength(10);
            entity.Property(e => e.QuestionInstanceID).HasMaxLength(10);

            entity.HasOne(d => d.Member).WithMany(p => p.Record)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Record__MemberID__5EBF139D");

            entity.HasOne(d => d.QuestionInstance).WithMany(p => p.Record)
                .HasForeignKey(d => d.QuestionInstanceID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Record__Question__5FB337D6");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK__Role__8AFACE3A04E0DD75");

            entity.Property(e => e.RoleID).HasMaxLength(10);
            entity.Property(e => e.RoleName).HasMaxLength(40);
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.PlanID).HasName("PK__Subscrip__755C22D70D47FA36");

            entity.Property(e => e.FeeInfo).HasMaxLength(100);
            entity.Property(e => e.PlanName).HasMaxLength(50);
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.WordID).HasName("PK__Word__2C20F0465E82737A");

            entity.Property(e => e.PartOfSpeech).HasMaxLength(5);
            entity.Property(e => e.Word1)
                .HasMaxLength(20)
                .HasColumnName("Word");
            entity.Property(e => e.WordTranslate).HasMaxLength(20);

            entity.HasOne(d => d.JLPTLevel).WithMany(p => p.Word)
                .HasForeignKey(d => d.JLPTLevelID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Word__JLPTLevelI__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
