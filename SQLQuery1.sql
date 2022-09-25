select t.tutor_id
from tutor_courses t
where t.course LIKE '%av%';

select d.user_id
from days_available d
where d.sunday = 'false' and d.monday = 'true' and d.tuesday = 'true' and d.wednesday = 'false' and d.thursday = 'false' and d.friday = 'true' and d.saturday = 'true';

select *
from tutor_info i
where i.id = (select d.user_id
from days_available d
where d.sunday = 'false' and d.monday = 'true' and d.tuesday = 'true' and d.wednesday = 'false' and d.thursday = 'false' and d.friday = 'true' and d.saturday = 'true')
and i.id = (select t.tutor_id
from tutor_courses t
where t.course LIKE '%av%');



