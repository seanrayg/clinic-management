USE [dbClinicManagement]
GO
ALTER TABLE [dbo].[Staff] DROP CONSTRAINT [FK_Staff_UserType]
GO
ALTER TABLE [dbo].[Patient] DROP CONSTRAINT [FK_Patient_PCollege]
GO
ALTER TABLE [dbo].[Patient] DROP CONSTRAINT [FK_Patient_PatientType]
GO
ALTER TABLE [dbo].[MedCheckHeader] DROP CONSTRAINT [FK_MedCheckHeader_Staff]
GO
ALTER TABLE [dbo].[MedCheckHeader] DROP CONSTRAINT [FK_MedCheckHeader_Patient]
GO
ALTER TABLE [dbo].[MedCheckHeader] DROP CONSTRAINT [FK_MedCheckHeader_Inventory]
GO
ALTER TABLE [dbo].[MedCheckDetail] DROP CONSTRAINT [FK_MedCheckDetail_MedCheckHeader]
GO
ALTER TABLE [dbo].[Inventory] DROP CONSTRAINT [FK_Inventory_Supply]
GO
ALTER TABLE [dbo].[Inventory] DROP CONSTRAINT [FK_Inventory_Staff]
GO
/****** Object:  Table [dbo].[UserType]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[UserType]
GO
/****** Object:  Table [dbo].[Supply]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[Supply]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[Staff]
GO
/****** Object:  Table [dbo].[PCollege]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[PCollege]
GO
/****** Object:  Table [dbo].[PatientType]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[PatientType]
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[Patient]
GO
/****** Object:  Table [dbo].[MedCheckHeader]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[MedCheckHeader]
GO
/****** Object:  Table [dbo].[MedCheckDetail]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[MedCheckDetail]
GO
/****** Object:  Table [dbo].[Inventory]    Script Date: 30/12/2017 1:09:09 AM ******/
DROP TABLE [dbo].[Inventory]
GO
/****** Object:  Table [dbo].[Inventory]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventory](
	[InventoryID] [varchar](20) NOT NULL,
	[SupplyID] [int] NULL,
	[StaffID] [varchar](20) NULL,
	[SupplyQuantity] [int] NULL,
	[ExpirationDate] [date] NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
(
	[InventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedCheckDetail]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedCheckDetail](
	[MedCheckHID] [varchar](20) NOT NULL,
	[DateofVisit] [date] NULL,
	[TimeofVisit] [time](7) NULL,
	[Complaint] [text] NULL,
	[Diagnosis] [text] NULL,
	[Treatment] [text] NULL,
	[Remarks] [text] NULL,
 CONSTRAINT [PK_MedCheckDetail] PRIMARY KEY CLUSTERED 
(
	[MedCheckHID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedCheckHeader]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedCheckHeader](
	[MedCheckHID] [varchar](20) NOT NULL,
	[StaffID] [varchar](20) NULL,
	[PatientID] [int] NULL,
	[InventoryID] [varchar](20) NULL,
	[MedCheckType] [int] NULL,
	[MedCheckStatus] [int] NULL,
 CONSTRAINT [PK_MedCheckHeader] PRIMARY KEY CLUSTERED 
(
	[MedCheckHID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patient](
	[PatientID] [int] IDENTITY(1,1) NOT NULL,
	[PatientLast] [varchar](50) NULL,
	[PatientFirst] [varchar](max) NULL,
	[PatientMid] [varchar](50) NULL,
	[PatientGender] [varchar](20) NULL,
	[PatientBDate] [date] NULL,
	[PatientAddrss] [text] NULL,
	[TypeID] [int] NULL,
	[PatientClass] [int] NULL,
	[CollegeID] [int] NOT NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[PatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientType]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientType](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](20) NULL,
 CONSTRAINT [PK_PatientType] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PCollege]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PCollege](
	[CollegeID] [int] IDENTITY(1,1) NOT NULL,
	[CollegeCode] [varchar](10) NULL,
	[CollegeName] [varchar](max) NULL,
 CONSTRAINT [PK_PCollege] PRIMARY KEY CLUSTERED 
(
	[CollegeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[StaffID] [varchar](20) NOT NULL,
	[StaffLast] [varchar](50) NULL,
	[StaffFirst] [varchar](max) NULL,
	[StaffMid] [varchar](50) NULL,
	[StaffGender] [varchar](20) NULL,
	[StaffPassword] [varchar](20) NOT NULL,
	[StaffJoinedDate] [nchar](10) NULL,
	[UserTypeID] [int] NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[StaffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supply]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supply](
	[SupplyID] [int] NOT NULL,
	[SupplyName] [varchar](max) NULL,
	[ReceivedDate] [date] NULL,
	[SupplyType] [int] NOT NULL,
 CONSTRAINT [PK_Supply] PRIMARY KEY CLUSTERED 
(
	[SupplyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserType]    Script Date: 30/12/2017 1:09:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserType](
	[UserTypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeDesc] [varchar](50) NULL,
 CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED 
(
	[UserTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Patient] ON 

INSERT [dbo].[Patient] ([PatientID], [PatientLast], [PatientFirst], [PatientMid], [PatientGender], [PatientBDate], [PatientAddrss], [TypeID], [PatientClass], [CollegeID]) VALUES (2004, N'Fontanilla', N'Jonina Rogette', N'Voces', N'Female', NULL, N'Blk 19 Lot 56 Pound st. Lores Country Homes, Antipolo City', 1, 3, 1)
SET IDENTITY_INSERT [dbo].[Patient] OFF
SET IDENTITY_INSERT [dbo].[PatientType] ON 

INSERT [dbo].[PatientType] ([TypeID], [TypeName]) VALUES (1, N'Student')
INSERT [dbo].[PatientType] ([TypeID], [TypeName]) VALUES (2, N'Faculty')
INSERT [dbo].[PatientType] ([TypeID], [TypeName]) VALUES (3, N'Staff')
SET IDENTITY_INSERT [dbo].[PatientType] OFF
SET IDENTITY_INSERT [dbo].[PCollege] ON 

INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (1, N'CCIS', N'College of Computer and Information Sciences')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (2, N'CBA', N'College of Business Administration')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (3, N'CAF', N'College of Accountancy and Finance')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (4, N'CAFA', N'College of Architecture and Fine Arts')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (5, N'CAL', N'College of Arts and Letters')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (6, N'COC', N'College of Communication')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (7, N'COED', N'College of Education')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (8, N'COE', N'College of Engineering')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (9, N'CHK', N'College of Human Kinetics')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (10, N'CPSPA', N'College of Political Science and Public Administration')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (11, N'COL', N'College of Law')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (12, N'CS', N'College of Science')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (13, N'CSSD', N'College of Social Sciences and Development')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (14, N'CTHTM', N'College of Tourism, Hospitality and Transportation Management')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (15, N'ITECH', N'Institute of Technology')
INSERT [dbo].[PCollege] ([CollegeID], [CollegeCode], [CollegeName]) VALUES (16, N'LHS', N'Laboratory High School')
SET IDENTITY_INSERT [dbo].[PCollege] OFF
INSERT [dbo].[Staff] ([StaffID], [StaffLast], [StaffFirst], [StaffMid], [StaffGender], [StaffPassword], [StaffJoinedDate], [UserTypeID]) VALUES (N'20171212-01', N'admin', N'admin', N'admin', N'Male', N'admin', NULL, 1)
SET IDENTITY_INSERT [dbo].[UserType] ON 

INSERT [dbo].[UserType] ([UserTypeID], [TypeDesc]) VALUES (1, N'admin')
INSERT [dbo].[UserType] ([UserTypeID], [TypeDesc]) VALUES (2, N'nurse')
SET IDENTITY_INSERT [dbo].[UserType] OFF
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Staff] FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([StaffID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Staff]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Supply] FOREIGN KEY([SupplyID])
REFERENCES [dbo].[Supply] ([SupplyID])
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Supply]
GO
ALTER TABLE [dbo].[MedCheckDetail]  WITH CHECK ADD  CONSTRAINT [FK_MedCheckDetail_MedCheckHeader] FOREIGN KEY([MedCheckHID])
REFERENCES [dbo].[MedCheckHeader] ([MedCheckHID])
GO
ALTER TABLE [dbo].[MedCheckDetail] CHECK CONSTRAINT [FK_MedCheckDetail_MedCheckHeader]
GO
ALTER TABLE [dbo].[MedCheckHeader]  WITH CHECK ADD  CONSTRAINT [FK_MedCheckHeader_Inventory] FOREIGN KEY([InventoryID])
REFERENCES [dbo].[Inventory] ([InventoryID])
GO
ALTER TABLE [dbo].[MedCheckHeader] CHECK CONSTRAINT [FK_MedCheckHeader_Inventory]
GO
ALTER TABLE [dbo].[MedCheckHeader]  WITH CHECK ADD  CONSTRAINT [FK_MedCheckHeader_Patient] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patient] ([PatientID])
GO
ALTER TABLE [dbo].[MedCheckHeader] CHECK CONSTRAINT [FK_MedCheckHeader_Patient]
GO
ALTER TABLE [dbo].[MedCheckHeader]  WITH CHECK ADD  CONSTRAINT [FK_MedCheckHeader_Staff] FOREIGN KEY([StaffID])
REFERENCES [dbo].[Staff] ([StaffID])
GO
ALTER TABLE [dbo].[MedCheckHeader] CHECK CONSTRAINT [FK_MedCheckHeader_Staff]
GO
ALTER TABLE [dbo].[Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_PatientType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[PatientType] ([TypeID])
GO
ALTER TABLE [dbo].[Patient] CHECK CONSTRAINT [FK_Patient_PatientType]
GO
ALTER TABLE [dbo].[Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_PCollege] FOREIGN KEY([CollegeID])
REFERENCES [dbo].[PCollege] ([CollegeID])
GO
ALTER TABLE [dbo].[Patient] CHECK CONSTRAINT [FK_Patient_PCollege]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_UserType] FOREIGN KEY([UserTypeID])
REFERENCES [dbo].[UserType] ([UserTypeID])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_UserType]
GO
