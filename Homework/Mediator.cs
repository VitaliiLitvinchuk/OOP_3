using Microsoft.Extensions.DependencyInjection;
using Task.Homework13.Abstracts;
using Task.Homework13.Contracts;
using Task.Homework13.Implementation;
using Xunit;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework13 : ITask
    {

        public void Start()
        {
            Console.WriteLine("Mediator");
        }
    }

    public class MediatorTests
    {
        public static int notificationsExecuted = 0;
        private readonly IServiceProvider _serviceProvider;

        public MediatorTests()
        {
            var services = new ServiceCollection();

            services.AddSingleton<Mediator>();
            services.AddTransient<IRequestHandler<TestRequest, string>, TestRequestHandler>();
            services.AddTransient<INotificationHandler<TestNotification>, TestNotificationHandler>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Another test")]
        public async System.Threading.Tasks.Task Send_ValidRequest_ShouldInvokeHandler(string payload)
        {
            // Arrange
            var mediator = _serviceProvider.GetRequiredService<Mediator>();
            var request = new TestRequest { Payload = payload };

            // Act
            var result = await mediator.Send<TestRequest, string>(request);

            // Assert
            Assert.Equal($"Processed: {payload}", result);
        }

        [Fact]
        public async System.Threading.Tasks.Task Publish_Notification_ShouldIgnoreException()
        {
            // Arrange
            int executed = MediatorTests.notificationsExecuted;
            var mediator = _serviceProvider.GetRequiredService<Mediator>();
            var notification = new TestNotification { Message = "Test notification" };

            // Act
            await mediator.Publish(notification);

            // Assert
            Assert.Equal(executed + 1, MediatorTests.notificationsExecuted);
        }

        public class TestRequest : IRequest<string>
        {
            public string? Payload { get; set; }
        }

        public class TestRequestHandler : IRequestHandler<TestRequest, string>
        {
            public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
            {
                return System.Threading.Tasks.Task.FromResult($"Processed: {request.Payload}");
            }
        }

        public class TestNotification : INotification
        {
            public string? Message { get; set; }
        }

        public class TestNotificationHandler : INotificationHandler<TestNotification>
        {
            public System.Threading.Tasks.Task Handle(TestNotification notification, CancellationToken cancellationToken)
            {
                MediatorTests.notificationsExecuted++;
                throw new NotImplementedException();
            }
        }
    }
}
