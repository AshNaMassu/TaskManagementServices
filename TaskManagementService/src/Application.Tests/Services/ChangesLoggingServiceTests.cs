using Application.DTO.Logging;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Services
{
    public class ChangesLoggingServiceTests
    {
        private readonly Mock<ILogger<ChangesLoggingService>> _loggerMock;
        private readonly Mock<IActivityLogSenderService> _activityLogSenderMock;
        private readonly ChangesLoggingService _service;

        public ChangesLoggingServiceTests()
        {
            _loggerMock = new Mock<ILogger<ChangesLoggingService>>();
            _activityLogSenderMock = new Mock<IActivityLogSenderService>();
            _service = new ChangesLoggingService(_loggerMock.Object, _activityLogSenderMock.Object);
        }

        [Fact]
        public async Task Log_ShouldSendMessageAndLogInformation_WhenCalled()
        {
            // Arrange
            var testMessage = new EntityChangedMessage
            {
                Entity = "Task",
                EntityId = 123,
                Operation = "Create"
            };

            _activityLogSenderMock
                .Setup(x => x.SendMessage(It.IsAny<EntityChangedMessage>()))
                .Returns(Task.CompletedTask);

            var logMessage = $"An action to change DB data occurred. Entity = {testMessage.Entity}, EntityId = {testMessage.EntityId}, Operation = {testMessage.Operation}";

            // Act
            await _service.Log(testMessage);

            // Assert
            _activityLogSenderMock.Verify(
                x => x.SendMessage(It.Is<EntityChangedMessage>(m =>
                    m.Entity == testMessage.Entity &&
                    m.EntityId == testMessage.EntityId &&
                    m.Operation == testMessage.Operation)),
                Times.Once);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(logMessage)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Theory]
        [InlineData(null, 123, "Create")]
        [InlineData("Task", 123, null)]
        [InlineData("", 123, "Create")]
        [InlineData("Task", 0, "Create")]
        public async Task Log_ShouldThrowArgumentException_WhenMessagePropertiesAreInvalid(string entity, long entityId, string operation)
        {
            // Arrange
            var invalidMessage = new EntityChangedMessage
            {
                Entity = entity,
                EntityId = entityId,
                Operation = operation
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.Log(invalidMessage));
        }
    }
}
