create schema public;

comment on schema public is 'standard public schema';

alter schema public owner to postgres;

create table clears
(
	id smallserial not null
);

alter table clears owner to postgres;

create table subscribs
(
	typ smallint not null,
	status smallint default 0 not null,
	partyid smallint not null,
	ctrid integer,
	created timestamp(0),
	creator varchar(10),
	id serial not null,
	no bigint,
	planid smallint,
	itemid smallint not null,
	price money,
	"off" money,
	qty integer,
	pay money,
	refund money,
	codestart integer,
	codes smallint,
	prodid integer
);

alter table subscribs owner to postgres;

create table _beans
(
	typ smallint not null,
	status smallint default 0 not null,
	name varchar(10) not null,
	tip varchar(30),
	created timestamp(0),
	creator varchar(10),
	modified timestamp(0),
	modifier varchar(10)
);

alter table _beans owner to postgres;

create table items
(
	id serial not null
		constraint items_pk
			primary key,
	unit varchar(4),
	unitip varchar(10),
	icon bytea
)
inherits (_beans);

alter table items owner to postgres;

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
	license varchar(20),
	idcard varchar(18),
	icon bytea
)
inherits (_beans);

alter table users owner to postgres;

create index users_admly_idx
	on users (admly)
	where (admly > 0);

create unique index users_im_idx
	on users (im);

create index users_orgid_idx
	on users (orgid)
	where (orgid > 0);

create unique index users_tel_idx
	on users (tel);

create table regs
(
	id smallint not null
		constraint regs_pk
			primary key,
	idx smallint
)
inherits (_beans);

alter table regs owner to postgres;

create table agrees
(
	id serial not null,
	orgid smallint,
	started date,
	ended date,
	content jsonb
)
inherits (_beans);

alter table agrees owner to postgres;

create table distribs
(
	id serial not null
		constraint books_pk
			primary key,
	bizid smallint not null,
	ctrid integer,
	supplyid smallint,
	itemid smallint not null,
	price money,
	"off" money,
	qty integer,
	pay money,
	refund money
)
inherits (_beans);

alter table distribs owner to postgres;

create table yields
(
	id serial not null
		constraint yields_pk
			primary key,
	srcid integer,
	itemid smallint,
	test bytea
)
inherits (_beans);

alter table yields owner to postgres;

create table orgs
(
	id serial not null
		constraint orgs_pk
			primary key,
	sprid integer,
	ctrid integer,
	license varchar(20),
	trust boolean,
	regid smallint not null,
	addr varchar(30),
	x double precision,
	y double precision,
	mgrid integer,
	icon bytea,
	fork smallint
)
inherits (_beans);

alter table orgs owner to postgres;

create table supplys
(
	id serial not null
		constraint supplys_pk
			primary key,
	ctrid integer not null,
	itemid integer not null,
	started date,
	ended date,
	filled date,
	bmode smallint,
	bunit varchar(4),
	bx smallint,
	bmin smallint,
	bmax smallint,
	bstep smallint,
	bprice money,
	boff money,
	dmode smallint,
	dunit varchar(4),
	dx smallint,
	dmin smallint,
	dmax smallint,
	dstep smallint,
	dprice money,
	doff money,
	smode smallint,
	sunit varchar(4),
	sx smallint,
	smin smallint,
	smax smallint,
	sstep smallint,
	sprice money,
	soff money
)
inherits (_beans);

alter table supplys owner to postgres;

create table posts
(
	id integer,
	supplyid integer,
	itemid integer,
	price money,
	discount money,
	status smallint
);

alter table posts owner to postgres;

create table bids
(
	status smallint default 0 not null,
	id serial not null,
	typ smallint,
	martid smallint not null,
	bizid integer not null,
	uid integer not null,
	price money,
	pay money,
	refund money,
	itemid smallint
);

alter table bids owner to postgres;

create view orgs_vw(typ, status, name, tip, created, creator, modified, modifier, id, fork, sprid, ctrid, license, trust, regid, addr, x, y, mgrid, mgrname, mgrtel, mgrim, icon) as
SELECT o.typ,
       o.status,
       o.name,
       o.tip,
       o.created,
       o.creator,
       o.modified,
       o.modifier,
       o.id,
       o.fork,
       o.sprid,
       o.ctrid,
       o.license,
       o.trust,
       o.regid,
       o.addr,
       o.x,
       o.y,
       o.mgrid,
       m.name             AS mgrname,
       m.tel              AS mgrtel,
       m.im               AS mgrim,
       o.icon IS NOT NULL AS icon
FROM orgs o
         LEFT JOIN users m
                   ON o.mgrid =
                      m.id;

alter table orgs_vw owner to postgres;

