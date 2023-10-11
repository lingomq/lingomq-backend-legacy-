<img src="https://avatars.githubusercontent.com/u/145294458?s=200&v=4" alt="Logo of the project" align="right">

# LingoMq &middot; Database documentation

> Документация базы данных

LingoMq - it is application, which represents opportunity to learns any language by himself with help recording words and repeating them

## Содержание

1. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#%D0%BE%D0%B1%D0%BE%D0%B1%D1%89%D0%B5%D0%BD%D0%BD%D0%BE%D0%B5-%D0%BE%D0%BF%D0%B8%D1%81%D0%B0%D0%BD%D0%B8%D0%B5">Обобщенное описание</a>
2. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#%D0%BB%D0%B8%D1%87%D0%BD%D0%BE%D1%81%D1%82%D0%B8">Личности</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#users">Users</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userinfos">UserInfos</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userroles">UserRoles</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userlinks">UserLinks</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#linktypes">LinkTypes</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userstatistics">UserStatistics</a>
3. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#%D1%81%D0%BB%D0%BE%D0%B2%D0%B0">Слова</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userwords">UserWords</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#languages">Languages</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#wordtypes">WordTypes</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userwordtypes">UserWordTypes</a>
4. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#%D1%82%D0%BE%D0%BF%D0%B8%D0%BA%D0%B8">Топики</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#topics">Topics</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#topicstatistics">TopicStatistics</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#topicstatistictypes">TopicStatisticTypes</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#topiclevels">TopicLevels</a>
5. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#уведомления">Уведомления</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#notifications">Notifications</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#usernotifications">UserNotifications</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#notificationtypes">NotificationTypes</a>
6. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#достижения">Достижения</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#achievements">Achievements</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userachievements">UserAchievements</a>
7. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#оплата">Оплата</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#userfinances">UserFinances</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#finances">Finances</a>
8. <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#статистика">Статистика</a>
    - <a href="https://github.com/lingomq/lingomq-backend/blob/dev/docs/dbDocs.md#appstatistics">AppStatistics</a>
## Обобщенное описание

<img src="https://sun9-20.userapi.com/impg/gzvDs5NMX5TlrjYtf4U_vDfcDtMArkEIhQWT2g/T1lfMx_bJjA.jpg?size=1796x929&quality=96&sign=0ed3b1db05ef781c8c684aa37a97bb47&type=album" alt="Logo of the project" align="center">

> Структура таблиц

В данный момент придерживаемся такой общей структуры 

## Личности

<img src="https://sun9-56.userapi.com/impg/FqsD-RdKiuvblU9jiVmYWfsrXGcLW0bapPGyeA/UxgV9BUCi2E.jpg?size=1340x257&quality=96&sign=ddfb3e2c1bdeaf170c1ccb1883899088&type=album" alt="Logo of the project" align="center">

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

