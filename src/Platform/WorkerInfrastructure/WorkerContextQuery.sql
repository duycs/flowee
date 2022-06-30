
use workerdb;
select * from roles;
select * from workerdb.groups;
select * from departments;
select * from shifts;
select * from skillLevels;
select * from skills;
select * from workers;

select * from workergroups;
select * from workerRoles;
select * from workerskills;
select * from workerShifts;

truncate table workerShifts;

-- worker have roles, groups, department
select w.Id as WorkerId, w.Code as WorkerCode, w.FullName as WorkerFullName, w.Email, r.Id as RoleId, r.Name as RoleName, r.Code as RoleCode,
g.Id as GroupId, g.Name as GroupName, g.Code as GroupCode 
from Workers w
join WorkerRoles wr on w.Id = wr.WorkerId
join Roles r on r.Id = wr.RoleId
join WorkerGroups wg on w.Id = wg.GroupId
join workerdb.Groups g on wg.GroupId = g.Id;

-- worker have skill
select w.id WorkerId, w.fullname WorkerName, w.code WorkerName, w.email WorkerEmail, 
ws.skillid SkillId, s.code as SkillCode, s.Name SkillName, 
ws.skillLevelid SkillLevelId, sl.name SkillLevelName 
from workers w 
join workerskills ws on w.id = ws.workerid
join skills s on s.id = ws.skillid
join skilllevels sl on sl.id = ws.SkillLevelId;

-- worker have shift time keeping
select w.id WorkerId, w.fullname WorkerName, 
ws.IsNormal IsWorkerWorkNormalShift, ws.dateStarted WorkerDateStarted, ws.dateEnded WorkerDateEnded,
ws.Id ShiftId, s.Name ShiftName, s.isNormal IsNormalShift, s.timeStart ShiftTimeStart, s.timeEnd ShiftTimeEnd 
from workers w
join workershifts ws on w.id = ws.workerId
join shifts s on ws.shiftId = s.id;