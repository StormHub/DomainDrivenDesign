namespace Domain.Infrastructure.Tests.Messaging.Mediator
{
    public class TestInspector
    {
        public bool FiredHandler1 { get; set; } = false;

        public bool FiredHandler2 { get; set; } = false;
    }
}