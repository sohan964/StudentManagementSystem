CREATE TABLE Departments (
    department_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    code NVARCHAR(20),
    description NVARCHAR(MAX)
);

CREATE TABLE Classes (
    class_id INT IDENTITY(1,1) PRIMARY KEY,
    class_name NVARCHAR(20) NOT NULL,
    is_secondary BIT DEFAULT 0,
    description NVARCHAR(255)
);

CREATE TABLE Subjects (
    subject_id INT IDENTITY(1,1) PRIMARY KEY,
    subject_code NVARCHAR(30),
    name NVARCHAR(150) NOT NULL,
    is_theory BIT DEFAULT 1,
    is_practical BIT DEFAULT 0,
    default_marks INT DEFAULT 100,
	department_id INT NULL
	CONSTRAINT FK_subjects_departments FOREIGN KEY (department_id) REFERENCES departments(department_id)
);

ALTER TABLE Subjects
ADD department_id INT NULL
CONSTRAINT FK_subjects_departments FOREIGN KEY (department_id) REFERENCES departments(department_id);

ALTER TABLE Subjects 
ADD credit_hours INT DEFAULT 1;


select * From Departments
select *From Subjects
select * FROM Classes
select * from ClassSubjects

CREATE TABLE Academic_years (
    year_id INT IDENTITY(1,1) PRIMARY KEY,
    year_label NVARCHAR(30) NOT NULL,
    start_date DATE,
    end_date DATE,
    is_active BIT DEFAULT 0
);


CREATE TABLE Sections (
    section_id INT IDENTITY(1,1) PRIMARY KEY,
    class_id INT NULL,
    section_name NVARCHAR(10) NOT NULL,
    capacity INT DEFAULT 0,
    CONSTRAINT FK_sections_classes FOREIGN KEY (class_id) REFERENCES Classes(class_id)
);

ALTER TABLE Sections
ADD department_id INT NULL,
CONSTRAINT FK_sections_departments FOREIGN KEY (department_id) REFERENCES Departments(department_id);


Select * From Sections
Select * from Departments
select * From Students
select * from Teachers
select * from AspNetUsers
select *from Subjects
--create sp of Sections

CREATE PROCEDURE spAddSection
    @class_id INT,
    @section_name NVARCHAR(10),
    @capacity INT,
    @department_id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if a section with same name and same department already exists
    IF EXISTS (
        SELECT 1 
        FROM Sections
        WHERE section_name = @section_name
          AND department_id = @department_id
    )
    BEGIN
        SELECT 'This department already has a section with this name' AS Message;
        RETURN;
    END

    -- Insert if not exists
    INSERT INTO Sections(class_id, section_name, capacity, department_id)
    VALUES(@class_id, @section_name, @capacity, @department_id);

    SELECT 'Section added successfully' AS Message;
END

CREATE PROCEDURE spGetSections
AS
BEGIN
	SELECT * FROM Sections
END

--get sections by department_id
CREATE PROCEDURE spGetSectionsByDepartmentId 
	@department_id INT
AS
BEGIN
	Select * FROM Sections WHERE department_id = @department_id;
END
--END section SP


CREATE TABLE Teachers (
    teacher_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id NVARCHAR(450) NULL,  -- AspNetUsers.Id is NVARCHAR(450)
    teacher_code NVARCHAR(50),
    first_name NVARCHAR(80),
    last_name NVARCHAR(80),
    department_id INT NULL,
    contact NVARCHAR(50),
    hire_date DATE,
    CONSTRAINT FK_teachers_users FOREIGN KEY (user_id) REFERENCES AspNetUsers(Id),
    CONSTRAINT FK_teachers_departments FOREIGN KEY (department_id) REFERENCES Departments(department_id)
);
ALTER TABLE Teachers
ADD photo NVARCHAR(255) NULL;

--Start SP Teachers
--Create Teacher SP
CREATE PROCEDURE spAddTeacher
    @user_id NVARCHAR(450),
    @teacher_code NVARCHAR(50),
    @first_name NVARCHAR(80),
    @last_name NVARCHAR(80),
    @department_id INT,
    @contact NVARCHAR(80),
    @hire_date DATE,
    @photo NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insert into Teachers table
        INSERT INTO Teachers (user_id, teacher_code, first_name, last_name, department_id, contact, hire_date, photo)
        VALUES (@user_id, @teacher_code, @first_name, @last_name, @department_id, @contact, @hire_date, @photo);

        -- Optional: remove old role mapping (if needed)
        DELETE FROM AspNetUserRoles WHERE UserId = @user_id;

        -- Assign the 'Teacher' role (ensure this ID exists in AspNetRoles)
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@user_id, 2);  -- 2 = Teacher role id

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

--get teachers list
CREATE PROCEDURE spGetTeachers
AS
BEGIN
	SELECT * FROM Teachers
END;

--get all teachers with join tables
CREATE PROCEDURE spGetAllTeachers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        t.teacher_id,
        t.teacher_code,
        t.first_name,
        t.last_name,
        t.department_id,
        d.name AS department_name,
        t.contact,
        t.hire_date,
        t.photo,
        u.Id AS user_id,
        u.UserName,
        u.Email,
        u.PhoneNumber
    FROM Teachers AS t
    INNER JOIN AspNetUsers AS u ON t.user_id = u.Id
    LEFT JOIN Departments AS d ON t.department_id = d.department_id;
END



--get teachers by Id sp join with Users and department table
CREATE PROCEDURE spGetTeacherById
    @teacher_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        t.teacher_id,
        t.teacher_code,
        t.first_name,
        t.last_name,
        t.department_id,
        d.name AS department_name,
        t.contact,
        t.hire_date,
        t.photo,
        u.Id AS user_id,
        u.UserName,
        u.Email,
        u.PhoneNumber
    FROM Teachers AS t
    INNER JOIN AspNetUsers AS u ON t.user_id = u.Id
    LEFT JOIN Departments AS d ON t.department_id = d.department_id
    WHERE t.teacher_id = @teacher_id;
END

CREATE PROCEDURE spGetTeacherByUserId
    @user_id VARCHAR(225)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        t.teacher_id,
        t.teacher_code,
        t.first_name,
        t.last_name,
        t.department_id,
        d.name AS department_name,
        t.contact,
        t.hire_date,
        t.photo,
        u.Id AS user_id,
        u.UserName,
        u.Email,
        u.PhoneNumber
    FROM Teachers AS t
    INNER JOIN AspNetUsers AS u ON t.user_id = u.Id
    LEFT JOIN Departments AS d ON t.department_id = d.department_id
    WHERE t.user_id = @user_id;
END


select * From AspNetUserRoles


CREATE TABLE Students (
    student_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id NVARCHAR(450) NULL,
    student_number NVARCHAR(50) UNIQUE,
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    dob DATE,
    gender CHAR(1) CHECK (gender IN ('M','F','O')),
    photo NVARCHAR(255),
    admission_year INT,
    current_class_id INT NULL,
    current_section_id INT NULL,
    contact NVARCHAR(50),
    address NVARCHAR(MAX),
    CONSTRAINT FK_students_users FOREIGN KEY (user_id) REFERENCES AspNetUsers(Id),
    CONSTRAINT FK_students_classes FOREIGN KEY (current_class_id) REFERENCES Classes(class_id),
    CONSTRAINT FK_students_sections FOREIGN KEY (current_section_id) REFERENCES Sections(section_id)
);


Select * FROM AspNetUsers


--Student SP
--Add student
CREATE PROCEDURE spAddStudent
    @user_id NVARCHAR(450) = NULL,
    @first_name NVARCHAR(100),
    @last_name NVARCHAR(100),
    @dob DATE,
    @gender CHAR(1),
    @photo NVARCHAR(255),
    @admission_year INT,
    @current_class_id INT = NULL,
    @current_section_id INT = NULL,
    @address NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @last_number NVARCHAR(50);
    DECLARE @next_number NVARCHAR(50);
    DECLARE @serial INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- ✅ Get last student_number for this admission year
        SELECT TOP 1 @last_number = student_number
        FROM Students
        WHERE LEFT(student_number, 4) = CAST(@admission_year AS NVARCHAR(4))
        ORDER BY student_number DESC;

        -- ✅ Determine next serial number
        IF @last_number IS NULL
            SET @serial = 1;  -- first student of the year
        ELSE
            SET @serial = CAST(RIGHT(@last_number, 4) AS INT) + 1;

        -- ✅ Build next student_number (e.g., 20250012)
        SET @next_number = CONCAT(@admission_year, RIGHT('0000' + CAST(@serial AS NVARCHAR(4)), 4));

        -- ✅ Insert new student
        INSERT INTO Students (
            user_id, student_number, first_name, last_name, dob, gender, photo,
            admission_year, current_class_id, current_section_id, address
        )
        VALUES (
            @user_id, @next_number, @first_name, @last_name, @dob, @gender, @photo,
            @admission_year, @current_class_id, @current_section_id, @address
        );

        -- ✅ Update role if user exists
        IF @user_id IS NOT NULL
        BEGIN
            UPDATE AspNetUserRoles
            SET RoleId = 3  -- 3 = Student
            WHERE UserId = @user_id;
        END

		--update the UserName
		IF @user_id IS NOT NULL
		BEGIN
			UPDATE AspNetUsers
			SET UserName = @next_number
			WHERE Id = @user_id
		END
		
        COMMIT TRANSACTION;

        -- ✅ Return generated student_number
        SELECT @next_number AS student_number;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;

