CREATE TABLE public."TASKS" (
    id bigserial PRIMARY KEY,
    title varchar(256) NOT NULL,
    description varchar NOT NULL,
    status varchar(32) NOT NULL,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL
);