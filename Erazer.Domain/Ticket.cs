﻿using System;
using Erazer.Domain.Constants;
using Erazer.Domain.Events;
using Erazer.Framework.Domain;
using System.Collections.Generic;

namespace Erazer.Domain
{
    public class Ticket : AggregateRoot
    {
        private string _title;
        private string _description;
        private string _priorityId;
        private string _statusId;
        private List<string> _comments = new List<string>();

        private const string DefaultStatusId = StatusConstants.Backlog;

        #region Constructors
        // Need of a parameterless constructor to build Aggregate from events!

        // Constructor used for creating a new ticket!
        public Ticket(Guid id, string title, string description, string priorityId, Guid creatorUserId) : this()
        {
            ApplyChange(new TicketCreateEvent(id)
            {
                Title = title,
                Description = description,
                UserId = creatorUserId,
                PriorityId = priorityId,
                StatusId = DefaultStatusId
            });
        }

        private Ticket() 
        {
            // Register EventHandlers
            Handles<TicketCreateEvent>(Apply);
            Handles<TicketPriorityEvent>(Apply);
            Handles<TicketStatusEvent>(Apply);
        }
        #endregion

        #region Events
        // Events are 'mutations' that happened in the past!
        // This blocks of code actually handle the business logic and will mutate the state of this domain class.
        // They are executed when a command arrives or when the aggregate is loaded from previous events!
        private void Apply(TicketCreateEvent e)
        {
            Id = e.AggregateRootId;

            _title = e.Title;
            _description = e.Description;
            _priorityId = e.PriorityId;
            _statusId = e.StatusId;
        }

        private void Apply(TicketPriorityEvent e)
        {
            _priorityId = e.ToPriorityId;
        }

        private void Apply(TicketStatusEvent e)
        {
            _statusId = e.ToStatusId;
        }

        #endregion

        #region Domain methods
        /// <summary>
        /// These methods will appy events to our domain This code should use a ubiquitous language.
        /// This will make sure even non developers will be able to read the method signature easy!
        /// 
        /// They also handle domain validation
        /// E.g. A ticket cannot be 'done' when it never was 'in progress'
        /// </summary>

        public void AddComment(string comment, Guid commenterId)
        {
            _comments.Add(comment);

            ApplyChange(new TicketCommentEvent
            {
                Comment = comment,
                UserId = commenterId
            });
        }

        public void UpdatePriority(string newPriorityId, Guid userId)
        {
            if (newPriorityId == _priorityId)
                return;

            ApplyChange(new TicketPriorityEvent
            {
                FromPriorityId = _priorityId,
                ToPriorityId = newPriorityId,
                UserId = userId
            });
        }

        public void UpdateStatus(string newStatusId, Guid userId)
        {
            if (newStatusId == _statusId)
                return;

            ApplyChange(new TicketStatusEvent
            {
                FromStatusId = _statusId,
                ToStatusId = newStatusId,
                UserId = userId
            });
        }
        #endregion
    }
}