select*FROM AspNetUsers
--get all students informations including others tables
CREATE PROCEDURE spGetAllStudents
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.student_id,
        s.student_number,
        s.first_name,
        s.last_name,
        s.dob,
        s.gender,
        s.photo,
        s.admission_year,
        s.address,
		--class
        c.class_id,
        c.class_name,
		--section
        sec.section_id,
        sec.section_name,
        
        u.Id AS user_id,
        u.UserName,
        u.Email,
        u.PhoneNumber
    FROM Students s
    LEFT JOIN AspNetUsers u ON s.user_id = u.Id
    LEFT JOIN Classes c ON s.current_class_id = c.class_id
    LEFT JOIN Sections sec ON s.current_section_id = sec.section_id
    ORDER BY s.admission_year DESC, s.student_number ASC;
END
spGetAllStudents
spGetStudentById 1


--get student by Id
CREATE PROCEDURE spGetStudentById 
    @student_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.student_id,
        s.student_number,
        s.first_name,
        s.last_name,
        s.dob,
        s.gender,
        s.photo,
        s.admission_year,
        s.address,
		--class
        c.class_id,
        c.class_name,
		--section
        sec.section_id,
        sec.section_name,
        
        u.Id AS user_id,
        u.UserName,
        u.Email,
        u.PhoneNumber
    FROM Students s
    LEFT JOIN AspNetUsers u ON s.user_id = u.Id
    LEFT JOIN Classes c ON s.current_class_id = c.class_id
    LEFT JOIN Sections sec ON s.current_section_id = sec.section_id
    WHERE s.student_id = @student_id;
END

CREATE drop PROCEDURE  spGetStudentByUserId
    @user_id VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        s.student_id,
        s.student_number,
        s.first_name,
        s.last_name,
        s.dob,
        s.gender,
        s.photo,
        s.admission_year,
        s.address,
		--class
        c.class_id,
        c.class_name,
		--section
        sec.section_id,
        sec.section_name,
        
        u.Id AS user_id,
        u.UserName,
        u.Email,
        u.PhoneNumber
    FROM Students s
    LEFT JOIN AspNetUsers u ON s.user_id = u.Id
    LEFT JOIN Classes c ON s.current_class_id = c.class_id
    LEFT JOIN Sections sec ON s.current_section_id = sec.section_id
    WHERE s.user_id = @user_id;
END
select * from AspNetUsers
--updated later
CREATE PROCEDURE spGetStudentByUserId '9ecbb0f2-0914-4ab8-8d47-5e20b168d8f7'
    @user_id VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        -- Student
        s.student_id,
        s.student_number,
        s.first_name,
        s.last_name,
        s.dob,
        s.gender,
        s.photo,
        s.admission_year,
        s.address,

        -- Class (current)
        c.class_id,
        c.class_name,

        -- Section (current)
        sec.section_id,
        sec.section_name,

        -- User
        u.Id AS user_id,
        u.UserName,
        u.Email,
        u.PhoneNumber,

        -- ✅ Enrollment info (ADDED AT END)
        e.enrollment_id,
        e.year_id,
        ay.year_label

    FROM Students s
    LEFT JOIN AspNetUsers u 
        ON s.user_id = u.Id

    -- 🔥 Current enrollment (active academic year)
    LEFT JOIN Enrollments e 
        ON e.student_id = s.student_id

    LEFT JOIN Academic_years ay 
        ON e.year_id = ay.year_id
       AND ay.is_active = 1

    LEFT JOIN Classes c 
        ON s.current_class_id = c.class_id

    LEFT JOIN Sections sec 
        ON s.current_section_id = sec.section_id

    WHERE s.user_id = @user_id
      AND ay.is_active = 1;
END;
GO


CREATE TABLE ClassSubjects (
    class_subject_id INT IDENTITY(1,1) PRIMARY KEY,
    class_id INT NOT NULL,
    subject_id INT NOT NULL,
    is_mandatory BIT DEFAULT 1,
    CONSTRAINT UQ_Class_Subject UNIQUE (class_id, subject_id),
    CONSTRAINT FK_class_subjects_classes FOREIGN KEY (class_id) REFERENCES Classes(class_id),
    CONSTRAINT FK_class_subjects_subjects FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id)
);

-- ====================================================
-- ENROLLMENTS
-- ====================================================
CREATE TABLE Enrollments (
    enrollment_id INT IDENTITY(1,1) PRIMARY KEY,
    student_id INT NOT NULL,
    year_id INT NOT NULL,
    class_id INT NULL,
    section_id INT NULL,
    roll_no INT,
    admission_date DATE,
    status NVARCHAR(20),
    CONSTRAINT UQ_Student_Year UNIQUE (student_id, year_id),
    CONSTRAINT FK_enrollments_students FOREIGN KEY (student_id) REFERENCES Students(student_id),
    CONSTRAINT FK_enrollments_years FOREIGN KEY (year_id) REFERENCES Academic_years(year_id),
    CONSTRAINT FK_enrollments_classes FOREIGN KEY (class_id) REFERENCES Classes(class_id),
    CONSTRAINT FK_enrollments_sections FOREIGN KEY (section_id) REFERENCES Sections(section_id)
);

--sp for enrollments
select* from Enrollments
select * from Academic_years
select * from Sections

--get Enrollments by year_id, class_id, Section_id
CREATE PROCEDURE spGetEnrollments
    @year_id INT = NULL,
    @class_id INT = NULL,
    @section_id INT = NULL,
	@status NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        e.enrollment_id,
        e.student_id,
        s.first_name,
        s.last_name,
        s.student_number,
        e.year_id,
        e.class_id,
        e.section_id,
        e.status
    FROM Enrollments e
    INNER JOIN Students s ON e.student_id = s.student_id
    WHERE 
        (@year_id IS NULL OR e.year_id = @year_id)
        AND (@class_id IS NULL OR e.class_id = @class_id)
        AND (@section_id IS NULL OR e.section_id = @section_id)
        AND (@status IS NULL OR e.status = @status);
END
GO


--add enrollment sp
CREATE PROCEDURE spAddEnrollment
    @student_id INT,
    @year_id INT,
    @class_id INT = NULL,
    @section_id INT = NULL,
    @roll_no INT = NULL,
    @admission_date DATE,
    @status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- ❌ Already enrolled for same year
        IF EXISTS (
            SELECT 1 FROM Enrollments
            WHERE student_id = @student_id AND year_id = @year_id
        )
        BEGIN
            RAISERROR('Student is already enrolled for this academic year.', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        -- ❌ Check Section Capacity
        IF @section_id IS NOT NULL
        BEGIN
            DECLARE @capacity INT = (SELECT capacity FROM Sections WHERE section_id = @section_id);

            DECLARE @currentCount INT = (
                SELECT COUNT(*)
                FROM Enrollments
                WHERE section_id = @section_id AND year_id = @year_id
            );

            IF @currentCount >= @capacity
            BEGIN
                RAISERROR('This section is full. Enrollment not allowed.', 16, 1);
                ROLLBACK TRANSACTION;
                RETURN;
            END;
        END;

        -- ✅ Insert Enrollment
        INSERT INTO Enrollments (
            student_id, year_id, class_id, section_id, admission_date, status
        )
        VALUES (
            @student_id, @year_id, @class_id, @section_id, @admission_date, @status
        );

        DECLARE @enrollmentId INT = SCOPE_IDENTITY();

        -- ✅ Update student's current class & section
        UPDATE Students
        SET current_class_id = @class_id,
            current_section_id = @section_id
        WHERE student_id = @student_id;

        -- ============================================
		-- ✅ Auto insert mandatory subjects (with department match)
		-- ============================================
		INSERT INTO StudentSubjects (enrollment_id, subject_id, is_mandatory)
		SELECT 
		    @enrollmentId, 
		    cs.subject_id, 
		    1
		FROM ClassSubjects cs
		JOIN Subjects s 
		    ON cs.subject_id = s.subject_id
		JOIN Sections sec
		    ON sec.section_id = @section_id
		WHERE 
		    cs.class_id = @class_id
		    AND cs.is_mandatory = 1
		    AND s.department_id = sec.department_id; 

        -- ============================================
        -- ✅ Auto insert mandatory subjects for dept 5
        -- (avoids duplicates)
        -- ============================================
        INSERT INTO StudentSubjects (enrollment_id, subject_id, is_mandatory)
        SELECT @enrollmentId, cs.subject_id, 1
        FROM ClassSubjects cs
        JOIN Subjects s ON cs.subject_id = s.subject_id
        WHERE cs.class_id = @class_id
          AND cs.is_mandatory = 1
          AND s.department_id = 5
          AND NOT EXISTS (
                SELECT 1 FROM StudentSubjects 
                WHERE enrollment_id = @enrollmentId 
                  AND subject_id = cs.subject_id
        );

        COMMIT TRANSACTION;

        SELECT @enrollmentId AS enrollment_id;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;


select * from Subjects
select * from ClassSubjects


Select * from Students
select * from Classes
select * from Sections

-- ====================================================
-- STUDENT_SUBJECTS
-- ====================================================
CREATE TABLE StudentSubjects (
    student_subject_id INT IDENTITY(1,1) PRIMARY KEY,
    enrollment_id INT NOT NULL,
    subject_id INT NOT NULL,
    is_optional BIT DEFAULT 0,
    is_mandatory BIT DEFAULT 0,
    CONSTRAINT UQ_Enrollment_Subject UNIQUE (enrollment_id, subject_id),
    CONSTRAINT FK_student_subjects_enrollments FOREIGN KEY (enrollment_id) REFERENCES Enrollments(enrollment_id),
    CONSTRAINT FK_student_subjects_subjects FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id)
);
GO
select * from ClassSubjects
select * from Students
select * from StudentSubjects
select * from Enrollments


