using System;
using Erazer.Domain.Constants;
using Erazer.Domain.Events;
using Erazer.Framework.Domain;

namespace Erazer.Domain
{
    public class Ticket : AggregateRoot
    {
        // Only 'keep' properties that will be needed for business logic
        // E.g. The name of a Ticket is never used in my logic.
        // If one of the business rules would be "if name of ticket contains 'Erazer' it's not possible to 'Close' the ticket" => Add name as property to aggregate!

        private string _priorityId;
        private int _statusId;

        private const string DefaultPriorityId = PriorityConstants.Medium;
        private const int DefaultStatusId = StatusConstants.Backlog;

        #region Constructors
        // Need of a parameterless constructor to build Aggregate from events!
        public Ticket()
        {
            // TODO Find a way to make this ctor private (Marten requires it to be public)
        }

        // Constructor used for creating a new ticket!
        public Ticket(Guid id, string title, string description) : this(id, title, description, DefaultPriorityId, DefaultStatusId)
        {

        }

        private Ticket(Guid id, string title, string description, string priorityId, int statusId) 
        {
            Id = id;
            _priorityId = priorityId;
            _statusId = statusId;

            // TODO Write code to save 'new ticket'
        }
        #endregion

        #region Events
        // This block of code is used to generate an aggragete from loading the events from the event store!

        // TODO Find a way to make this private (Marten requires it to be public)
        public void Apply(TicketPriorityEvent e)
        {
            _priorityId = e.ToPriorityId;
        }

        #endregion

        #region Domain methods
        /// <summary>
        /// These methods define what our aggragate does. This code should use a ubiquitous language.
        /// This will make sure even non developers will be able to read the method signature easy!
        /// 
        /// Not every of this methods contain business logic.
        /// For example AddComment will just fire a new event.
        /// 
        /// TODO
        /// While changing criticality will check if current status allows to change it! 
        /// </summary>

        public void AddComment(string comment, Guid commenterId)
        {
            ApplyChange(new TicketCommentEvent
            {
                Comment = comment,
                UserId = commenterId
            });
        }

        public void UpdatePriority(string priorityId, Guid userId)
        {
            var currentPriority = _priorityId;
            _priorityId = priorityId;

            ApplyChange(new TicketPriorityEvent
            {
                FromPriorityId = currentPriority,
                ToPriorityId = priorityId,
                UserId = userId
            });
        }
        #endregion
    }
}
