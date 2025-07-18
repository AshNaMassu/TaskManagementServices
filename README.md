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

## 📁 Структура проекта
TaskManagementService/
├── src/
│ ├── API/ # Веб-API (точка входа), валидация входных моделей
│ ├── Application/ # Бизнес-логика приложения, валидация данных, формирование ответов и сообщений об изменениях
│ ├── Domain/ # Доменные модели
│ ├── Infrastructure/ # Инфраструктурные компоненты (Kafka, Refit (Http)) для отправки сообщений об изменениях
│ └── Persistence/ # EF Core, DbContext, репозитории, миграции
├── Dockerfile

Аналогична структура проекта TaskManagementActivitiesLogging.

