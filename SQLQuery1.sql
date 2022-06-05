

create table Points(
	Id int identity(1,1) primary key,
	TeamId tinyint not null,
	Played tinyint,
	Won tinyint,
	Drawn tinyint,
	Lost tinyint, GoalFor tinyint, GoalAgainst tinyint, GoalDifference tinyint, Points tinyint
)

create table Matches(
Id int identity(1,1) primary key, HomeId tinyint,AwayId tinyint,HomeGoals tinyint, AwayGoals tinyint
)

alter table Points add constraint FK_Points_TeamId foreign key (TeamId) references Teams(Id)
alter table Matches add constraint FK_Matches_HomeId foreign key (HomeId) references Teams(Id)
alter table Matches add constraint FK_Matches_AwayId foreign key (AwayId) references Teams(Id)