<img src="https://avatars.githubusercontent.com/u/145294458?s=200&v=4" alt="Logo of the project" align="right">

# LingoMq &middot; Database documentation

> Документация базы данных

LingoMq - it is application, which represents opportunity to learns any language by himself with help recording words and repeating them

## Обобщенное описание

<img src="https://sun9-49.userapi.com/impg/CG_zsQNso2vjhwazy1SKXiIsZwdwqtt_vV0TSQ/K8WDgBh5j44.jpg?size=1872x902&quality=95&sign=38341e41548305d15382efa36421452c&c_uniq_tag=ccAYhEtC52eSOBh-Mjbn3M70cbrN1_2oN_oQ2a9nAyw&type=album" alt="Logo of the project" align="center">

> Структура таблиц

В данный момент придерживаемся такой общей структуры 

## Личности

<img src="https://sun9-76.userapi.com/impg/V8Zx2IuSp4RN1epGTkiXkWrTyxO6Pi8kuXC1yg/WfHjEJFDxJs.jpg?size=1567x298&quality=95&sign=647e3fb5e146f145bf4ad6ed223544ab&type=album" alt="Logo of the project" align="center">

> Структура зоны личностей 

Данная зона представляет собой таблицы, работающие с пользовательскими данными, роли, статистика, подробная информация

### Таблицы
#### Users

| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Email | varchar(256) | Это поле сохраняет в себе Email пользователя  для проверки пользователя.  Является обязательным к заполнению | UNIQUE, NOT NULL CHECKS(0>N<=256) |
|  | Phone | varchar(15) | Это поле сохраняет в себе телефон пользователя по его обоюдному согласию. Не является обязательным к заполнению | UNIQUE,  CHECKS(0>N<=15) |
|  | PasswordHash | varchar(MAX) | Это поле хранит хеш пароля | NOT NULL |
|  | PasswordSalt | varchar(MAX) | Это поле хранит "соль" пароля | NOT NULL |

#### Users:Ссылается на
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Words.UserWords | UserWords.UserId -> Users.Id | FK_UserWords_UserId -> Users.Id |
| Notifications.UserNotifications | UserNotifications.UserId -> Users.Id | FK_UserNotifications_UserId -> Users.Id |
| Achievements.UserAchievements | UserAchievements.UserId -> Users.Id | FK_UserAchievements_UserId -> Users.Id |
| Finances.UserFinances | UserFinances.UserId -> Users.Id | FK_UserFinances_UserId -> Users.Id |
| Topics.TopicStatistics | Topics.UserId -> Users.Id | FK_TopicStatistics_UserId -> Users.Id |


