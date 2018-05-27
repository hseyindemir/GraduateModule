Create Table Graduates

(
StudentID nvarchar(50) NOT NULL,
StudentPassword nvarchar(100) NOT NULL,
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

Create Table Admins

(
AdminID nvarchar(50) NOT NULL,
AdminName nvarchar(50),
AdminLastName nvarchar(50),
AdminPassword nvarchar(100) NOT NULL,
PRIMARY KEY(AdminID)

)

Create Table AdminGraduateVerification

(
VerificationID int IDENTITY(1,1),
IsVerified bit,
AdminID nvarchar(50),
StudentID nvarchar(50),
GraduateName nvarchar(50),
GrauateSurname nvarchar(50),
GraduateEmail nvarchar(50),
PRIMARY KEY(VerificationID),
FOREIGN KEY (AdminID) REFERENCES Admins(AdminID),
FOREIGN KEY (StudentID) REFERENCES Graduates(StudentID)
)

drop Table Graduates

