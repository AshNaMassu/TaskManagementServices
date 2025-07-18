# Task Management Service

Проект реализует API для управления задачами пользователя на основе .NET 8 с использованием Entity Framework Core и PostgreSQL.
Система состоит из двух сервисов: 
- **TaskManagementService** - создание, изменение, удаление и просмотр задач пользователя. При создании/изменении/удалении задачи отправляет сообщея об изменившйся сущности как по синхронно (Http), так и асинхронно (KafkaProducer). Режим отправки сообщений задается в настройках сервиса параметром.
```bash
LogSender": "kafka" //kafka or http
```
Принимающим значения: *kafka* - для асинхронной передачи. Отсутвие настройки или указание параметра отличного от *kafka* приведет к синхронной отправке сообщений.

- **TaskManagementActivitiesLogging** - логирование изменений по задачам с возможностью просмотра и удаления логов. Принимает сообщения об изменениях сущностей как синхронно (WebAPI), так и асинхронно (KafkaConsumer). Работает одновременно в обоих режимах.

## 📦 Технологии

- **.NET 8**
- **Entity Framework Core** (Code First + Migrations)
- **Kafka**
- **REST API**
- **PostgreSQL**
- **Docker** (для контейнеризации)
- **ASP.NET Core Web API**
- **Automapper**
- **FluentValidation**
- **Refit**
- **Serilog**
- **Swagger**

## 📁 Структура проектов
**TaskManagementService** и **TaskManagementActivitiesLogging** построены на Clean Architecture с разделением на слои:

- **API** - WebAPI (точка входа), валидация входных моделей
- **Application**- Бизнес-логика приложения, валидация данных, формирование ответов и сообщений об изменениях
- **Domain** - Доменные модели
- **Infrastructure** - Инфраструктурные компоненты (Kafka, Refit (Http)) для отправки сообщений об изменениях
- **Persistence** - Работа с базой данных: EF Core, DbContext, репозитории, миграции








## 📁 Миграции
Миграции применяются автоматически при старте приложения:
```csharp
public static IApplicationBuilder ApplyMigration(this WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
        dbContext.Database.Migrate();
    }

    return app;
}
```

Если нужно создать новую миграцию:
```bash
dotnet ef migrations add InitialCreate --project src/Persistence/Persistence.csproj --startup-project src/API/API.csproj --context DataBaseContext
```

## 📎 Конфигурация
Настройки подключения к БД находятся в appsettings.json:
```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=activities_logging;Username=postgres;Password=password"
  }
```

## 🛠 Установка и запуск

### 🧪 Docker Compose
- Перейти в корневую директорию, где содержится файл docker-compose.yml
- Выполнить команды
```bash
docker-compose build
docker-compose up
```

- Запускает проекты TaskManagementService и TaskManagementActivitiesLogging, базу данных PostgreSQL, брокер сообщений Kafka:
```yaml
version: '3.8'

networks:
  app-network:
    driver: bridge

services:
  zookeeper:
    image: wurstmeister/zookeeper:latest
    ports:
      - "2181:2181"
    networks:
      - app-network

  kafka:
    image: wurstmeister/kafka:latest
    ports:
      - "9092:9092"
    expose:
      - "9093"
    environment:
      KAFKA_ADVERTISED_LISTENERS: INSIDE://kafka:9093,OUTSIDE://localhost:9092
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: INSIDE:PLAINTEXT,OUTSIDE:PLAINTEXT
      KAFKA_LISTENERS: INSIDE://0.0.0.0:9093,OUTSIDE://0.0.0.0:9092
      KAFKA_INTER_BROKER_LISTENER_NAME: INSIDE
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_CREATE_TOPICS: "activity-log-topic:1:1"
      KAFKA_AUTO_CREATE_TOPICS_ENABLE: "false"
      KAFKA_CFG_AUTO_CREATE_TOPICS_ENABLE: "false"
    networks:
      - app-network
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock

  task-management-activities-logging:
    build:
      context: ./TaskManagementActivitiesLogging
      dockerfile: Dockerfile
    ports:
      - "5176:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - KafkaConfigurationOptions__BootstrapServers=kafka:9093
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=activities_logging;Username=postgres;Password=3216733
    depends_on:
      db:
        condition: service_healthy
    networks:
      - app-network

  task-management-service:
    build:
      context: ./TaskManagementService
      dockerfile: Dockerfile
    ports:
      - "5175:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - KafkaConfigurationOptions__BootstrapServers=kafka:9093
      - ITaskManagementActivitiesLoggingHttpClient=http://task-management-activities-logging:80
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=activities_logging;Username=postgres;Password=3216733
    depends_on:
      db:
        condition: service_healthy
    networks:
      - app-network

  db:
    image: postgres:16-alpine
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: 3216733
      POSTGRES_DB: activities_logging
    ports:
    - "5432:5432"
    networks:
    - app-network
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres -d activities_logging"]
      interval: 5s
      timeout: 5s
      retries: 10
```

### 🔧 Локальный запуск

- Проверить значения в `appsettings.json`, чтобы они соответствовали окружению, настроенному на вашей машине.
- Перейти в корневые директории каждого проекта (к файлу .sln) и выполнить для каждого команды

```bash
# Восстановить зависимости
dotnet restore

# Собрать проект
dotnet build

# Запустить приложение
dotnet run --project src/API/API.csproj
```