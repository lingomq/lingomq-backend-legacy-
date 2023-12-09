# Бекенд приложения LingoMq

```
Дизайн приложения и прочее вы можете посмотреть в wiki
```

## Как запустить?
Для запуска бекенда с целью тестирования приложения, достаточно иметь [Docker](https://www.docker.com/products/docker-desktop/) и используя docker-compose файл 'docker-compose.dev.yaml',  все крутится в них. <br/>
<b>Команда для запуска</b>
```
# ~/lingomq-backend/src

docker-compose -f docker-compose.dev.yaml --verbose up --build
```
Но этого недостаточно, также вам нужно иметь .env файлы.<br/>
<b>Список полей .env файла</b>
```
JWT__Issuer="JWT Credentials"
JWT__Audience="JWT Credentials"
JWT__Key="JWT Credentials"
JWT__ExpiredMinutesAccessToken="JWT Credentials"
JWT__ExpiredMinutesRefreshToken="JWT Credentials"

RabbitMq__Uri=rabbitmq
RabbitMq__UserName="Rabbit mq имя"
RabbitMq__Password="Rabbit mq пароль"

Mail__From="Reply"
Mail__Corp=""
Mail__Smtp__Client="Клиент от почтового сервиса"
Mail__Smtp__Port="Порт от почтового сервиса"
Mail__Credentials__Client="Логин от почтового сервиса"
Mail__Credentials__Password="Пароль от почтового сервиса"

ConnectionStrings__MongoDb="Строка подключения к бд"

ConnectionStrings__Dev__Achievements="Строка подключения к бд"
ConnectionStrings__Dev__Authentication="Строка подключения к бд"
ConnectionStrings__Dev__Finances="Строка подключения к бд"
ConnectionStrings__Dev__Identity="Строка подключения к бд"
ConnectionStrings__Dev__Notification="Строка подключения к бд"
ConnectionStrings__Dev__Topics="Строка подключения к бд"
ConnectionStrings__Dev__Words="Строка подключения к бд"
Keys__SpellCheckerApiKey="Ключ к сервису api.textgears.com"

```
<b>Список полей postgres.env файла</b>
```
POSTGRES_USER: "Имя пользователя postgres"
POSTGRES_HOST: "psql"
POSTGRES_PASSWORD: "Пароль пользователя postgres"
PGDATA: "/var/lib/postgresql/data/pgdata"
```

## Формат запроса
Мы используем микросервисную архитектуру, поэтому у нас есть api-шлюз, который распологается по адресу:
```
http://localhost/api.lingomq/{microservice}
```
На данный момент не вижу смысла все досконально документировать, так как репозиторий не пользуется спросом, но следующее все же отметить стоит:<br/>
<b>Формат ответа: JSON. Отправляемый статус: всегда сотые (100, 200, 300, 400, 500)</b>
```
{
  "code": 0, // код ответа, на самом деле соответствует http статусам,
  "message": "some message",
  "data": {
    "some": "data",
    "if": "exists",
    "then": "it exists too"
  },
  "errors": {
    "if": "exists",
    "then": "it exists too"
  }
}
```
