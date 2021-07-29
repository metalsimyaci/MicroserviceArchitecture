using System;

namespace EventBusRabbitMq.Events.Abstracts
{
	public abstract class EventBase
	{
		public Guid RequestId { get; private init; }
		public DateTime CreationDate { get; private init; }

		public EventBase()
		{
			RequestId = Guid.NewGuid();
			CreationDate = DateTime.UtcNow;
		}

	}
}
