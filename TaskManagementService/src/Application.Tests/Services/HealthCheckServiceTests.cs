using Application.DTO.HealthCheck;
using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Moq;

namespace Application.Tests.Services
{
    public class HealthCheckServiceTests
    {
        private readonly Mock<IHealthCheckRepository> _repoMock;
        private readonly Mock<IInfrastructureInfoService> _infraMock;
        private readonly HealthCheckService _service;

        public HealthCheckServiceTests()
        {
            _repoMock = new Mock<IHealthCheckRepository>();
            _infraMock = new Mock<IInfrastructureInfoService>();
            _service = new HealthCheckService(_repoMock.Object, _infraMock.Object);
        }

        [Fact]
        public async Task CheckHealthAsync_ReturnsInfraFailure_WhenInfraUnhealthy()
        {
            // Arrange
            var infraFailure = new HealthCheck
            {
                Status = HealthCheckStatus.Failed,
                Message = "Kafka unavailable"
            };

            _infraMock.Setup(x => x.HealthCheckAsync())
                .ReturnsAsync(infraFailure);

            // Act
            var result = await _service.CheckHealthAsync();

            // Assert
            Assert.Equal(HealthCheckStatus.Failed, result.Status);
            Assert.Equal("Kafka unavailable", result.Message);
            _repoMock.Verify(x => x.CheckHealthDbAsync(), Times.Never);
        }

        [Fact]
        public async Task CheckHealthAsync_ReturnsDbStatus_WhenInfraHealthy()
        {
            // Arrange
            var healthyInfra = new HealthCheck { Status = HealthCheckStatus.Ok };
            var dbFailure = new HealthCheck
            {
                Status = HealthCheckStatus.Failed,
                Message = "DB connection error"
            };

            _infraMock.Setup(x => x.HealthCheckAsync())
                .ReturnsAsync(healthyInfra);

            _repoMock.Setup(x => x.CheckHealthDbAsync())
                .ReturnsAsync(dbFailure);

            // Act
            var result = await _service.CheckHealthAsync();

            // Assert
            Assert.Equal(HealthCheckStatus.Failed, result.Status);
            Assert.Equal("DB connection error", result.Message);
        }

        [Fact]
        public async Task CheckHealthAsync_ReturnsSuccess_WhenAllSystemsOk()
        {
            // Arrange
            var healthyInfra = new HealthCheck { Status = HealthCheckStatus.Ok };
            var healthyDb = new HealthCheck { Status = HealthCheckStatus.Ok };

            _infraMock.Setup(x => x.HealthCheckAsync())
                .ReturnsAsync(healthyInfra);

            _repoMock.Setup(x => x.CheckHealthDbAsync())
                .ReturnsAsync(healthyDb);

            // Act
            var result = await _service.CheckHealthAsync();

            // Assert
            Assert.Equal(HealthCheckStatus.Ok, result.Status);
        }
    }
}
