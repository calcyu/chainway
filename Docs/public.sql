create table infos
(
	typ smallint not null,
	state smallint default 0 not null,
	name varchar(12) not null,
	tip varchar(30),
	created timestamp(0),
	creator varchar(10),
	adapted timestamp(0),
	adapter varchar(10)
);

alter table entities owner to postgres;

create table regs
(
	id smallint not null
		constraint regs_pk
			primary key,
	idx smallint
)
inherits (entities);

alter table regs owner to postgres;

create table orgs
(
	id serial not null
		constraint orgs_pk
			primary key,
	license varchar(20),
	regid smallint
		constraint orgs_regid_fk
			references regs
				on update cascade,
	addr varchar(30),
	x double precision,
	y double precision,
	tel varchar(11),
	fork smallint,
	mgrid integer,
	img bytea
)
inherits (entities);

alter table orgs owner to postgres;

create table users
(
	id serial not null
		constraint users_pk
			primary key,
	tel varchar(11) not null,
	im varchar(28),
	credential varchar(32),
	admly smallint default 0 not null,
	orgid smallint,
	orgly smallint default 0 not null,
	idcard varchar(18)
)
inherits (entities);

alter table users owner to postgres;

create table ledgrs_
(
	seq integer,
	acct varchar(20),
	name varchar(12),
	amt integer,
	bal integer,
	cs uuid,
	blockcs uuid,
	stamp timestamp(0)
);

alter table ledgrs_ owner to postgres;

create table peerledgrs_
(
	peerid smallint
)
inherits (ledgrs_);

alter table peerledgrs_ owner to postgres;

create table deals
(
	id serial not null,
	projectid integer,
	orgid integer,
	uid integer,
	uname varchar(12),
	utel varchar(11),
	uim varchar(28),
	state jsonb
);

alter table deals owner to postgres;

create table peers_
(
	id smallint not null
		constraint peers_pk
			primary key,
	weburl varchar(50),
	fed smallint,
	secret varchar(16)
)
inherits (entities);

alter table peers_ owner to postgres;

create table reviews
(
	projid integer,
	idx integer
)
inherits (entities);

alter table reviews owner to postgres;

create table accts_
(
	no varchar(20),
	v integer
)
inherits (entities);

alter table accts_ owner to postgres;

create table mvarconds
(
	factid varchar(8),
	idx smallint,
	webp bytea,
	audio bytea,
	name varchar(12),
	expr varchar(30)
);

alter table mobjconds owner to postgres;

create table mvars
(
	id varchar(8)
)
inherits (entities);

alter table mobjs owner to postgres;

create table dats
(
	stamp timestamp(0),
	var varchar(12),
	state smallint,
	value double precision,
	quality money,
	quantity money
);

alter table dats owner to postgres;

create table mopconds
(
);

alter table mtaskconds owner to postgres;

create table projs
(
	id serial not null,
	orgid integer,
	unit varchar(4),
	price money,
	min smallint,
	max smallint,
	step smallint,
	mpml xml
)
inherits (entities);

alter table projects owner to postgres;

create table projdats
(
	projid integer
)
inherits (dats);

alter table dealdats owner to postgres;

create table regdats
(
	regid smallint
)
inherits (dats);

alter table regdats owner to postgres;

create table clazzdats
(
	classid varchar(12)
)
inherits (dats);

alter table catdats owner to postgres;

create table mops
(
	id varchar(8),
	column_2 integer
)
inherits (entities);

alter table mtasks owner to postgres;

create table clazzs
(
	id varchar(20)
);

alter table cats owner to postgres;

create table dealdats
(
	dealid integer,
	typ smallint
)
inherits (dats);

alter table dealdats owner to postgres;

