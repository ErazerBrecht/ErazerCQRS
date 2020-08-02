using System;
using System.Collections.Generic;
using Erazer.Domain.Constants;
using Erazer.Domain.Files;
using Erazer.Domain.Ticket.Events;
using Erazer.Framework.Domain;

namespace Erazer.Domain.Ticket
{
    public class Ticket : AggregateRoot
    {
        private const string DefaultStatusId = StatusConstants.Backlog;
        
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string PriorityId { get; private set; }
        public string StatusId { get; private set; }
        public List<string> Comments { get; } = new List<string>();
        public List<File> Files { get; private set; } = new List<File>();

        #region Constructors

        // Constructor used for creating a new ticket!
        public Ticket(string title, string description, string priorityId, List<File> files) : this()
        {
            Id = Guid.NewGuid();

            ApplyChange(new TicketCreatedEvent
            (
                title,
                description,
                priorityId,
                DefaultStatusId,
                files
            ));
        }

        // Need of a parameterless constructor to build Aggregate from events!
        private Ticket()
        {
            // Register EventHandlers
            Handles<TicketCreatedEvent>(Apply);
            Handles<TicketPriorityChangedEvent>(Apply);
            Handles<TicketStatusChangedEvent>(Apply);
        }

        #endregion

        #region Domain methods

        /// <summary>
        ///     These methods will apply events to our domain
        ///     This code should use a ubiquitous language.
        ///     This will make sure even non developers will be able to read the method signature easy!
        ///     They also handle domain validation
        ///     E.g. A ticket cannot be 'done' when it never was 'in progress'
        /// </summary>
        public void AddComment(string comment)
        {
            ApplyChange(new TicketCommentPlacedEvent(comment));
        }

        public void UpdatePriority(string newPriorityId)
        {
            if (newPriorityId == PriorityId)
                return;

            ApplyChange(new TicketPriorityChangedEvent
            (
                PriorityId,
                newPriorityId
            ));
        }

        public void UpdateStatus(string newStatusId)
        {
            if (newStatusId == StatusId)
                return;

            ApplyChange(new TicketStatusChangedEvent
            (
                StatusId,
                newStatusId
            ));
        }

        #endregion

        #region Events

        // Events are 'mutations' that happened in the past!
        // This blocks of code actually handle the business logic and will mutate the state of this domain class.
        // They are executed when a command arrives or when the aggregate is loaded from previous events!
        private void Apply(TicketCreatedEvent e)
        {
            Title = e.Title;
            Description = e.Description;
            PriorityId = e.PriorityId;
            StatusId = e.StatusId;
            Files = e.Files;
        }

        private void Apply(TicketPriorityChangedEvent e)
        {
            PriorityId = e.ToPriorityId;
        }

        private void Apply(TicketStatusChangedEvent e)
        {
            StatusId = e.ToStatusId;
        }

        private void Apply(TicketCommentPlacedEvent e)
        {
            Comments.Add(e.Comment);
        }

        #endregion
    }
}