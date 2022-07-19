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

create table entities
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

create table scopes
(
    id smallint not null
        constraint regs_pk
            primary key,
    idx smallint
)
    inherits (entities);

alter table scopes owner to postgres;

create table orgs
(
    id serial not null
        constraint orgs_pk
            primary key,
    license varchar(20),
    regid smallint
        constraint orgs_regid_fk
            references scopes
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

create table dats
(
    stamp timestamp(0),
    var varchar(12),
    state smallint,
    value double precision,
    cent money
);

alter table dats owner to postgres;

create table projects
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

create table dealdats
(
    projid integer
)
    inherits (dats);

alter table dealdats owner to postgres;

create table scopedats
(
    regid smallint
)
    inherits (dats);

alter table scopedats owner to postgres;

create table plans
(
    projid integer,
    idx smallint
);

alter table plans owner to postgres;

create table deals
(
    id serial not null
)
    inherits (entities);

alter table deals owner to postgres;

create table clears
(
    id serial not null
        constraint clears_pk
            primary key,
    dt date,
    orgid integer not null,
    sprid integer not null,
    orders integer,
    total money,
    rate money,
    pay integer,
    status smallint
)
    inherits (entities);

alter table clears owner to postgres;

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