-- ====================================================
-- INDEXES
-- ====================================================
CREATE INDEX idx_student_number ON Students(student_number);
CREATE INDEX idx_enrollment_year_class ON Enrollments(year_id, class_id);
CREATE INDEX idx_attendance_date ON AttendanceSessions(session_date);
GO


-- upto this are created
-- code for procedure
--Departments start
--get all departments
CREATE PROCEDURE spGetDepartments
AS
BEGIN
	SELECT * FROM Departments;
END;

--get Departments by department_id
CREATE PROCEDURE spGetDepartmentById
	@department_id INT
AS
BEGIN
	SELECT * FROM Departments Where department_id = @department_id;
END;

Exec spGetDepartmentById @department_id = 2;

--Add new Department
CREATE PROCEDURE spAddDepartment
	@name NVARCHAR(100),
	@code NVARCHAR(20),
	@description NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO Departments(name, code, description) VALUES(@name, @code, @description); 
END

EXEC spAddDepartment
	@name = 'Computer Science and Engineering',
    @code = 'CSE',
    @description = 'The Computer Science department focuses on programming and systems.';


--update department
CREATE PROCEDURE spUpdateDepartment
	@department_id INT,
	@name NVARCHAR(100),
	@code NVARCHAR(20),
	@description NVARCHAR(MAX)
AS
BEGIN
	UPDATE Departments set name = @name, code = @code, description = @description WHERE department_id = @department_id;
END
use School_SMS_DB
Select*From Classes;

--Departments END

--Classes Start (Procedure)

--Get all classes
CREATE PROCEDURE spGetClasses
AS
BEGIN
	SELECT * FROM Classes;
END

--get class by Id
CREATE PROCEDURE spGetClassById
	@class_id INT
AS
BEGIN
	SELECT * FROM Classes WHERE class_id = @class_id;
END

--Update Class
CREATE PROCEDURE spUpdateClass
	@class_id INT,
	@class_name NVARCHAR(20),
	@is_secondary BIT,
	@description NVARCHAR(255)
AS
BEGIN
	UPDATE Classes SET class_name = @class_name, is_secondary = @is_secondary, description = @description WHERE class_id = @class_id;
END;


--Add Class
CREATE PROCEDURE spAddClass
	@class_name NVARCHAR(20),
	@is_secondary BIT,
	@description NVARCHAR(255)
AS
BEGIN
	INSERT INTO Classes(class_name, is_secondary, description) VALUES(@class_name, @is_secondary, @description);
END;

--Classes Procedure end

--Subject Procedure Start
Select * From Subjects
--create Subject
CREATE PROCEDURE spAddSubject
	
	@subject_code NVARCHAR(30),
	@name NVARCHAR(150),
	@is_theory BIT,
	@is_practical BIT,
	@default_marks INT,
	@department_id INT,
	@credit_hours INT
AS
BEGIN
	INSERT INTO Subjects(subject_code, name, is_theory, is_practical, default_marks, department_id, credit_hours)
	VALUES(@subject_code, @name, @is_theory, @is_practical, @default_marks, @department_id, @credit_hours);
END

--get subject
CREATE PROCEDURE spGetSubjects
AS
BEGIN
	SELECT * FROM Subjects;
END

--get subject by ID
CREATE PROCEDURE spGetSubjectById
	@subject_id INT
AS
BEGIN
	SELECT *FROM Subjects WHERE subject_id = @subject_id;
END
--END procedure of subject
Select * from Subjects Where subject_id = 6

--Procedure for ClassSubjects start 
--first need to create a table that use as parameter
--must need this way to create the table
CREATE TYPE SubjectIdList AS TABLE (
    subject_id INT
);

--Add many subject to 1 class
CREATE PROCEDURE spAddClassSubjects
	@class_id INT,
	@subject_list SubjectIdList READONLY
AS
BEGIN
	INSERT INTO ClassSubjects(class_id, subject_id, is_mandatory)
	SELECT @class_id, s.subject_id, 1
	FROM @subject_list s
	WHERE NOT EXISTS(
		SELECT 1 FROM ClassSubjects WHERE ClassSubjects.class_id = @class_id AND
		ClassSubjects.subject_id = s.subject_id
	)
END

Select * From SubjectIdList;
EXEC sp_helptext 'spAddClassSubjects';


select * From ClassSubjects
Select * From Classes
Select * From Subjects

--Get class and its subjects 
CREATE PROCEDURE spGetSubjectsByClassId 
	@class_id INT
AS
BEGIN
	SELECT 
		c.class_id,
		c.class_name,
		c.description AS class_description,
		s.subject_id,
		s.subject_code,
		s.name AS subject_name,
		s.is_theory,
		s.is_practical,
		s.default_marks,
		cs.is_mandatory,
		s.department_id,
		s.credit_hours
	FROM Classes c
	INNER JOIN ClassSubjects cs ON c.class_id = cs.class_id
	INNER JOIN Subjects s ON cs.subject_id = s.subject_id
	WHERE c.class_id = @class_id;
END

---End ClassSubjects procedure

--start academic_years PRocedure
CREATE PROCEDURE spAddAcademicYear
	@year_label NVARCHAR(30),
	@start_date Date,
	@end_date Date,
	@is_active BIT
AS
BEGIN
	INSERT INTO Academic_years(year_label, start_date, end_date, is_active) 
	Values(@year_label, @start_date, @end_date, @is_active);
END
select * from subjects

Select * From Academic_years

--Get all AcademicYears
CREATE PROCEDURE spGetAcademicYears
AS
BEGIN
	SELECT * FROM Academic_years
END


--ClassSlots
CREATE TABLE ClassSlots (
    slot_id INT IDENTITY(1,1) PRIMARY KEY,
    slot_number INT NOT NULL,           -- 1st period, 2nd period, etc.
    start_time VARCHAR(20) NOT NULL,    -- e.g., '10:00 AM'
    end_time VARCHAR(20) NOT NULL       -- e.g., '10:45 AM'
);

CREATE PROCEDURE spGetClassSlots
AS
BEGIN
	SELECT * FROM ClassSlots;
END
Select * from ClassSlots

--WeeklyDays Table
CREATE TABLE WeeklyDays (
    day_id INT IDENTITY(1,1) PRIMARY KEY,
    day_name NVARCHAR(20) NOT NULL,
    is_school_open BIT DEFAULT 1  -- 0 for Friday/Saturday
);

CREATE PROCEDURE spGetDays
AS
BEGIN
	SELECT * FROM WeeklyDays;
END

Select * from AttendanceRecords
Select * from AttendanceSessions
select * from ClassRoutine

