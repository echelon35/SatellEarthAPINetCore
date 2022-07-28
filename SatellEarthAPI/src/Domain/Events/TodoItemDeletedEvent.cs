﻿namespace SatellEarthAPI.Domain.Events
{
    public class TodoItemDeletedEvent : BaseEvent
    {
        public TodoItemDeletedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}