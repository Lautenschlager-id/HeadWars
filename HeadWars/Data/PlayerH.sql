create table PlayerH
(
	username varchar(12) primary key,
	points int default 0 not null,
	experience int default 6 not null,
	highscore int default 0 not null,

	foreign key(username) references UserH(username) on delete cascade
);