-- ==========================
-- 建立資料庫
-- ==========================
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'JapaneseLearnSystem')
BEGIN
    CREATE DATABASE JapaneseLearnSystem;
END
GO

USE JapaneseLearnSystem;
GO

-- ==========================
-- 1. JPLT 等級表
-- ==========================
CREATE TABLE JPLTLevel (
    JPLTLevelID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    JPLTLevelName NVARCHAR(2) NOT NULL
);
GO

-- ==========================
-- 2. 訂閱方案表
-- ==========================
CREATE TABLE SubscriptionPlan (
    PlanID TINYINT NOT NULL PRIMARY KEY,
    PlanName NVARCHAR(50) NOT NULL,
    FeeInfo NVARCHAR(100) NOT NULL,
    LimitCount INT NULL
);
GO

-- ==========================
-- 3. 方案狀態表
-- ==========================
CREATE TABLE PlanStatus (
    PlanStatusID TINYINT NOT NULL PRIMARY KEY,
    PlanStatusName NVARCHAR(50) NOT NULL
);
GO

-- ==========================
-- 4. 會員表
-- ==========================
CREATE TABLE Member (
    MemberID NVARCHAR(10) NOT NULL PRIMARY KEY,
    Name NVARCHAR(40) NOT NULL,
    Tel NVARCHAR(20) NOT NULL,
    PlanID TINYINT NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Birthday DATE NOT NULL,
    CONSTRAINT FK_Member_Plan FOREIGN KEY (PlanID) REFERENCES SubscriptionPlan(PlanID)
);
GO

-- ==========================
-- 5. 身分表 (Role)
-- ==========================
CREATE TABLE Role (
    RoleID NVARCHAR(10) NOT NULL PRIMARY KEY,
    RoleName NVARCHAR(40) NOT NULL
);
GO
-- ==========================
-- 6. 會員身分中介表 (MemberRole)
-- ==========================
CREATE TABLE MemberRole (
    MemberID NVARCHAR(10) NOT NULL,
    RoleID NVARCHAR(10) NOT NULL,
    CONSTRAINT PK_MemberRole PRIMARY KEY (MemberID, RoleID),
    CONSTRAINT FK_MemberRole_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
    CONSTRAINT FK_MemberRole_Role FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
);
GO



-- ==========================
-- 7. 會員電話
-- ==========================
CREATE TABLE MemberTel (
    SN INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Tel NVARCHAR(20) NOT NULL,
    MemberID NVARCHAR(10) NOT NULL,
    CONSTRAINT FK_MemberTel_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);
GO

-- ==========================
-- 8. 會員帳密
-- ==========================
CREATE TABLE MemberAccount (
    Account NVARCHAR(50) NOT NULL PRIMARY KEY,
    Password NVARCHAR(100) NOT NULL,
    MemberID NVARCHAR(10) NOT NULL,
    CONSTRAINT FK_MemberAccount_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);
GO

-- ==========================
-- 9. 會員方案紀錄
-- ==========================
CREATE TABLE MemberPlan (
    MemberPlanID NVARCHAR(20) NOT NULL PRIMARY KEY,
    MemberID NVARCHAR(10) NOT NULL,
    PlanID TINYINT NOT NULL,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    [ReMark/Source] NVARCHAR(30) NOT NULL,
    PlanStatusID TINYINT NOT NULL,
    CONSTRAINT FK_MemberPlan_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
    CONSTRAINT FK_MemberPlan_Plan FOREIGN KEY (PlanID) REFERENCES SubscriptionPlan(PlanID),
    CONSTRAINT FK_MemberPlan_Status FOREIGN KEY (PlanStatusID) REFERENCES PlanStatus(PlanStatusID)
);
GO

-- ==========================
-- 10. 付款紀錄表
-- ==========================
CREATE TABLE PaymentRecord (
    PaymentID NVARCHAR(20) NOT NULL PRIMARY KEY,
    MemberID NVARCHAR(10) NOT NULL,
    PlanID TINYINT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    PaymentDate DATETIME2 NOT NULL,
    Method NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_PaymentRecord_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
    CONSTRAINT FK_PaymentRecord_Plan FOREIGN KEY (PlanID) REFERENCES SubscriptionPlan(PlanID)
);
GO

