﻿using System;
using System.Collections.Generic;

namespace Shuttle.Recall.Core.Tests
{
	public class EventStore : IEventStore
	{
		private readonly Dictionary<Guid, EventStream> _eventStreams = new Dictionary<Guid, EventStream>();
		private readonly Dictionary<Guid, Event> _snapshots = new Dictionary<Guid, Event>();

		public EventStream Get(Guid id)
		{
			if (!_eventStreams.ContainsKey(id))
			{
				_eventStreams.Add(id, new EventStream(id, 0, new List<Event>(), FindSnapshot(id)));
			}

			return _eventStreams[id];
		}

		private Event FindSnapshot(Guid id)
		{
			return _snapshots.ContainsKey(id)
				? _snapshots[id]
				: null;
		}

		public EventStream GetRaw(Guid id)
		{
			if (!_eventStreams.ContainsKey(id))
			{
				_eventStreams.Add(id, new EventStream(id, 0, new List<Event>(), null));
			}

			return _eventStreams[id];
		}

		public void SaveEventStream(EventStream eventStream)
		{
			if (_eventStreams.ContainsKey(eventStream.Id))
			{
				_eventStreams.Remove(eventStream.Id);
			}

			_eventStreams.Add(eventStream.Id, new EventStream(eventStream.Id, eventStream.Version, eventStream.EventsAfter(0), eventStream.Snapshot));

			if (!eventStream.HasSnapshot)
			{
				return;
			}

			if (_snapshots.ContainsKey(eventStream.Id))
			{
				_snapshots.Remove(eventStream.Id);
			}

			_snapshots.Add(eventStream.Id, eventStream.Snapshot);
		}
	}
}