select * from Teams order by GroupId
select * from Matches

select * from Points

insert Points (TeamId) select Id from Teams
update Points set Played=0,Won=0,Drawn=0,Lost=0,GoalFor=0,GoalAgainst=0,GoalDifference=0,Points=0

update Points set Played=100 where Id = 1

select t.TeamName,m.HomeGoals,m.AwayGoals, t2.TeamName from Matches m 
join Teams t on t.Id = m.HomeId join Teams t2 on m.AwayId = t2.Id 

select * from Points p join teams t on t.Id = p.TeamId order by t.GroupId

alter table matches add Played bit

alter table matches drop IsPlayed

alter table matches drop constraint DF__Matches__Played__5AEE82B9

update matches set Played = 0


select * from Matches