USE SchoolDb;

-- Restore db by delete all existing data
DELETE FROM Grades;
DELETE FROM Students;
DELETE FROM Subjects;

-- Reset identity seeds to 1
DBCC CHECKIDENT ('Grades', RESEED, 1);
DBCC CHECKIDENT ('Students', RESEED, 1);
DBCC CHECKIDENT ('Subjects', RESEED, 1);


-- Insert new data
INSERT INTO [dbo].[Students] (FirstName, LastName, DateOfBirth)
VALUES ('John', 'Doe', '1999-01-01 00:00:00.0000000'),
       ('Jane', 'Smith', '2000-02-02 00:00:00.0000000'),
       ('Bob', 'Johnson', '1998-03-03 00:00:00.0000000');

INSERT INTO [dbo].[Subjects] (Name)
VALUES ('Math'),
       ('Science'),
       ('English');


INSERT INTO [dbo].[Grades] (StudentId, SubjectId, What, Mark, Date)
VALUES (1, 1, 'Test 1', 80, '2022-01-01 00:00:00.0000000'),
       (2, 1, 'Test 1', 90, '2022-01-01 00:00:00.0000000'),
       (3, 1, 'Test 1', 75, '2022-01-01 00:00:00.0000000'),
       (1, 2, 'Test 1', 85, '2022-01-01 00:00:00.0000000'),
       (2, 2, 'Test 1', 95, '2022-01-01 00:00:00.0000000'),
       (3, 2, 'Test 1', 70, '2022-01-01 00:00:00.0000000');

-- Show inserted data
SELECT s.Id, s.FirstName, s.LastName, s.DateOfBirth,
       sb.Id, sb.Name,
       g.Id, g.StudentId, g.SubjectId, g.What, g.Mark, g.Date
FROM Students s
JOIN Grades g ON s.Id = g.StudentId
JOIN Subjects sb ON sb.Id = g.SubjectId;
