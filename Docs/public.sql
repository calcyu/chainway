create schema public;

comment on schema public is 'standard public schema';

alter schema public owner to postgres;

create table infos
(
	typ smallint not null,
	status smallint default 0 not null,
	name varchar(12) not null,
	tip varchar(30),
	created timestamp(0),
	creator varchar(10),
	adapted timestamp(0),
	adapter varchar(10)
);

alter table infos owner to postgres;

create table regs
(
	id smallint not null
		constraint regs_pk
			primary key,
	idx smallint
)
inherits (infos);

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
	zone smallint,
	mgrid integer,
	img bytea
)
inherits (infos);

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
	orgid smallint
		constraint users_orgid_fk
			references orgs,
	orgly smallint default 0 not null,
	idcard varchar(18)
)
inherits (infos);

alter table users owner to postgres;

create table ledgs_
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

alter table ledgs_ owner to postgres;

create table peerledgs_
(
	peerid smallint
)
inherits (ledgs_);

alter table peerledgs_ owner to postgres;

create table deals
(
	id bigserial not null
);

alter table deals owner to postgres;

create table peers_
(
	id smallint not null
		constraint peers_pk
			primary key,
	domain varchar(50),
	secure boolean,
	fed smallint,
	secret varchar(16)
)
inherits (infos);

alter table peers_ owner to postgres;

create table projs
(
	id serial not null,
	def jsonb
);

alter table projs owner to postgres;

create table projsites
(
	projid integer,
	idx smallint
)
inherits (infos);

alter table projsites owner to postgres;

create table projsitedats
(
	projid integer,
	site smallint,
	day date,
	a01 money,
	a02 money
);

alter table projsitedats owner to postgres;

create table projsteps
(
	projid integer,
	phase smallint
);

alter table projsteps owner to postgres;

create table projrevs
(
	projid integer,
	idx integer
)
inherits (infos);

alter table projrevs owner to postgres;

create table dealsteps
(
	dealid integer,
	idx smallint
)
inherits (infos);

alter table dealsteps owner to postgres;

