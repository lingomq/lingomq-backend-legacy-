<img src="https://avatars.githubusercontent.com/u/145294458?s=200&v=4" alt="Logo of the project" align="right">

# LingoMq &middot; Database documentation

> Документация базы данных

LingoMq - it is application, which represents opportunity to learns any language by himself with help recording words and repeating them

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

#### Users:Структура
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