-- ==========================
-- 11. 單字表
-- ==========================
CREATE TABLE Word (
    WordID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Word NVARCHAR(20) NOT NULL,
    PartOfSpeech NVARCHAR(5) NOT NULL,
    WordTranslate NVARCHAR(20) NOT NULL,
    JPLTLevelID INT NOT NULL,
    CONSTRAINT FK_Word_JPLTLevel FOREIGN KEY (JPLTLevelID) REFERENCES JPLTLevel(JPLTLevelID)
);
GO

-- ==========================
-- 12. 筆記表
-- ==========================
CREATE TABLE Note (
    NoteID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title NVARCHAR(10) NOT NULL,
    OriginalArticle NVARCHAR(500) NOT NULL,
    Translate NVARCHAR(500) NOT NULL,
    JPLTLevelID INT NOT NULL,
    MemberID NVARCHAR(10) NOT NULL,
    CONSTRAINT FK_Note_JPLTLevel FOREIGN KEY (JPLTLevelID) REFERENCES JPLTLevel(JPLTLevelID),
    CONSTRAINT FK_Note_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);
GO

-- ==========================
-- 13. 筆記中介表
-- ==========================
CREATE TABLE NoteWordMapping (
    NoteID INT NOT NULL,
    WordID INT NOT NULL,
    CONSTRAINT PK_NoteWordMapping PRIMARY KEY (NoteID, WordID),
    CONSTRAINT FK_NoteWordMapping_Note FOREIGN KEY (NoteID) REFERENCES Note(NoteID),
    CONSTRAINT FK_NoteWordMapping_Word FOREIGN KEY (WordID) REFERENCES Word(WordID)
);
GO

-- ==========================
-- 14. 題目模板表
-- ==========================
CREATE TABLE QuestionTemplate (
    QuestionTemplateID NVARCHAR(10) NOT NULL PRIMARY KEY,
    WordID INT NOT NULL,
    QuestionType NVARCHAR(100) NOT NULL,
    QuestionTemplate NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_QuestionTemplate_Word FOREIGN KEY (WordID) REFERENCES Word(WordID)
);
GO

-- ==========================
-- 15. 題目實例表
-- ==========================
CREATE TABLE QuestionInstance (
    QuestionInstanceID NVARCHAR(10) NOT NULL PRIMARY KEY,
    QuestionTemplateID NVARCHAR(10) NOT NULL,
    AnswerOptionID NVARCHAR(10) NOT NULL,
    QuestionContent NVARCHAR(100) NOT NULL,
    CreateDate DATETIME2 NOT NULL,
    CONSTRAINT FK_QuestionInstance_Template FOREIGN KEY (QuestionTemplateID) REFERENCES QuestionTemplate(QuestionTemplateID)
);
GO

-- ==========================
-- 16. 題目選項表
-- ==========================
CREATE TABLE QuestionOption (
    QuestionInstanceID NVARCHAR(10) NOT NULL,
    OptionID NVARCHAR(10) NOT NULL,
    OptionContent NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_QuestionOption PRIMARY KEY (QuestionInstanceID, OptionID),
    CONSTRAINT FK_QuestionOption_Instance FOREIGN KEY (QuestionInstanceID) REFERENCES QuestionInstance(QuestionInstanceID)
);
GO

-- ==========================
-- 17. 答題記錄
-- ==========================
CREATE TABLE Record (
    MemberID NVARCHAR(10) NOT NULL,
    QuestionInstanceID NVARCHAR(10) NOT NULL,
    IsCorrect BIT NOT NULL,
    RecordTime DATETIME2 NOT NULL,
    AnswerTime DATETIME2 NOT NULL,
    CONSTRAINT PK_Record PRIMARY KEY (MemberID, QuestionInstanceID),
    CONSTRAINT FK_Record_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID),
    CONSTRAINT FK_Record_QuestionInstance FOREIGN KEY (QuestionInstanceID) REFERENCES QuestionInstance(QuestionInstanceID)
);
GO

-- ==========================
-- 18. 學習報表
-- ==========================
CREATE TABLE LearnRecordTable (
    RecordID NVARCHAR(10) NOT NULL PRIMARY KEY,
    MemberID NVARCHAR(10) NOT NULL,
    LearnedWordCount INT NOT NULL,
    TotalAnswers INT NOT NULL,
    CorrectAnswers INT NOT NULL,
    Accuracy DECIMAL(5,2) NOT NULL,
    AnswerTime DATETIME2 NOT NULL,
    CONSTRAINT FK_LearnRecordTable_Member FOREIGN KEY (MemberID) REFERENCES Member(MemberID)
);
GO
