namespace BuildingBlocks.Messaging.Events;

public class IntegrationEvent
{
    public Guid Id => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName ?? "GENERIC_EVENT";
}
