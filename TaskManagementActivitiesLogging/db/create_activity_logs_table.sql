CREATE TABLE public."ACTIVITY_LOGS" (
    id bigserial PRIMARY KEY,
    event_type varchar(32) NOT NULL,
    entity varchar(128) NOT NULL,
    entity_id bigint NOT NULL,
    event_time timestamp NOT NULL
);