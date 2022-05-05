create schema public;

alter schema public owner to postgres;

create type dat_type as
(
	clock smallint,
	a1 real,
	a2 real,
	a3 real,
	a4 real,
	a5 real
);

alter type dat_type owner to postgres;

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
	fork smallint,
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
	orgid smallint,
	orgly smallint default 0 not null,
	idcard varchar(18)
)
inherits (infos);

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

create table plays
(
	id bigserial not null
);

alter table plays owner to postgres;

create table peers_
(
	id smallint not null
		constraint peers_pk
			primary key,
	weburl varchar(50),
	fed smallint,
	secret varchar(16)
)
inherits (infos);

alter table peers_ owner to postgres;

create table reviews
(
	projid integer,
	idx integer
)
inherits (infos);

alter table reviews owner to postgres;

create table accts_
(
	no varchar(20),
	v integer
)
inherits (infos);

alter table accts_ owner to postgres;

create table jobs
(
	id varchar(8),
	column_2 integer
);

alter table jobs owner to postgres;

create table factorcas
(
	factid varchar(8),
	idx smallint,
	webp bytea,
	audio bytea,
	name varchar(12),
	expr varchar(30)
);

alter table factorcas owner to postgres;

create table factors
(
	id varchar(8)
)
inherits (infos);

alter table factors owner to postgres;

create table sitedats
(
	siteidx integer,
	dats dat_type[],
	dt date
);

alter table sitedats owner to postgres;

create table sites
(
	id serial not null
		constraint sites_pkey
			primary key,
	vars varchar(8) []
)
inherits (infos);

alter table sites owner to postgres;

create table projects
(
	id serial not null,
	factrefs varchar(8) [],
	unit varchar(4),
	price money,
	min smallint,
	max smallint,
	step smallint
)
inherits (infos);

alter table projects owner to postgres;

create view orgs_vw(typ, status, name, tip, created, creator, adapted, adapter, id, fork, license, regid, addr, x, y, tel, mgrid, mgrname, mgrtel, mgrim, img) as
SELECT o.typ,
       o.status,
       o.name,
       o.tip,
       o.created,
       o.creator,
       o.adapted,
       o.adapter,
       o.id,
       o.fork,
       o.license,
       o.regid,
       o.addr,
       o.x,
       o.y,
       o.tel,
       o.mgrid,
       m.name            AS mgrname,
       m.tel             AS mgrtel,
       m.im              AS mgrim,
       o.img IS NOT NULL AS img
FROM orgs o
         LEFT JOIN users m
                   ON o.mgrid =
                      m.id;

alter table orgs_vw owner to postgres;

