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

1. Клонируйте репозиторий на локальную машину:

   ```bash
   git clone https://github.com/topoff0/internship-entry-task
   ```

2. В корне проекта создайте файл `.env` и скопируйте в него содержимое из `.env.example`.

3. Запустите проект с помощью команды:

   ```bash
   docker compose up --build -d
   ```

После запуска API будет доступен по адресу: [http://localhost:8080](http://localhost:8080), где также будет доступна документация Swagger.

---

## Тестирование

Для запуска всех unit- и интеграционных тестов выполните из корня проекта:

```bash
dotnet test
```

В рамках задания также настроен CI, который автоматически запускает тесты при каждом коммите.  
Результаты выполнения можно просмотреть во вкладке **Actions** на странице репозитория на GitHub.

Файл конфигурации CI находится по пути:

```
.github/workflows/ci.yml
```