### UserInfos
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
#### UserInfos:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Identity.UserLinks | UserLinks.UserInfoId -> UserInfos.Id | FK_Identity_UserLinks_UserInfoId -> UserInfos.Id |
### UserRoles
> Суть таблицы - хранить в себе допустимые роли пользователей
#### UserRoles:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID  | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Name | varchar(20) | Это поле является названием роли | NOT NULL, UNIQUE, CHECK (0 > n <= 20) |
#### UserRoles:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Identity.UserInfos | UserInfos.RoleId -> UserRoles.Id | FK_Identity_UserInfos_RoleId -> UserRoles.Id |
### UserLinks
> Суть таблицы - хранить в себе ссылки на социальные сети пользователя
#### UserLinks:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | UserInfoId | GUID | Это поле представляет собой ссылку на пользовательскую информацию | NOT NULL |
| FK | LinkId | GUID | Это поле представляет собой ссылку на тип социальной сети | NOT NULL |
|  | Href | varchar(256) | Это поле представляет собой сокращенную ссылку на соц.сеть | NOT NULL |
#### UserLinks:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Identity.UserInfos | UserInfos.UserLinkId -> UserLinks.Id | FK_Identity_UserInfos_UserLinkId -> UserLinks.Id |
#### UserLinks:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Identity.UserInfos | UserLinks.UserInfoId -> UserInfos.Id  | FK_Identity_UserLinks_UserInfoId -> UserInfos.Id |
| Identity.LinkTypes | UserLinks.LinkId -> LinkTypes.Id | FK_Identity_UserLinks_LinkId -> LinkTypes.Id |
### LinkTypes
> Суть таблицы - хранить в себе допустимые ссылки на соц.сети
#### LinkTypes:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Name | varchar(256) | Это поле - наименование социальной сети | NOT NULL, CHECK(0 > n <= 256) |
|  | ShortLink | varchar(256) | Это поле - ссылка на профиль в социальной сети без идентификатора | NOT NULL, CHECK(0 > n <= 256) |
#### LinkTypes:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Identity.UserLinks | UserLinks.LinkId -> LinkTypes.Id  | FK_Identity_UserLinks_LinkId -> LinkTypes.Id |
### UserStatistics
> Суть таблицы - собирать общую информацию об использовании приложения пользователем (сколько часов, добавленных слов, посещенных дней подряд)
#### UserStatistics:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | TotalWords | INT | Это поле - общее количество слов пользователя | NOT NULL DEFAULT 0 |
|  | TotalHours | FLOAT | Это поле - общее количество часов, потраченных в приложении | NOT NULL DEFAULT 0 |
|  | VisitStreak | INT | Это поле - максимальное количество посещений подряд (дни) | NOT NULL DEFAULT 0 |
|  | AvgWords | INT | Это поле - среднее арифметическое от общего количества слов | NOT NULL DEFAULT 0 |
|  | LastUpdateAt | DateTime | Это поле представляет собой дату последнего изменения поля | NOT NULL |
| FK | UserId | GUID | Это поле представляет собой ссылку на пользователя | NOT NULL |
#### UserStatistics:Ссылается на
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Identity.Users | UserStatistics.UserId -> Users.Id  | FK_Identity_UserStatistics_UserId -> Users.Id |
## Слова
<img src="https://sun9-31.userapi.com/impg/5MwCVO5fX5zs8Q35GMx57pI_c8lWk07P6HCehw/1r4HEYrRsp0.jpg?size=947x311&quality=95&sign=a1f5d2d80e33310ffc277bb66214971c&type=album"/>

> Структура зоны слов

Данная зона представляет собой таблицы, работающие с добавленными пользователем словами, языками, разделениями их на категориями и управлением

