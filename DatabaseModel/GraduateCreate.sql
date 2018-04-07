Create Table Graduates

(
StudentID nvarchar(50) NOT NULL,
GraduateName nvarchar(50),
GraduateLastName nvarchar(50),
GraduateYear int,
WorkAreaID int,
WorkAreaDetailID int,
GraduateCompany nvarchar(100),
GraduateTitle nvarchar(50),
GraduateMail nvarchar(50),
GraduatePhone nvarchar(50),
PRIMARY KEY(StudentID),
FOREIGN KEY (WorkAreaID) REFERENCES WorkArea(WAID),
FOREIGN KEY (WorkAreaDetailID) REFERENCES WorkAreaDetail(WADID)
)