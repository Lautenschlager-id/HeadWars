create table PlayerSkillH
(
	username varchar(12) not null,
	skill varchar(30) not null,
	skillStage int default 1 not null,

	foreign key(username) references PlayerH(username) on delete cascade
);