<img src="https://avatars.githubusercontent.com/u/145294458?s=200&v=4" alt="Logo of the project" align="right">

# LingoMq &middot; Database documentation

> Документация базы данных

LingoMq - it is application, which represents opportunity to learns any language by himself with help recording words and repeating them

## Содержание

1. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#%D0%BE%D0%B1%D0%BE%D0%B1%D1%89%D0%B5%D0%BD%D0%BD%D0%BE%D0%B5-%D0%BE%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5">Обобщенное описание</a>
2. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#%D0%BB%D0%B8%D1%87%D0%BD%D0%BE%D1%81%D1%82%D0%B8">Личности</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#users">Users</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userinfos">UserInfos</a>
## Обобщенное описание

<img src="https://sun9-10.userapi.com/impg/_LdEybbTJqAnLlreDZbYT1IgKgtNaYh3mRBqxQ/8bjgdNMF_9Q.jpg?size=1708x843&quality=95&sign=c1e97f22be7e7c20cc9414572e028719&type=album" alt="Logo of the project" align="center">

> Структура таблиц

В данный момент придерживаемся такой общей структуры 

## Личности

<img src="https://sun9-21.userapi.com/impg/E5mQkKjyXvw8XscpQWbfsifyQWEWaopohLVUwQ/TnPMp6mjVHE.jpg?size=1707x323&quality=95&sign=213ca7a952a5ab84d8ecbfe135f34369&type=album" alt="Logo of the project" align="center">

> Структура зоны личностей 

Данная зона представляет собой таблицы, работающие с пользовательскими данными, роли, статистика, подробная информация

## Таблицы
### Users
> Суть таблицы - отделить критические данные для входа от общих данных
#### Users:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Email | varchar(256) | Это поле сохраняет в себе Email пользователя  для проверки пользователя.  Является обязательным к заполнению | UNIQUE, NOT NULL CHECKS(0>N<=256) |
|  | Phone | varchar(15) | Это поле сохраняет в себе телефон пользователя по его обоюдному согласию. Не является обязательным к заполнению | UNIQUE,  CHECKS(0>N<=15) |
|  | PasswordHash | varchar(MAX) | Это поле хранит хеш пароля | NOT NULL |
|  | PasswordSalt | varchar(MAX) | Это поле хранит "соль" пароля | NOT NULL |

#### Users:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Words.UserWords | UserWords.UserId -> Users.Id | FK_UserWords_UserId -> Users.Id |
| Notifications.UserNotifications | UserNotifications.UserId -> Users.Id | FK_UserNotifications_UserId -> Users.Id |
| Achievements.UserAchievements | UserAchievements.UserId -> Users.Id | FK_UserAchievements_UserId -> Users.Id |
| Finances.UserFinances | UserFinances.UserId -> Users.Id | FK_UserFinances_UserId -> Users.Id |
| Topics.TopicStatistics | Topics.UserId -> Users.Id | FK_TopicStatistics_UserId -> Users.Id |

## UserInfos
> Суть таблицы - основные видимые данные пользователя, имя, описание, картинка профиля и прочее
#### UserInfos:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Nickname | varchar(100) | Это поле представляет собой отображаемое имя | UNIQUE, NOT NULL, CHECK(0 > n < 100) |
|  | ImageUri | varchar(MAX) | Это поле является путем к картинке |  |
|  | Additional | varchar(256) | Это поле есть описание своего профиля | CHECK(0 > n < 256) |
| FK | UserLinkId | GUID | Это поле является ссылкой на таблицу множеств социальных сетей пользователя |  |
| FK | RoleId | GUID | Это поле есть уровень доступа к данным | NOT NULL |
| FK | UserId | GUID | Это поле является ссылкой на реквизиты для входа | NOT NULL |
|  | CreationalDate | DateTime | Это поле - дата создания профиля |  |
|  | IsRemoved | BOOLEAN | Это поле является флагом удаления | DEFAULT FALSE |
#### UserInfos:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Identity.Roles | Roles.Id -> UserInfos.RoleId | FK_Identity_UserInfos_RoleId -> Roles.Id |
| Identity.Users | Users.Id -> UserInfos.UserId | FK_Identity_UserInfos_UserId -> Users.Id |
| Identity.UserLinks | UserLinks.Id -> UserInfos.UserLinkId | FK_Identity_UserInfos_UserLinkId -> UserLinks.Id |