## Таблицы
### UserWords
> Суть таблицы -> хранить данные о добавленном слове и связывать их с конкретным пользователем
#### UserWords:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Word | varchar(256) | Это поле - конкретное слово | NOT NULL CHECK(0 > n <= 256) |
|  | Translated | varchar(256) | Это поле - перевод слова | CHECK(0 > n <= 256) |
| FK | LanguageId | GUID | Это поле - ссылка на язык слова | NOT NULL |
| FK | UserWordTypeId | GUID | Это поле - ссылка на тип слова |  |
|  | Repeats | INT | Это поле - количество повторений конкретного слова | DEFAULT 0 |
| FK | UserId | GUID | Это поле представляет собой ссылку на пользователя | NOT NULL |
|  | CreatedAt | DateTime | Это поле - дата добавления слова |  |
#### UserWords:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Words.UserWordTypes | UserWords.UserWordTypeId -> UserWordTypes.Id | FK_Words_UserWords_UserWordTypeId  -> UserWordTypes.Id |
| Words.Languages | UserWords.LanguageId -> Languages.Id | FK_Words_UserWords_LanguageId   -> Languages.Id |
| Identity.Users | UserWords.UserId -> Users.Id | FK_Words_UserWords_UserId ->  Users.Id |
#### UserWords:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Words.UserWordTypes | UserWordTypes.UserWordId -> UserWords.Id | FK_Words_UserWordTypes_UserWordId  -> UserWords.Id |
### Languages
> Суть таблицы -> хранить в себе допустимые языки
#### Languages:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Name | varchar(15) | Это поле - конкретный язык | NOT NULL CHECK(0 > n <= 15) |
#### Languages:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Words.UserWords | UserWords.LanguageId -> Languages.Id | FK_Words_UserWords_LanguageId  -> Languages.Id |
### WordTypes
> Суть таблицы -> хранить в себе допустимые типы слов
#### WordTypes:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Name | varchar(15) | Это поле - конкретный тип | NOT NULL CHECK(0 > n <= 15) |
#### WordTypes:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Words.UserWords | UserWords.WordTypeId -> WordTypes.Id | FK_Words_UserWords_WordTypeId  -> WordTypes.Id |
### UserWordTypes
> Суть таблицы - связать между собой множество слов и множество типов слов
#### UserWordTypes:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | UserWordId | GUID | Это поле - ссылка на добавленное слово | NOT NULL |
| FK | WordTypeId | GUID | Это поле - ссылка на тип слова | NOT NULL |
#### UserWordTypes:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Words.UserWords | UserWords.UserWordTypeId -> UserWordTypes.Id | FK_Words_UserWords_UserWordTypeId  -> UserWordType.Id |
#### UserWordTypes:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Words.UserWordTypes | UserWordTypes.UserWordId -> UserWords.Id | FK_Words_UserWordTypes_UserWordsId  -> UserWords.Id |
| Words.WordTypes | UserWordsType.WordTypeId -> WordTypes.Id | FK_Words_UserWordTypes_WordTypesId  -> WordTypes.Id |
## Топики
<img src="https://sun9-15.userapi.com/impg/kP-V8xDaEdKfiluZGPzYvCdLfagjyP71M6Mx-A/hcULUbPxHG0.jpg?size=620x466&quality=95&sign=957cc44916a9cd4be6469a05693456f4&type=album"/>

> Структура зоны топиков

Данная зона представляет собой таблицы, работающие с топиками, их управлением, статистикой

## Таблицы
### Topics
> Суть таблицы - хранить конкретную информацию о топике
#### Topics:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Title | varchar(50) | Это поле - название топика | NOT NULL CHECK(0 > n <= 50) |
|  | Content | varchar(MAX) | Это поле - контент топика | NOT NULL |
|  | Icon | varchar(MAX) | Это поле является путем для картинки топика |  |
|  | CreationDate | DateTime | Это поле является датой создания топика | NOT NULL |
| FK | LanguageId | GUID | Это поле - ссылка на конкретный язык |  |
| FK | TopicLevelId | GUID | Это поле - ссылка на уровень топика |  |
#### Topics:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Topics.Topics | Topics.TopicLevelId -> TopicLevels.Id | FK_Topics_Topics_TopicLevelId  -> TopicLevels.Id |
| Topics.Topics | Topics.LanguageId -> Words.Languages.Id | FK_Topics_Topics_LanguageId ->  Words.Languages.Id |
#### Topics:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Topics.TopicStatistics | TopicStatistics.TopicId -> Topics.Id | FK_Topics_TopicStatistics_TopicId  -> Topics.Id |
### TopicStatistics
> Суть таблицы - сбор статистики конкретного топика
#### TopicStatistics:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | TopicId | GUID | Это поле - ссылка на топик | NOT NULL |
| FK | UserId | GUID | Это поле - ссылка на конкретного пользователя | NOT NULL |
| FK | StatisticsTypeId | GUID | Это поле - ссылка на тип статистики | NOT NULL |
|  | StatisticsDate | DateTime | Это поле - дата обновления статистики |  |
#### TopicStatistics:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Topics.TopicStatistics | TopicStatistics.TopicId -> Topics.Id | FK_Topics_TopicStatistics_TopicId  -> Topics.Id |
| Topics.TopicStatistics | TopicStatistics.UserId -> Identity.Users.Id | FK_Topics_TopicStatistics_UserId ->  Identity.Users.Id |
| Topics.TopicStatistics | TopicStatistics.StatisticsTypeId ->  TopicStatisticTypes.Id | FK_Topics_TopicStatistics_StatisticsId  -> TopicStatisticTypes.Id |
### TopicStatisticTypes
> Суть таблицы - хранить в себе типы статистики (лайк, дизлайк, просмотр)
#### TopicStatisticTypes:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | Name | varchar(50) | Это поле - название типа | NOT NULL CHECK (0 > n <= 50) |
#### TopicStatisticTypes:Используется в..
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Topics.TopicStatistics | TopicStatistics.StatisticTypeId -> TopicStatisticTypes.Id | FK_Topics_TopicStatistics_StatisticTypeId  -> TopicStatisticTypes.Id |
### TopicLevels
> Суть таблицы - хранить в себе уровни сложности топика
#### TopicLevels:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | Name | varchar(20) | Это поле - название сложности | NOT NULL CHECK (0 > n <= 20) |
#### TopicLevels:Используется в..
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Topics.TopicStatistics | TopicStatistics.TopicLevelId -> TopicLevels.Id | FK_Topics_TopicStatistics_LevelId  -> TopicLevels.Id |