Select * from WeeklyDays
select * from Academic_years
--ClassRoutine table
CREATE TABLE ClassRoutine (
    routine_id INT IDENTITY(1,1) PRIMARY KEY,

    year_id INT NOT NULL,   -- FK from Academic_years

    class_id INT NOT NULL,
    section_id INT NOT NULL,
    subject_id INT NOT NULL,
    teacher_id INT NOT NULL,
    day_id INT NOT NULL,
    slot_id INT NOT NULL,

    CONSTRAINT FK_routine_year FOREIGN KEY (year_id) REFERENCES Academic_years(year_id),
    CONSTRAINT FK_routine_class FOREIGN KEY (class_id) REFERENCES Classes(class_id),
    CONSTRAINT FK_routine_section FOREIGN KEY (section_id) REFERENCES Sections(section_id),
    CONSTRAINT FK_routine_subject FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id),
    CONSTRAINT FK_routine_teacher FOREIGN KEY (teacher_id) REFERENCES Teachers(teacher_id),
    CONSTRAINT FK_routine_day FOREIGN KEY (day_id) REFERENCES WeeklyDays(day_id),
    CONSTRAINT FK_routine_slot FOREIGN KEY (slot_id) REFERENCES ClassSlots(slot_id),

    -- Prevent duplicate slot assignment for the same class-section in same academic year
    CONSTRAINT UQ_class_routine UNIQUE (year_id, class_id, section_id, day_id, slot_id)
);

select * From WeeklyDays
select * From ClassSlots
select * From ClassRoutine Where section_id =3
select * From Subjects
Select * From Teachers

--sp of adding ClassRoutine of a subject one by one
CREATE PROCEDURE spAddClassRoutine
    @year_id INT,
    @class_id INT,
    @section_id INT,
    @subject_id INT,
    @teacher_id INT,
    @day_id INT,
    @slot_id INT
AS
BEGIN
    SET NOCOUNT ON;

    ---------------------------------------------
    -- 1️⃣ Check: Teacher conflict
    ---------------------------------------------
    IF EXISTS (
        SELECT 1 
        FROM ClassRoutine
        WHERE year_id = @year_id
          AND teacher_id = @teacher_id
          AND day_id = @day_id
          AND slot_id = @slot_id
    )
    BEGIN
        RAISERROR ('❌ Conflict: This teacher already has a class in this time slot.', 16, 1);
        RETURN;
    END;


    ---------------------------------------------
    -- 2️⃣ Check: Section conflict
    ---------------------------------------------
    IF EXISTS (
        SELECT 1 
        FROM ClassRoutine
        WHERE year_id = @year_id
          AND section_id = @section_id
          AND day_id = @day_id
          AND slot_id = @slot_id
    )
    BEGIN
        RAISERROR ('❌ Conflict: This section already has a class in this time slot.', 16, 1);
        RETURN;
    END;


    ---------------------------------------------
    -- 3️⃣ Insert (No conflict)
    ---------------------------------------------
    INSERT INTO ClassRoutine
    (year_id, class_id, section_id, subject_id, teacher_id, day_id, slot_id)
    VALUES
    (@year_id, @class_id, @section_id, @subject_id, @teacher_id, @day_id, @slot_id);

    SELECT '✅ Class routine added successfully.' AS Message;
END;
GO

CREATE  PROCEDURE spGetRoutineByTeacher
    @teacher_id INT,
    @year_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        cr.routine_id,
        wy.day_name,
        cs.slot_number,
        cs.start_time,
        cs.end_time,
        s.name AS subject_name,
        c.class_name,
        sec.section_name,
		cr.year_id,
		cr.class_id,
		cr.section_id,
		s.subject_id

    FROM ClassRoutine cr
    JOIN WeeklyDays wy ON cr.day_id = wy.day_id
    JOIN ClassSlots cs ON cr.slot_id = cs.slot_id
    JOIN Subjects s ON cr.subject_id = s.subject_id
    JOIN Classes c ON cr.class_id = c.class_id
    JOIN Sections sec ON cr.section_id = sec.section_id
    WHERE cr.teacher_id = @teacher_id
      AND cr.year_id = @year_id
    ORDER BY wy.day_id, cs.slot_number;
END;
GO

CREATE PROCEDURE spGetRoutineByClassSection
    @year_id INT,
    @class_id INT,
    @section_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        cr.routine_id,
        wy.day_name,
        cs.slot_number,
        cs.start_time,
        cs.end_time,
        s.subject_id,
        s.name AS subject_name,
        c.class_id,
        c.class_name,
        sec.section_id,
        sec.section_name,
        cr.teacher_id,
        cr.year_id
    FROM ClassRoutine cr
    JOIN WeeklyDays wy ON cr.day_id = wy.day_id
    JOIN ClassSlots cs ON cr.slot_id = cs.slot_id
    JOIN Subjects s ON cr.subject_id = s.subject_id
    JOIN Classes c ON cr.class_id = c.class_id
    JOIN Sections sec ON cr.section_id = sec.section_id
    WHERE cr.year_id = @year_id
      AND cr.class_id = @class_id
      AND cr.section_id = @section_id
    ORDER BY wy.day_id, cs.slot_number;
END;
GO



select * from AttendanceRecords
select * from AttendanceSessions




-- ====================================================
-- ATTENDANCE_SESSIONS
-- ====================================================
CREATE TABLE AttendanceSessions (
    session_id INT IDENTITY(1,1) PRIMARY KEY,
    routine_id INT NOT NULL,                 -- 🔥 From ClassRoutine
    session_date DATE NOT NULL,              -- Teacher takes attendance on this date
    created_at DATETIME DEFAULT GETDATE(),

    -- Prevent duplicate attendance for same routine on same day
    CONSTRAINT UQ_routine_session UNIQUE (routine_id, session_date),

    CONSTRAINT FK_attendance_sessions_routine 
        FOREIGN KEY (routine_id) REFERENCES ClassRoutine(routine_id)
);
GO


-- ====================================================
-- ATTENDANCE_RECORDS
-- ====================================================
CREATE TABLE AttendanceRecords (
    record_id INT IDENTITY(1,1) PRIMARY KEY,
    session_id INT NOT NULL,
    enrollment_id INT NOT NULL,        -- identifies the student
    status NVARCHAR(10) NOT NULL,      -- Present / Absent / Late
    remarks NVARCHAR(255),

    recorded_at DATETIME NOT NULL DEFAULT GETDATE(),   -- initial entry timestamp
    updated_at DATETIME NULL,                          -- timestamp when updated later

    -- Prevent duplicate attendance for one student in one session
    CONSTRAINT UQ_session_student UNIQUE (session_id, enrollment_id),

    CONSTRAINT FK_attendance_records_sessions 
        FOREIGN KEY (session_id) REFERENCES AttendanceSessions(session_id),

    CONSTRAINT FK_attendance_records_enrollments 
        FOREIGN KEY (enrollment_id) REFERENCES Enrollments(enrollment_id)
);
GO



