using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MiodOdStaniula.Services;
using Moq;

namespace e_commerce.Tests;

public class EmailServiceTests
{
    private readonly EmailService _emailService;


    public EmailServiceTests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath("/Users/pawelstaniul/e-commerce/backend/")
            .AddJsonFile("appsettings.json")
            .Build();

        var mockLogger = new Mock<ILogger<EmailService>>();

        _emailService = new EmailService(configuration, mockLogger.Object);
    }

    [Fact]
    public async Task SendActivationEmail_Sends_RealEmail()
    {
        // Arrange
        var email = "pstaniul@gmail.com";
        var userId = "user-id-123";
        var name = "Test Paweł";
        var token = "test-token";

        Exception? recordedException = null;

        // Act
        try
        {
            await _emailService.SendActivationEmail(email, userId, name, token);
        }
        catch (Exception ex)
        {
            recordedException = ex;
        }

        // Assert
        Assert.Null(recordedException);
    }
}