## Уведомления
<img src="https://sun9-17.userapi.com/impg/Ii9lR5bZM5bxvGQkWxzjlCM7AKkNFrnHpdkMXQ/HgJFpqc3VtA.jpg?size=503x809&quality=95&sign=2b3bec60f5ca5f777b61b7bc64eff059&type=album"/>

> Структура зоны уведомлений

Данная зона представляет собой таблицы, работающие с уведомлениями для пользователей

## Таблицы
### Notifications
> Суть таблицы - хранить в себе конкретное уведомление
#### Notifications:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Title | varchar(50) | Это поле - наименование уведомления | NOT NULL,  CHECK (0 > n <= 50) |
|  | Content | varchar(255) | Это поле - описание уведомления | CHECK (0 > n < 255) |
| FK | NotificationTypeId | GUID | Это поле - ссылка на тип уведомления | NOT NULL |
#### Notifications:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Notifications.UserNotifications | UserNotifications.NotificationId  -> Notifications.Id | FK_Notifications_UserNotifications_NotificationId -> Notifications.Id |
#### Notifications:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Notifications.Notifications | NotificationTypes.NotificationTypeId  -> NotificationTypes.Id | FK_Notifications_NotificationTypes_NotificationTypeId -> NotificationTypes.Id |
### UserNotifications
> Суть таблицы - хранить в себе уведомления пользователей
#### UserNofifications:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | UserId | GUID | Это поле - ссылка на пользователя | NOT NULL |
| FK | NotificationId | GUID | Это поле - ссылка на уведомление | NOT NULL |
|  | DateOfReceipt | DateTime | Это поле - дата прихода уведомления | NOT NULL |
|  | IsReaded | BOOLEAN | Это поле определяет, прочитано ли уведомление |  |
#### UserNotifications:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Notifications.UserNotifications | Notifications.NotificationId  -> Notifications.Id | FK_Notifications_Notifications_NotificationsId -> Notifications.Id |
| Notifications.UserNotifications | Notifications.UserId -> Identity.Users.Id | FK_Notifications_UserNotifications_UserId ->  Identity.Users.Id |
### NotificationTypes
> Суть таблицы - хранить в себе типы уведомлений
#### TopicLevels:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | Name | varchar(15) | Это поле - название типа | NOT NULL CHECK (0 > n <= 15) |
#### TopicLevels:Используется в..
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Notifications.Notifications | NotificationTypes.NotificationTypeId  -> NotificationTypes.Id | FK_Notifications_NotificationTypes_NotificationTypeId -> NotificationTypes.Id |

## Достижения
<img src="https://sun9-4.userapi.com/impg/BB_iplVbgZ3k_z6a0oZrbtDUtpBXPiNSkD_irA/G45WN1pHxgA.jpg?size=475x478&quality=95&sign=32fe67df735f302bb61808c58500be51&type=album"/>

> Структура зоны достижений

Данная зона представляет собой таблицы, работающие с достижениями