--User define data table to handle the Attendance List
CREATE TYPE AttendanceInput AS TABLE
(
    enrollment_id INT,
    status NVARCHAR(10),
    remarks NVARCHAR(255) NULL
);
GO
-- Take attendace for section
CREATE PROCEDURE spTakeAttendanceForSection
(
    @routine_id INT,
    @session_date DATE,
    @AttendanceList AttendanceInput READONLY
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @session_id INT;

    -----------------------------------------------------
    -- 1️⃣ Check if session already exists for today
    -----------------------------------------------------
    SELECT @session_id = session_id
    FROM AttendanceSessions
    WHERE routine_id = @routine_id
      AND session_date = @session_date;

    IF @session_id IS NOT NULL
    BEGIN
        -- 🔥 Return message and stop execution
        SELECT 
            'Attendance already taken for this routine on this date.' AS message,
            @session_id AS session_id;

        RETURN;
    END


    -----------------------------------------------------
    -- 2️⃣ Create a new attendance session
    -----------------------------------------------------
    INSERT INTO AttendanceSessions (routine_id, session_date)
    VALUES (@routine_id, @session_date);

    SET @session_id = SCOPE_IDENTITY();


    -----------------------------------------------------
    -- 3️⃣ Insert records using MERGE
    -----------------------------------------------------
    MERGE AttendanceRecords AS Target
    USING @AttendanceList AS Source
        ON Target.session_id = @session_id
       AND Target.enrollment_id = Source.enrollment_id
    WHEN MATCHED THEN
        UPDATE SET
            status = Source.status,
            remarks = Source.remarks,
            updated_at = GETDATE()
    WHEN NOT MATCHED THEN
        INSERT (session_id, enrollment_id, status, remarks)
        VALUES (@session_id, Source.enrollment_id, Source.status, Source.remarks);


    -----------------------------------------------------
    -- 4️⃣ Return success message
    -----------------------------------------------------
    SELECT 
        'Attendance recorded successfully.' AS message,
        @session_id AS session_id;

END
GO
--get attendance summary of a section and subject
CREATE PROCEDURE spGetAttendanceSummary 1, 11, 3, 9
    @year_id INT,
    @class_id INT,
    @section_id INT,
    @subject_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        e.enrollment_id,
        s.student_id,
        s.student_number,
        e.year_id,
        e.class_id,
        e.section_id,
        @subject_id AS subject_id,

        s.first_name,
        s.last_name,

        -- Total Classes (all AttendanceRecords)
        COUNT(ar.record_id) AS total_classes,

        -- Present = Present + Late
        SUM(CASE WHEN ar.status IN ('Present', 'Late') THEN 1 ELSE 0 END) AS total_present,

        -- Absent only
        SUM(CASE WHEN ar.status = 'Absent' THEN 1 ELSE 0 END) AS total_absent,

        -- Attendance Percentage
        CASE 
            WHEN COUNT(ar.record_id) = 0 THEN 0
            ELSE (
                SUM(CASE WHEN ar.status IN ('Present', 'Late') THEN 1 ELSE 0 END) * 100.0 
                / COUNT(ar.record_id)
            )
        END AS attendance_percentage

    FROM Enrollments e
    JOIN Students s 
        ON e.student_id = s.student_id

    JOIN ClassRoutine cr
        ON cr.year_id = e.year_id
       AND cr.class_id = e.class_id
       AND cr.section_id = e.section_id
       AND cr.subject_id = @subject_id

    LEFT JOIN AttendanceSessions ats
        ON ats.routine_id = cr.routine_id

    LEFT JOIN AttendanceRecords ar
        ON ar.session_id = ats.session_id
       AND ar.enrollment_id = e.enrollment_id

    WHERE 
        e.year_id = @year_id
        AND e.class_id = @class_id
        AND e.section_id = @section_id

    GROUP BY 
        e.enrollment_id,
        s.student_id,
        s.student_number,
        e.year_id,
        e.class_id,
        e.section_id,
        s.first_name,
        s.last_name

    ORDER BY 
        s.student_number;
END;
GO

--get student a subject attendance
CREATE PROCEDURE spGetStudentAttendanceDetails 7, 1, 9
    @enrollment_id INT,
    @year_id INT,
    @subject_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Attendance Details
    SELECT
        ar.record_id,
        ats.session_id,
        ats.session_date,
        ar.status,
        ar.remarks,
        ar.recorded_at,
        ar.updated_at,

        cr.subject_id,
        cr.class_id,
        cr.section_id,
        cr.teacher_id,

        s.student_id,
        s.student_number,
        s.first_name,
        s.last_name

    FROM AttendanceRecords ar
    JOIN AttendanceSessions ats
        ON ar.session_id = ats.session_id

    JOIN ClassRoutine cr
        ON cr.routine_id = ats.routine_id
        AND cr.subject_id = @subject_id
        AND cr.year_id = @year_id

    JOIN Enrollments e
        ON ar.enrollment_id = e.enrollment_id
        AND e.enrollment_id = @enrollment_id

    JOIN Students s
        ON e.student_id = s.student_id

    ORDER BY ats.session_date ASC;
END;
GO


select * from AttendanceRecords
select * from AttendanceSessions

CREATE PROCEDURE spUpdateAttendanceRecord 
    @record_id INT,
    @status NVARCHAR(10),
    @updated_at DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if record exists
    IF NOT EXISTS (SELECT 1 FROM AttendanceRecords WHERE record_id = @record_id)
    BEGIN
        SELECT 'Record not found' AS Message, 0 AS Success;
        RETURN;
    END

    -- Update the record
    UPDATE AttendanceRecords
    SET 
        status = @status,
      
        updated_at = @updated_at
    WHERE record_id = @record_id;

    SELECT 'Attendance updated successfully' AS Message, 1 AS Success;
END;
GO


--ALL about result module


CREATE TABLE Grades (
    grade_id INT IDENTITY(1,1) PRIMARY KEY,
    grade_name NVARCHAR(5) NOT NULL,        -- A+, A, B-, etc.
    min_mark INT NOT NULL,                 -- Minimum mark range
    max_mark INT NOT NULL,                 -- Maximum mark range
    grade_point DECIMAL(3,2) NULL          -- Optional (can be NULL)
);

INSERT INTO Grades (grade_name, min_mark, max_mark, grade_point)
VALUES
('A+', 80, 100, 5.00),
('A', 75, 79, 4.50),
('A-', 70, 74, 4.00),
('B+', 65, 69, 3.50),
('B', 60, 64, 3.25),
('B-', 55, 59, 3.00),
('C+', 50, 54, 2.75),
('C', 45, 49, 2.50),
('D', 40, 44, 2.00),
('F', 0, 39, 0.00);
select * from Grades Where 50 between min_mark AND max_mark

CREATE TABLE ExamTypes (
    exam_type_id INT IDENTITY(1,1) PRIMARY KEY,
    type_name NVARCHAR(50) NOT NULL,     -- Mid Term, Final, Assignment, etc
    weight_percentage DECIMAL(5,2) NOT NULL -- e.g., 30.00, 45.00, 10.00
);

INSERT INTO ExamTypes (type_name, weight_percentage)
VALUES
('Mid Term', 30.00),
('Final Exam', 45.00),
('Assignment', 10.00),
('Quiz', 10.00),
('Attendance', 5.00);
select * from ExamTypes

CREATE PROCEDURE spGetExamTypes
AS
BEGIN
	SELECT * FROM ExamTypes;
END

--ExamSlots
CREATE TABLE ExamSlots (
    exam_slot_id INT IDENTITY(1,1) PRIMARY KEY,
    exam_slot_name NVARCHAR(50) NOT NULL,       -- Morning, Afternoon, etc.
    exam_start_time VARCHAR(10) NOT NULL,
    exam_end_time VARCHAR(10) NOT NULL,

    CONSTRAINT UQ_exam_slot UNIQUE (exam_start_time, exam_end_time)
);

INSERT INTO ExamSlots (exam_slot_name, exam_start_time, exam_end_time)
VALUES
('Morning', '10:00 AM', '01:00 PM'),
('Afternoon', '02:00 PM', '05:00 PM');
select * from ExamSlots

CREATE PROCEDURE spGetExamSlots
AS
BEGIN
	SELECT * FROM ExamSlots
END


CREATE TABLE ExamSessions (
    exam_session_id INT IDENTITY(1,1) PRIMARY KEY,
    year_id INT NOT NULL,                -- academic year
    exam_type_id INT NOT NULL,           -- Mid Term, Final, Assignment
    subject_id INT NOT NULL,
    class_id INT NOT NULL,
    section_id INT NOT NULL,
    exam_date DATE,
    max_marks DECIMAL(5,2) NOT NULL,     -- 100, 50, etc
    CONSTRAINT FK_exam_year FOREIGN KEY (year_id) REFERENCES Academic_years(year_id),
    CONSTRAINT FK_exam_type FOREIGN KEY (exam_type_id) REFERENCES ExamTypes(exam_type_id),
    CONSTRAINT FK_exam_subject FOREIGN KEY (subject_id) REFERENCES Subjects(subject_id)
);

ALTER TABLE ExamSessions
ADD exam_slot_id INT NOT NULL;

ALTER TABLE ExamSessions
ADD CONSTRAINT FK_exam_slot
FOREIGN KEY (exam_slot_id) REFERENCES ExamSlots(exam_slot_id);

CREATE OR ALTER PROCEDURE spGetExamSessions
(
    @exam_session_id INT = NULL,
    @year_id INT = NULL,
    @exam_type_id INT = NULL,
    @subject_id INT = NULL,
    @class_id INT = NULL,
    @section_id INT = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        es.exam_session_id,
        es.year_id,
        es.exam_type_id,
        et.type_name AS exam_type_name,
        es.subject_id,
        s.name AS subject_name,
        es.class_id,
        es.section_id,
        es.exam_date,
        es.max_marks,
		es.exam_slot_id

    FROM ExamSessions es
    INNER JOIN ExamTypes et ON es.exam_type_id = et.exam_type_id
    INNER JOIN Subjects s ON es.subject_id = s.subject_id
    WHERE 
        (@exam_session_id IS NULL OR es.exam_session_id = @exam_session_id)
        AND (@year_id IS NULL OR es.year_id = @year_id)
        AND (@exam_type_id IS NULL OR es.exam_type_id = @exam_type_id)
        AND (@subject_id IS NULL OR es.subject_id = @subject_id)
        AND (@class_id IS NULL OR es.class_id = @class_id)
        AND (@section_id IS NULL OR es.section_id = @section_id)
    ORDER BY es.exam_session_id DESC;
END;
GO


select * from classes
select * from Sections
select * from Subjects

select * from ExamSessions

--SP to add Exam Session
CREATE PROCEDURE spAddExamSession
    @year_id INT,
    @exam_type_id INT,
    @subject_id INT,
    @class_id INT,
    @section_id INT,
    @exam_date DATE,
    @exam_slot_id INT,
    @max_marks DECIMAL(5,2)
AS
BEGIN
    SET NOCOUNT ON;

    --------------------------------------------------------
    -- 1. Check: Same exam for same year/class/subject exists
    --------------------------------------------------------
    IF EXISTS (
        SELECT 1
        FROM ExamSessions
        WHERE year_id = @year_id
          AND exam_type_id = @exam_type_id
          AND subject_id = @subject_id
          AND class_id = @class_id
          AND section_id = @section_id
    )
    BEGIN
        SELECT 
            0 AS success,
            'This exam session already exists for this subject & exam type.' AS message;
        RETURN;
    END


    --------------------------------------------------------
    -- 2. Check Time Slot Overlap for same date same class-section
    --------------------------------------------------------
    DECLARE @new_start TIME, @new_end TIME;

    SELECT 
        @new_start = CONVERT(TIME, exam_start_time),
        @new_end   = CONVERT(TIME, exam_end_time)
    FROM ExamSlots
    WHERE exam_slot_id = @exam_slot_id;


    IF EXISTS (
        SELECT 1
        FROM ExamSessions es
        JOIN ExamSlots s ON es.exam_slot_id = s.exam_slot_id
        WHERE es.exam_date = @exam_date
          AND es.class_id = @class_id
          AND es.section_id = @section_id
          AND (
                CONVERT(TIME, s.exam_start_time) < @new_end AND
                @new_start < CONVERT(TIME, s.exam_end_time)
              )
    )
    BEGIN
        SELECT 
            0 AS success,
            'Another exam is already scheduled for this class/section at this time.' AS message;
        RETURN;
    END


    --------------------------------------------------------
    -- 3. Insert Exam Session
    --------------------------------------------------------
    INSERT INTO ExamSessions
        (year_id, exam_type_id, subject_id, class_id, section_id, exam_date, exam_slot_id, max_marks)
    VALUES
        (@year_id, @exam_type_id, @subject_id, @class_id, @section_id, @exam_date, @exam_slot_id, @max_marks);


    SELECT 
        1 AS success,
        'Exam session added successfully.' AS message,
        SCOPE_IDENTITY() AS exam_session_id;
END;
GO

--get exam session by year_id, class_id, Section_id, subject_id




CREATE TABLE ExamResults (
    result_id INT IDENTITY(1,1) PRIMARY KEY,
    exam_session_id INT NOT NULL,
    enrollment_id INT NOT NULL,          -- student
    obtained_marks DECIMAL(5,2) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME NULL,

    CONSTRAINT FK_result_session FOREIGN KEY (exam_session_id) REFERENCES ExamSessions(exam_session_id),
    CONSTRAINT FK_result_enrollment FOREIGN KEY (enrollment_id) REFERENCES Enrollments(enrollment_id),

    CONSTRAINT UQ_exam_student UNIQUE (exam_session_id, enrollment_id)
);

select * from ExamResults
CREATE PROCEDURE spGetExamResult
    @exam_session_id INT,
    @enrollment_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        result_id,
        exam_session_id,
        enrollment_id,
        obtained_marks,
        created_at,
        updated_at
    FROM ExamResults
    WHERE exam_session_id = @exam_session_id
      AND enrollment_id = @enrollment_id;
END;

select * from AspNetUsers

CREATE PROCEDURE spAddExamResult
(
    @exam_session_id INT,
    @enrollment_id INT,
    @obtained_marks DECIMAL(5,2)
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if result already exists for this student in this exam session
    IF EXISTS (
        SELECT 1 FROM ExamResults
        WHERE exam_session_id = @exam_session_id
          AND enrollment_id = @enrollment_id
    )
    BEGIN
        SELECT 
            'conflict' AS status,
            'Result already exists for this student in this exam session.' AS message;
        RETURN;
    END

    -- Insert new exam result
    INSERT INTO ExamResults
    (
        exam_session_id,
        enrollment_id,
        obtained_marks
    )
    VALUES
    (
        @exam_session_id,
        @enrollment_id,
        @obtained_marks
    );

    SELECT 
        'success' AS status,
        'Exam result added successfully.' AS message,
        SCOPE_IDENTITY() AS result_id;
END;

select * from Students
GO
select * from Departments
--*** updatedone
CREATE drop PROCEDURE spGetStudentOverallResults 7
    @enrollment_id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @year_id INT;
    DECLARE @class_id INT;
    DECLARE @section_id INT;
    DECLARE @student_id INT;
    DECLARE @has_exam_sessions BIT = 0;
    DECLARE @has_results BIT = 0;
    DECLARE @all_results_published BIT = 0;
    
    -- Get enrollment details
    SELECT
        @year_id = year_id,
        @class_id = class_id,
        @section_id = section_id,
        @student_id = student_id
    FROM Enrollments
    WHERE enrollment_id = @enrollment_id;
    
    -- Check if any exam sessions exist for this enrollment
    IF EXISTS (
        SELECT 1 FROM ExamSessions es
        JOIN StudentSubjects ss ON es.subject_id = ss.subject_id
        WHERE ss.enrollment_id = @enrollment_id
          AND es.year_id = @year_id
          AND es.class_id = @class_id
          AND (@section_id IS NULL OR es.section_id = @section_id)
    )
    BEGIN
        SET @has_exam_sessions = 1;
    END
    
    -- Check if any exam results exist for this enrollment
    IF EXISTS (
        SELECT 1 FROM ExamResults er
        JOIN ExamSessions es ON er.exam_session_id = es.exam_session_id
        WHERE er.enrollment_id = @enrollment_id
    )
    BEGIN
        SET @has_results = 1;
    END
    
    -- Check if all exam sessions have results (results are published)
    DECLARE @total_sessions INT = 0;
    DECLARE @total_results INT = 0;
    
    SELECT @total_sessions = COUNT(DISTINCT es.exam_session_id)
    FROM ExamSessions es
    JOIN StudentSubjects ss ON es.subject_id = ss.subject_id
    WHERE ss.enrollment_id = @enrollment_id
      AND es.year_id = @year_id
      AND es.class_id = @class_id
      AND (@section_id IS NULL OR es.section_id = @section_id)
      
    
    SELECT @total_results = COUNT(DISTINCT er.exam_session_id)
    FROM ExamResults er
    JOIN ExamSessions es ON er.exam_session_id = es.exam_session_id
    WHERE er.enrollment_id = @enrollment_id
      
    
    IF @total_sessions > 0 AND @total_sessions = @total_results
    BEGIN
        SET @all_results_published = 1;
    END
    
    -- If no exam sessions exist, exam not taken yet
    IF @has_exam_sessions = 0
    BEGIN
        SELECT 'The exam is not taken yet' AS message;
        RETURN;
    END
    
    -- If exam sessions exist but no results, result not submitted yet
    IF @has_exam_sessions = 1 AND @has_results = 0
    BEGIN
        SELECT 'The result is not submitted yet' AS message;
        RETURN;
    END
    
    -- If some but not all results are published
    IF @has_exam_sessions = 1 AND @has_results = 1 AND @all_results_published = 0
    BEGIN
        SELECT 'The result is not published yet' AS message;
        RETURN;
    END
    
    -- Create temporary table to store subject results
    CREATE TABLE #SubjectResults (
        subject_id INT,
        subject_name NVARCHAR(150),
        subject_code NVARCHAR(30),
        credit_hours INT,
        total_marks DECIMAL(5,2),
        max_marks DECIMAL(5,2),
        percentage DECIMAL(5,2)
    );
    
    -- Calculate subject-wise results (excluding attendance for now)
    INSERT INTO #SubjectResults
    SELECT
        s.subject_id,
        s.name AS subject_name,
        s.subject_code,
        s.credit_hours,
        SUM(ISNULL(er.obtained_marks, 0) * et.weight_percentage / 100.0) AS total_marks,
        SUM(es.max_marks * et.weight_percentage / 100.0) AS max_marks,
        CASE
            WHEN SUM(es.max_marks * et.weight_percentage / 100.0) > 0 THEN
                SUM(ISNULL(er.obtained_marks, 0) * et.weight_percentage / 100.0) * 100.0 /
                SUM(es.max_marks * et.weight_percentage / 100.0)
            ELSE 0
        END AS percentage
    FROM Subjects s
    JOIN StudentSubjects ss ON s.subject_id = ss.subject_id
    JOIN ExamSessions es ON s.subject_id = es.subject_id AND es.year_id = @year_id
                        AND es.class_id = @class_id AND es.section_id = @section_id
    JOIN ExamTypes et ON es.exam_type_id = et.exam_type_id
    LEFT JOIN ExamResults er ON es.exam_session_id = er.exam_session_id AND er.enrollment_id = @enrollment_id
    WHERE ss.enrollment_id = @enrollment_id
      AND et.type_name != 'Attendance'  -- Exclude attendance type for now
    GROUP BY s.subject_id, s.name, s.subject_code, s.credit_hours;
    
    -- Add attendance marks for each subject (calculated from attendance tables)
    UPDATE sr
    SET sr.total_marks = sr.total_marks + ISNULL(att.attendance_marks, 0),
        sr.max_marks = sr.max_marks + ISNULL(att.max_marks, 0),  -- Add attendance max marks
        sr.percentage = CASE
            WHEN sr.max_marks + ISNULL(att.max_marks, 0) > 0 THEN
                (sr.total_marks + ISNULL(att.attendance_marks, 0)) * 100.0 / (sr.max_marks + ISNULL(att.max_marks, 0))
            ELSE 0
        END
    FROM #SubjectResults sr
    LEFT JOIN (
        -- Calculate attendance marks and max marks from attendance tables
        SELECT
            cr.subject_id,
            -- Calculate attendance percentage from actual attendance records
            (ISNULL((
                SELECT SUM(CASE WHEN ar.status IN ('Present', 'Late') THEN 1 ELSE 0 END) * 100.0 /
                       NULLIF(COUNT(ar.record_id), 0)
                FROM AttendanceRecords ar
                JOIN AttendanceSessions ats ON ar.session_id = ats.session_id
                JOIN ClassRoutine cr_inner ON ats.routine_id = cr_inner.routine_id
                WHERE cr_inner.subject_id = cr.subject_id
                  AND cr_inner.year_id = @year_id
                  AND cr_inner.class_id = @class_id
                  AND cr_inner.section_id = @section_id
                  AND ar.enrollment_id = @enrollment_id
            ), 0) * 5.0 / 100.0) AS attendance_marks,  -- 5% of attendance percentage
            -- Calculate max marks for attendance (5% of 100 = 5 marks)
            5.0 AS max_marks
        FROM ClassRoutine cr
        WHERE cr.year_id = @year_id
          AND cr.class_id = @class_id
          AND cr.section_id = @section_id
        GROUP BY cr.subject_id
    ) att ON sr.subject_id = att.subject_id;
    
    -- Return subject-wise results with grades from Grades table
    SELECT
        sr.subject_id,
        sr.subject_name,
        sr.subject_code,
        sr.credit_hours,
        sr.total_marks,
        sr.max_marks,
        sr.percentage,
        g.grade_name,
        g.grade_point,
        sr.credit_hours * g.grade_point AS weighted_grade_point
    FROM #SubjectResults sr
    JOIN Grades g ON sr.percentage BETWEEN g.min_mark AND g.max_mark
    ORDER BY sr.subject_name;
    
    -- Calculate and return overall GPA
    DECLARE @total_credit_hours INT = 0;
    DECLARE @total_weighted_grade_points DECIMAL(5,2) = 0;
    DECLARE @overall_gpa DECIMAL(3,2) = 0;
    
    SELECT
        @total_credit_hours = SUM(credit_hours * g.grade_point),
        @total_weighted_grade_points = SUM(credit_hours * g.grade_point)
    FROM #SubjectResults sr
    JOIN Grades g ON sr.percentage BETWEEN g.min_mark AND g.max_mark;
    
    IF @total_credit_hours > 0
    BEGIN
        SET @overall_gpa = @total_weighted_grade_points / @total_credit_hours;
    END
    
    -- Return overall GPA with grade from Grades table
    SELECT
        @enrollment_id AS enrollment_id,
        s.student_number,
        s.first_name,
        s.last_name,
        c.class_name,
        sec.section_name,
        ay.year_label,
        @total_credit_hours AS total_credit_hours,
        @total_weighted_grade_points AS total_weighted_grade_points,
        @overall_gpa AS overall_gpa,
        g.grade_name AS overall_grade
    FROM Students s
    JOIN Enrollments e ON s.student_id = e.student_id
    JOIN Classes c ON e.class_id = c.class_id
    JOIN Sections sec ON e.section_id = sec.section_id
    JOIN Academic_years ay ON e.year_id = ay.year_id
    JOIN Grades g ON @overall_gpa * 20 BETWEEN g.min_mark AND g.max_mark  -- Convert GPA to percentage scale
    WHERE e.enrollment_id = @enrollment_id;
    
    -- Clean up
    DROP TABLE #SubjectResults;
END
GO

---******
CREATE PROCEDURE spGetStudentOverallResult 7
    @enrollment_id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @year_id INT;
    DECLARE @class_id INT;
    DECLARE @section_id INT;
    DECLARE @student_id INT;
    DECLARE @has_results BIT = 0;
    
    -- Get enrollment details
    SELECT
        @year_id = year_id,
        @class_id = class_id,
        @section_id = section_id,
        @student_id = student_id
    FROM Enrollments
    WHERE enrollment_id = @enrollment_id;
    
    -- Check if any exam results exist for this enrollment
    IF EXISTS (
        SELECT 1 FROM ExamResults er
        JOIN ExamSessions es ON er.exam_session_id = es.exam_session_id
        WHERE er.enrollment_id = @enrollment_id
    )
    BEGIN
        SET @has_results = 1;
    END
    
    -- If no results found, return message
    IF @has_results = 0
    BEGIN
        SELECT 'The result is not published yet' AS message;
        RETURN;
    END
    
    -- Create temporary table to store subject results
    CREATE TABLE #SubjectResults (
        subject_id INT,
        subject_name NVARCHAR(150),
        subject_code NVARCHAR(30),
        credit_hours INT,
        total_marks DECIMAL(5,2),
        max_marks DECIMAL(5,2),
        percentage DECIMAL(5,2)
    );
    
    -- Calculate subject-wise results (excluding attendance for now)
    INSERT INTO #SubjectResults
    SELECT
        s.subject_id,
        s.name AS subject_name,
        s.subject_code,
        s.credit_hours,
        SUM(ISNULL(er.obtained_marks, 0) * et.weight_percentage / 100.0) AS total_marks,
        SUM(es.max_marks * et.weight_percentage / 100.0) AS max_marks,
        CASE
            WHEN SUM(es.max_marks * et.weight_percentage / 100.0) > 0 THEN
                SUM(ISNULL(er.obtained_marks, 0) * et.weight_percentage / 100.0) * 100.0 /
                SUM(es.max_marks * et.weight_percentage / 100.0)
            ELSE 0
        END AS percentage
    FROM Subjects s
    JOIN StudentSubjects ss ON s.subject_id = ss.subject_id
    JOIN ExamSessions es ON s.subject_id = es.subject_id AND es.year_id = @year_id
                        AND es.class_id = @class_id AND es.section_id = @section_id
    JOIN ExamTypes et ON es.exam_type_id = et.exam_type_id
    LEFT JOIN ExamResults er ON es.exam_session_id = er.exam_session_id AND er.enrollment_id = @enrollment_id
    WHERE ss.enrollment_id = @enrollment_id
      AND et.type_name != 'Attendance'  -- Exclude attendance type for now
    GROUP BY s.subject_id, s.name, s.subject_code, s.credit_hours;
    
    -- Add attendance marks for each subject
    UPDATE sr
    SET sr.total_marks = sr.total_marks + ISNULL(att.attendance_marks, 0),
        sr.max_marks = sr.max_marks + 5.0,  -- 5 marks for attendance
        sr.percentage = CASE
            WHEN sr.max_marks + 5.0 > 0 THEN
                (sr.total_marks + ISNULL(att.attendance_marks, 0)) * 100.0 / (sr.max_marks + 5.0)
            ELSE 0
        END
    FROM #SubjectResults sr
    LEFT JOIN (
        SELECT
            cr.subject_id,
            (ISNULL((
                SELECT SUM(CASE WHEN ar.status IN ('Present', 'Late') THEN 1 ELSE 0 END) * 100.0 /
                       NULLIF(COUNT(ar.record_id), 0)
                FROM AttendanceRecords ar
                JOIN AttendanceSessions ats ON ar.session_id = ats.session_id
                JOIN ClassRoutine cr_inner ON ats.routine_id = cr_inner.routine_id
                WHERE cr_inner.subject_id = cr.subject_id
                  AND cr_inner.year_id = @year_id
                  AND cr_inner.class_id = @class_id
                  AND cr_inner.section_id = @section_id
                  AND ar.enrollment_id = @enrollment_id
            ), 0) * 5.0 / 100.0) AS attendance_marks  -- 5% of 100 = 5 marks
        FROM ClassRoutine cr
        WHERE cr.year_id = @year_id
          AND cr.class_id = @class_id
          AND cr.section_id = @section_id
        GROUP BY cr.subject_id
    ) att ON sr.subject_id = att.subject_id;
    
    -- Return subject-wise results with grades from Grades table
    SELECT
        sr.subject_id,
        sr.subject_name,
        sr.subject_code,
        sr.credit_hours,
        sr.total_marks,
        sr.max_marks,
        sr.percentage,
        g.grade_name,
        g.grade_point,
        sr.credit_hours * g.grade_point AS weighted_grade_point
    FROM #SubjectResults sr
    JOIN Grades g ON sr.percentage BETWEEN g.min_mark AND g.max_mark
    ORDER BY sr.subject_name;
    
    -- Calculate and return overall GPA
    DECLARE @total_credit_hours INT = 0;
    DECLARE @total_weighted_grade_points DECIMAL(5,2) = 0;
    DECLARE @overall_gpa DECIMAL(3,2) = 0;
    
    SELECT
        @total_credit_hours = SUM(credit_hours * g.grade_point),
        @total_weighted_grade_points = SUM(credit_hours * g.grade_point)
    FROM #SubjectResults sr
    JOIN Grades g ON sr.percentage BETWEEN g.min_mark AND g.max_mark;
    
    IF @total_credit_hours > 0
    BEGIN
        SET @overall_gpa = @total_weighted_grade_points / @total_credit_hours;
    END
    
    -- Return overall GPA with grade from Grades table
    SELECT
        @enrollment_id AS enrollment_id,
        s.student_number,
        s.first_name,
        s.last_name,
        c.class_name,
        sec.section_name,
        ay.year_label,
        @total_credit_hours AS total_credit_hours,
        @total_weighted_grade_points AS total_weighted_grade_points,
        @overall_gpa AS overall_gpa,
        g.grade_name AS overall_grade
    FROM Students s
    JOIN Enrollments e ON s.student_id = e.student_id
    JOIN Classes c ON e.class_id = c.class_id
    JOIN Sections sec ON e.section_id = sec.section_id
    JOIN Academic_years ay ON e.year_id = ay.year_id
    JOIN Grades g ON @overall_gpa * 20 BETWEEN g.min_mark AND g.max_mark  -- Convert GPA to percentage scale
    WHERE e.enrollment_id = @enrollment_id;
    
    -- Clean up
    DROP TABLE #SubjectResults;
END
GO


---update from chatgpt
CREATE PROCEDURE spGetStudentOverallResults 7
    @enrollment_id INT
AS
BEGIN
    SET NOCOUNT ON;

    ----------------------------------------------------
    -- 1️⃣ Get enrollment basic info
    ----------------------------------------------------
    DECLARE @year_id INT;
    DECLARE @class_id INT;
    DECLARE @section_id INT;

    SELECT
        @year_id = year_id,
        @class_id = class_id,
        @section_id = section_id
    FROM Enrollments
    WHERE enrollment_id = @enrollment_id;


    ----------------------------------------------------
    -- 2️⃣ Temp table for subject-wise result
    ----------------------------------------------------
    CREATE TABLE #SubjectResults (
        subject_id INT,
        subject_name NVARCHAR(150),
        subject_code NVARCHAR(30),
        credit_hours INT,
        total_marks DECIMAL(6,2),
        max_marks DECIMAL(6,2),
        percentage DECIMAL(6,2)
    );


    ----------------------------------------------------
    -- 3️⃣ Insert SUBJECT results (missing = 0)
    ----------------------------------------------------
    INSERT INTO #SubjectResults
    SELECT
        s.subject_id,
        s.name,
        s.subject_code,
        s.credit_hours,

        -- obtained marks
        SUM(
            ISNULL(er.obtained_marks, 0) 
            * ISNULL(et.weight_percentage, 0) / 100.0
        ) AS total_marks,

        -- max marks
        SUM(
            ISNULL(es.max_marks, 0) 
            * ISNULL(et.weight_percentage, 0) / 100.0
        ) AS max_marks,

        -- percentage
        CASE 
            WHEN SUM(ISNULL(es.max_marks, 0) * ISNULL(et.weight_percentage, 0) / 100.0) > 0
            THEN
                SUM(ISNULL(er.obtained_marks, 0) * ISNULL(et.weight_percentage, 0) / 100.0)
                * 100.0 /
                SUM(ISNULL(es.max_marks, 0) * ISNULL(et.weight_percentage, 0) / 100.0)
            ELSE 0
        END AS percentage

    FROM StudentSubjects ss
    JOIN Subjects s 
        ON ss.subject_id = s.subject_id

    LEFT JOIN ExamSessions es
        ON es.subject_id = s.subject_id
       AND es.year_id = @year_id
       AND es.class_id = @class_id
       AND es.section_id = @section_id

    LEFT JOIN ExamTypes et
        ON es.exam_type_id = et.exam_type_id
       AND et.type_name <> 'Attendance'

    LEFT JOIN ExamResults er
        ON er.exam_session_id = es.exam_session_id
       AND er.enrollment_id = @enrollment_id

    WHERE ss.enrollment_id = @enrollment_id
    GROUP BY
        s.subject_id,
        s.name,
        s.subject_code,
        s.credit_hours;


    ----------------------------------------------------
    -- 4️⃣ Add Attendance marks (5%)
    ----------------------------------------------------
    UPDATE sr
    SET
        sr.total_marks = sr.total_marks + ISNULL(att.attendance_marks, 0),
        sr.max_marks   = sr.max_marks + 5,
        sr.percentage  =
            CASE
                WHEN (sr.max_marks + 5) > 0
                THEN (sr.total_marks + ISNULL(att.attendance_marks, 0)) * 100.0 / (sr.max_marks + 5)
                ELSE 0
            END
    FROM #SubjectResults sr
    LEFT JOIN (
        SELECT
            cr.subject_id,
            (
                COUNT(CASE WHEN ar.status IN ('Present','Late') THEN 1 END)
                * 100.0 /
                NULLIF(COUNT(ar.record_id), 0)
            ) * 5 / 100.0 AS attendance_marks
        FROM AttendanceRecords ar
        JOIN AttendanceSessions ats ON ar.session_id = ats.session_id
        JOIN ClassRoutine cr ON ats.routine_id = cr.routine_id
        WHERE ar.enrollment_id = @enrollment_id
          AND cr.year_id = @year_id
          AND cr.class_id = @class_id
          AND cr.section_id = @section_id
        GROUP BY cr.subject_id
    ) att ON sr.subject_id = att.subject_id;


    ----------------------------------------------------
    -- 5️⃣ Subject-wise result with Grade
    ----------------------------------------------------
    SELECT
        sr.subject_id,
        sr.subject_name,
        sr.subject_code,
        sr.credit_hours,
        sr.total_marks,
        sr.max_marks,
        sr.percentage,
        g.grade_name,
        g.grade_point,
        sr.credit_hours * g.grade_point AS weighted_grade_point
    FROM #SubjectResults sr
    JOIN Grades g
        ON sr.percentage BETWEEN g.min_mark AND g.max_mark
    ORDER BY sr.subject_name;


    ----------------------------------------------------
    -- 6️⃣ GPA Calculation
    ----------------------------------------------------
    DECLARE @total_credit_hours INT = 0;
    DECLARE @total_weighted_points DECIMAL(6,2) = 0;
    DECLARE @gpa DECIMAL(4,2) = 0;

    SELECT
        @total_credit_hours = SUM(sr.credit_hours),
        @total_weighted_points = SUM(sr.credit_hours * g.grade_point)
    FROM #SubjectResults sr
    JOIN Grades g
        ON sr.percentage BETWEEN g.min_mark AND g.max_mark;

    IF @total_credit_hours > 0
        SET @gpa = @total_weighted_points / @total_credit_hours;


    ----------------------------------------------------
    -- 7️⃣ Final Overall Result
    ----------------------------------------------------
    SELECT
        e.enrollment_id,
        st.student_number,
        st.first_name,
        st.last_name,
        c.class_name,
        sec.section_name,
        ay.year_label,
        @total_credit_hours AS total_credit_hours,
        @total_weighted_points AS total_weighted_grade_points,
        @gpa AS gpa,
        g.grade_name AS overall_grade
    FROM Enrollments e
    JOIN Students st ON e.student_id = st.student_id
    JOIN Classes c ON e.class_id = c.class_id
    JOIN Sections sec ON e.section_id = sec.section_id
    JOIN Academic_years ay ON e.year_id = ay.year_id
    JOIN Grades g ON (@gpa * 20) BETWEEN g.min_mark AND g.max_mark
    WHERE e.enrollment_id = @enrollment_id;


    ----------------------------------------------------
    -- 8️⃣ Cleanup
    ----------------------------------------------------
    DROP TABLE #SubjectResults;
END;
GO


