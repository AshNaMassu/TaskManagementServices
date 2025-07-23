using Application.DTO.Logging;
using Application.DTO.Task;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Xunit.Sdk;

namespace Application.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepoMock;
        private readonly Mock<ILogger<TaskService>> _loggerMock;
        private readonly Mock<IChangesLoggingService> _loggingServiceMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _loggerMock = new Mock<ILogger<TaskService>>();
            _loggingServiceMock = new Mock<IChangesLoggingService>();
            _taskService = new TaskService(
                _taskRepoMock.Object,
                _loggerMock.Object,
                _loggingServiceMock.Object);
        }

        #region CreateAsync
        [Fact]
        public async Task CreateAsync_ReturnsSuccessWithId_WhenCreationSucceeds()
        {
            // Arrange
            var request = new CreateTaskRequest { Title = "Test", Description = "Desc" };
            var expectedId = 1L;

            _taskRepoMock.Setup(x => x.CreateAsync(request))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _taskService.CreateAsync(request);

            // Assert
            Assert.Equal(MethodResultType.Success, result.ResultType);
            Assert.Equal(expectedId, result.Value);

            _loggingServiceMock.Verify(x => x.Log(It.Is<EntityChangedMessage>(m =>
                m.Operation == nameof(TaskService.CreateAsync) &&
                m.Entity == nameof(TaskEntity) &&
                m.EntityId == expectedId)),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_LogsErrorAndReturnsInternalError_WhenRepositoryThrows()
        {
            // Arrange
            var request = new CreateTaskRequest { Title = "Test", Description = "Desc" };
            var exceptionMessage = "DB failure";
            var exception = new Exception(exceptionMessage);
            var errorMessage = GetErrorMessage(nameof(TaskService.CreateAsync), $"Failed to create task: {exceptionMessage}");

            _taskRepoMock.Setup(x => x.CreateAsync(request))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.CreateAsync(request);

            // Assert
            Assert.Equal(MethodResultType.InternalError, result.ResultType);
            Assert.Contains(exceptionMessage, result.Error);

            VerifyErrorWasLogged(errorMessage);
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task DeleteAsync_ReturnsTask_WhenExists()
        {
            // Arrange
            var taskId = 1L;

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.DeleteAsync(taskId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskService.DeleteAsync(taskId);

            // Assert
            Assert.Equal(MethodResultType.Success, result.ResultType);
            _taskRepoMock.Verify(x => x.ExistsById(taskId), Times.Once);
            _taskRepoMock.Verify(x => x.DeleteAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFound_WhenTaskNotExists()
        {
            // Arrange
            var taskId = 999L;

            var errorMessage = GetErrorMessage(nameof(TaskService.DeleteAsync), $"{nameof(TaskEntity)} with id {taskId} not found");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(false);

            // Act
            var result = await _taskService.DeleteAsync(taskId);

            // Assert
            Assert.Equal(MethodResultType.NotFound, result.ResultType);
            Assert.Contains("not found", result.Error);
            _taskRepoMock.Verify(x => x.DeleteAsync(It.IsAny<long>()), Times.Never);
            VerifyErrorWasLogged(errorMessage);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFoundError_WhenRepositoryNotFoundExceptionFails()
        {
            // Arrange
            var taskId = 1L;
            var excMessage = $"{nameof(TaskEntity)} with id {taskId} not found";
            var exception = new NotFoundException(excMessage);
            var errorMessage = GetErrorMessage(nameof(TaskService.DeleteAsync), $"Failed to delete task: {excMessage}");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.DeleteAsync(taskId))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.DeleteAsync(taskId);

            // Assert
            Assert.Equal(MethodResultType.NotFound, result.ResultType);
            Assert.Contains(excMessage, result.Error);
            VerifyErrorWasLogged(errorMessage);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsInternalError_WhenRepositoryFails()
        {
            // Arrange
            var taskId = 1L;
            var excMessage = "DB error";
            var exception = new Exception(excMessage);
            var errorMessage = GetErrorMessage(nameof(TaskService.DeleteAsync), $"Failed to delete task: {excMessage}");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.DeleteAsync(taskId))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.DeleteAsync(taskId);

            // Assert
            Assert.Equal(MethodResultType.InternalError, result.ResultType);
            Assert.Contains("Failed to delete task", result.Error);
            VerifyErrorWasLogged(errorMessage);
        }
        #endregion

        #region GetAsyncMany
        [Fact]
        public async Task GetAsync_ReturnsFilteredTasks_WhenRequestValid()
        {
            // Arrange
            var request = new FilteringTaskRequest
            {
                Title = "Important",
                Limit = 10
            };

            var expectedTasks = new List<TaskResponse>
            {
                new() { Id = 1, Title = "Important task 1" },
                new() { Id = 2, Title = "Important task 2" }
            };

            _taskRepoMock.Setup(x => x.GetAsync(request))
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _taskService.GetAsync(request);

            // Assert
            Assert.Equal(MethodResultType.Success, result.ResultType);
            Assert.Equal(expectedTasks, result.Value);
            _taskRepoMock.Verify(x => x.GetAsync(request), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ReturnsEmptyList_WhenNoMatches()
        {
            // Arrange
            var request = new FilteringTaskRequest
            {
                Title = "Non-existent",
                Status = "Completed"
            };

            _taskRepoMock.Setup(x => x.GetAsync(request))
                .ReturnsAsync(new List<TaskResponse>());

            // Act
            var result = await _taskService.GetAsync(request);

            // Assert
            Assert.Equal(MethodResultType.Success, result.ResultType);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task GetAsyncFilteredTasks_ReturnsInternalError_WhenRepositoryFails()
        {
            // Arrange
            var request = new FilteringTaskRequest();
            var exceptionMessage = "Database timeout";
            var exception = new Exception(exceptionMessage);
            var errorMessage = GetErrorMessage(nameof(TaskService.GetAsync), $"Failed to get task: {exceptionMessage}");

            _taskRepoMock.Setup(x => x.GetAsync(request))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.GetAsync(request);

            // Assert
            Assert.Equal(MethodResultType.InternalError, result.ResultType);
            Assert.Contains("Failed to get task", result.Error);
            VerifyErrorWasLogged(errorMessage);
        }

        #endregion

        #region GetAsyncById

        [Fact]
        public async Task GetAsync_ReturnsTask_WhenExists()
        {
            // Arrange
            var taskId = 1L;
            var expectedTask = new TaskResponse { Id = taskId, Title = "Test" };

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.GetAsync(taskId))
                .ReturnsAsync(expectedTask);

            // Act
            var result = await _taskService.GetAsync(taskId);

            // Assert
            Assert.Equal(MethodResultType.Success, result.ResultType);
            Assert.Equal(expectedTask, result.Value);
            _taskRepoMock.Verify(x => x.ExistsById(taskId), Times.Once);
            _taskRepoMock.Verify(x => x.GetAsync(taskId), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ReturnsNotFound_WhenTaskNotExists()
        {
            // Arrange
            var taskId = 999L;

            var errorMessage = GetErrorMessage(nameof(TaskService.GetAsync), $"{nameof(TaskEntity)} with id {taskId} not found");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(false);

            // Act
            var result = await _taskService.GetAsync(taskId);

            // Assert
            Assert.Equal(MethodResultType.NotFound, result.ResultType);
            Assert.Contains("not found", result.Error);
            _taskRepoMock.Verify(x => x.GetAsync(It.IsAny<long>()), Times.Never);
            VerifyErrorWasLogged(errorMessage);
        }

        [Fact]
        public async Task GetAsync_ReturnsNotFoundError_WhenRepositoryNotFoundExceptionFails()
        {
            // Arrange
            var taskId = 1L;
            var excMessage = $"{nameof(TaskEntity)} with id {taskId} not found";
            var exception = new NotFoundException(excMessage);
            var errorMessage = GetErrorMessage(nameof(TaskService.GetAsync), $"Failed to get task: {excMessage}");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.GetAsync(taskId))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.GetAsync(taskId);

            // Assert
            Assert.Equal(MethodResultType.NotFound, result.ResultType);
            Assert.Contains(excMessage, result.Error);
            VerifyErrorWasLogged(errorMessage);
        }

        [Fact]
        public async Task GetAsync_ReturnsInternalError_WhenRepositoryFails()
        {
            // Arrange
            var taskId = 1L;
            var excMessage = "DB error";
            var exception = new Exception(excMessage);
            var errorMessage = GetErrorMessage(nameof(TaskService.GetAsync), $"Failed to get task: {excMessage}");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.GetAsync(taskId))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.GetAsync(taskId);

            // Assert
            Assert.Equal(MethodResultType.InternalError, result.ResultType);
            Assert.Contains("Failed to get task", result.Error);
            VerifyErrorWasLogged(errorMessage);
        }

        [Fact]
        public async Task GetAsync_AppliesAllFilters_Correctly()
        {
            var complexRequest = new FilteringTaskRequest
            {
                Ids = new[] { 1L, 2L },
                CreatedAtStart = DateTime.UtcNow.AddDays(-1),
                Status = "Active"
            };

            // Проверка что все параметры передаются в репозиторий без изменений
            _taskRepoMock.Setup(x => x.GetAsync(It.Is<FilteringTaskRequest>(r =>
                r.Ids.SequenceEqual(complexRequest.Ids) &&
                r.CreatedAtStart == complexRequest.CreatedAtStart &&
                r.Status == complexRequest.Status)))
                .ReturnsAsync(new List<TaskResponse>());

            await _taskService.GetAsync(complexRequest);
        }

        #endregion

        #region UpdateAsync
        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenUpdateValid()
        {
            // Arrange
            var taskId = 1L;
            var request = new UpdateTaskRequest
            {
                Title = "Updated Title",
                Description = "Updated Desc"
            };

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.UpdateAsync(taskId, request))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskService.UpdateAsync(taskId, request);

            // Assert
            Assert.Equal(MethodResultType.Success, result.ResultType);
            _taskRepoMock.Verify(x => x.UpdateAsync(taskId, request), Times.Once);
            _taskRepoMock.Verify(x => x.ExistsById(taskId), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNotFound_WhenTaskNotExists()
        {
            // Arrange
            var taskId = 999L;
            var request = new UpdateTaskRequest();

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(false);

            // Act
            var result = await _taskService.UpdateAsync(taskId, request);

            // Assert
            Assert.Equal(MethodResultType.NotFound, result.ResultType);
            _taskRepoMock.Verify(x => x.UpdateAsync(It.IsAny<long>(), It.IsAny<UpdateTaskRequest>()), Times.Never);
            VerifyErrorWasLogged($"{nameof(TaskEntity)} with id {taskId} not found");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsInternalError_WhenRepositoryFails()
        {
            // Arrange
            var taskId = 1L;
            var request = new UpdateTaskRequest();
            var exception = new Exception("DB error");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.UpdateAsync(taskId, request))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.UpdateAsync(taskId, request);

            // Assert
            Assert.Equal(MethodResultType.InternalError, result.ResultType);
            Assert.Contains("Failed to update task", result.Error);
            VerifyErrorWasLogged("Failed to update task: DB error");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNotFound_WhenRepositoryThrowsNotFound()
        {
            // Arrange
            var taskId = 1L;
            var request = new UpdateTaskRequest();
            var exception = new NotFoundException("Task deleted");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.UpdateAsync(taskId, request))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.UpdateAsync(taskId, request);

            // Assert
            Assert.Equal(MethodResultType.NotFound, result.ResultType);
            Assert.Contains("Task deleted", result.Error);
        }
        #endregion

        #region UpdateStatusAsync
        [Fact]
        public async Task UpdateStatusAsync_ReturnsSuccess_WhenUpdateValid()
        {
            // Arrange
            var taskId = 1L;
            var request = new UpdateTaskStatusRequest { Status = "Completed" };

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.UpdateStatusAsync(taskId, request))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskService.UpdateStatusAsync(taskId, request);

            // Assert
            Assert.Equal(MethodResultType.Success, result.ResultType);
            _taskRepoMock.Verify(x => x.UpdateStatusAsync(taskId, request), Times.Once);
            _taskRepoMock.Verify(x => x.ExistsById(taskId), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ReturnsNotFound_WhenTaskNotExists()
        {
            // Arrange
            var taskId = 999L;
            var request = new UpdateTaskStatusRequest();

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(false);

            // Act
            var result = await _taskService.UpdateStatusAsync(taskId, request);

            // Assert
            Assert.Equal(MethodResultType.NotFound, result.ResultType);
            _taskRepoMock.Verify(x => x.UpdateStatusAsync(It.IsAny<long>(), It.IsAny<UpdateTaskStatusRequest>()), Times.Never);
            VerifyErrorWasLogged($"{nameof(TaskEntity)} with id {taskId} not found");
        }

        [Fact]
        public async Task UpdateStatusAsync_ReturnsInternalError_WhenRepositoryFails()
        {
            // Arrange
            var taskId = 1L;
            var request = new UpdateTaskStatusRequest();
            var exception = new Exception("DB error");

            _taskRepoMock.Setup(x => x.ExistsById(taskId))
                .ReturnsAsync(true);
            _taskRepoMock.Setup(x => x.UpdateStatusAsync(taskId, request))
                .ThrowsAsync(exception);

            // Act
            var result = await _taskService.UpdateStatusAsync(taskId, request);

            // Assert
            Assert.Equal(MethodResultType.InternalError, result.ResultType);
            Assert.Contains("Failed to update task status", result.Error);
            VerifyErrorWasLogged("Failed to update task status: DB error");
        }
        #endregion




        #region CommonMethods
        private string GetErrorMessage(string method, string message)
        {
            return $"[{nameof(TaskService)}][{method}]: {message}";
        }

        private void VerifyErrorWasLogged(string message)
        {
            _loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
        #endregion
    }
}
