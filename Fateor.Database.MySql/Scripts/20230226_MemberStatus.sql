alter table `org_status`
add column IsActiveMember tinyint not null default(0) after CanBeElected;
