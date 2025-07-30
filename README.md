# Tic Tac Toe NxN REST API (.NET 9 + PostgreSQL)

## Архитектура

Проект разделен на несколько слоев:

- **TTT.Core** — бизнес-логика и модели
- **TTT.Data** — доступ к данным (EF Core)
- **TTT.Services** — сервисный слой с логикой игры
- **TTT.Api** — REST API на ASP.NET Core

Используется DI, конфигурация через `IOptions`, слои взаимодействуют через интерфейсы. Для удобства тестирования был добавлен Swagger, который доступен при запуске приложения по адресу: `http://localhost:8080/index.html`. Также были добавлены тестовые данные, которые инициализируются при запуске приложения. Настроить их можно в файле: `TTT.Data/Development/Data/SeedPlayers.json`.

## API

### Healthcheck

`GET /health`

→ 200 OK

### Создать игру

`POST /api/game`

**Тело запроса:**

```json
{
  "playerXId": "guid",
  "playerOId": "guid"
}
```

### Сделать ход

`POST /api/game/{gameId}/moves`

**Тело запроса:**

```json
{
  "playerId": "guid",
  "playerSign": "X",
  "position": {
    "row": 1,
    "column": 2
  }
}
```

### Получить состояние игры

`GET /api/game/{gameId}`

**Тело запроса:**

```json
{
  "gameId": "guid"
}
```

## Запуск приложения

Для запуска приложения нужно клонировать данный репозиторий на свою рабочую станцию: `git clone https://github.com/topoff0/internship-entry-task`. Далее нужно создать в корне проекта файл `.env` и скопировать в него данные из `.env.example`. После чего можно запустить проект командой `docker compose up --build -d`. Swagger будет доступен по адресу: `http://localhost:8080`.

## Тестирование

Для запуска всех тестов (интеграционные и unit) нужно выполнить команду `dotnet test` из корня проекта. Также, согласно условиям был добавлен запуск тестов в CI. Проверить это можно, создав и запушив тестовый коммит и перейдя во вкладку Actions вашего репозитория на GitHub. Настройку CI можно посмотреть в `.github/workflows/ci.yml`
