
create database galaxy
go
use galaxy
go
create table Employee
(
	employee_id		int				primary key identity(1,1),
    surname			varchar(100)	null,
	[name]			varchar(100)	null,
	middle_name		varchar(100)	null,
	birthday		datetime		null
)


insert into Employee(surname,[name],middle_name	,birthday) values('Сидоров','Игорь', 'Николаевич','19911221')
insert into Employee(surname,[name],middle_name	,birthday) values('Федоров','Евгений', 'Петрович','19911222')
insert into Employee(surname,[name],middle_name	,birthday) values('Фатеев','Дмитрий', 'Владимирович','19911223')
insert into Employee(surname,[name],middle_name	,birthday) values('Хмельницкий','Денис', 'Алексеевич','19911224')
insert into Employee(surname,[name],middle_name	,birthday) values('Валявский','Ярослав', 'Генадьевич','19911225')

select * from Employee
go
GRANT SELECT ON dbo.Employee TO public
go
GRANT UPDATE ON dbo.Employee TO public
go
GRANT insert ON dbo.Employee TO public
go