## Таблицы
### Achievements
> Суть таблицы - хранить в себе конкретное достижение
#### Achievements:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Name | varchar(50) | Это поле - название достижения | NOT NULL  CHECK (0 > n <= 50) |
|  | Content | varchar(100) | Это поле - описание достижения | NOT NULL CHECK (0 > n <= 100) |
|  | ImageUri | varchar(max) | Это поле - иконка достижения |  |
#### Achievements:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Achievements.UserAchievements | UserAchievements.AchievementId ->  Achievement.Id | FK_Achievements_UserAchievements_AchievementId ->  Achievement.Id |
### UserAchievements
> Суть таблицы - хранить в себе все достижения пользователя
#### UserAchievements:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | UserId | GUID | Это поле - ссылка на пользователя | NOT NULL |
| FK | AchievementId | GUID | Это поле - ссылка на достижение | NOT NULL |
|  | DateOfReceipt | DateTime | Это поле - дата получения достижения |  |
#### UserAchievements:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Achievements.UserAchievements | UserAchievements.AchievementId ->  Achievement.Id | FK_Achievements_UserAchievements_AchievementId ->  Achievement.Id |
| Achievements.UserAchievements | UserAchievements.UserId ->  Identity.Users.Id | FK_Achievements_UserAchievements_UserId ->  Identity.Users.Id |

## Оплата
<img src="https://sun9-61.userapi.com/impg/0iuAnee_DYulsykUv8ygMdewZotBexML3V_mGA/wLp1FwniQzc.jpg?size=749x728&quality=95&sign=7484225a354735f1925e6d91bc7d0c6d&type=album"/>

> Структура зоны оплаты

Данная зона представляет собой таблицы, сохраняющими историю об оплате, конкретную оплату

## Таблицы
### UserFinances
> Суть таблицы - хранить в себе историю о платежах
#### UserFinances:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
| FK | UserId | GUID | Это поле - ссылка на конкретного пользователя | NOT NULL  |
| FK | FinanceId | GUID | Это поле - ссылка на конкретную платную услугу | NOT NULL |
|  | CreationDate | DateTime | Это поле - дата оплаты |  |
|  | SubscriptionEndDate | DateTime | Это поле - дата оплаты |  |
#### UserFinances:Ссылается на...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Finances.UserFinances | UserFinances.FinancetId ->  Finances.Id | FK_Finances_UserFinances_FinanceId -> Finances.Id |
| Finances.UserFinances | UserFinances.UserId ->  Identity.Users.Id | FK_Finances_UserFinances_UserId ->  Identity.Users.Id |
### Finances
> Суть таблицы - хранить в себе конкретную платную услугу
#### Finances:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | Name | varchar(50) | Это поле - название услуги | NOT NULL CHECK (0 > n <= 50) |
|  | Description | varchar(256) | Это поле - описание услуги | NOT NULL CHECK (0 > n <= 256) |
|  | Amount | FLOAT | Это поле - стоимость услуги |  |
#### Finances:Используется в...
| Зона.Таблица | Ключи | Названия |
|---|---|---|
| Finances.UserFinances | UserFinances.FinancetId ->  Finances.Id | FK_Finances_UserFinances_FinanceId -> Finances.Id |

## Статистика
<img src="https://sun9-71.userapi.com/impg/Mi5vIexhbFGy17lpBVFlOHsY3Sw-HpiqRSkkKg/DA9yHXUzxaY.jpg?size=759x325&quality=95&sign=11b4e2ed5fdb9655ea2833407e39d3a1&type=album"/>

> Структура зоны статистики

Данная зона представляет собой сбор информации о нашем приложении, скачиваний всего и т.д и т.п

## Таблицы
### AppStatistics
> Суть таблицы - хранить в себе статистику приложения на конкретный момент времени (наподобии снапшотов)
#### AppStatistics:Структура
| Key | Name | DataType | Description | Constrains |
|---|---|---|---|---|
| PK | Id | GUID | Это поле представляет собой уникальный ключ | UNIQUE, NOT NULL |
|  | TotalUsers | INT | Это поле - общее количество пользователей |  |
|  | AvgTime | FLOAT | Это поле - среднее количество проведенных часов в приложении |  |
|  | TotalWords | INT | Это поле - количество всех добавленных слов |  |
|  | Downloads | INT | Это поле - количество скачиваний |  |
