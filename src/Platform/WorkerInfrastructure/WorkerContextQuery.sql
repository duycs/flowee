
use workerdb;
select * from roles;
select * from workerdb.groups;
select * from departments;
select * from shifts;
select * from skillLevels;
select * from skills;
select * from workershifts;
select * from workergroups;
select * from workers;
select * from workerskills;

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