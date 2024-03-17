use ahvia_fateor;
update app_config set ConfigValue = 'Estudios Realizados' where ConfigCode='Default_InitialPoll';

use lvs_fateor;
update app_config set ConfigValue = 'Test de Nolan' where ConfigCode='Default_InitialPoll';

select * from app_config where ConfigCode='Default_InitialPoll';

select * from qst_questionaire;
