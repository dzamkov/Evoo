using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Evoo
{
    /// <summary>
    /// An area within the game that has its own physics and entities.
    /// </summary>
    public class Zone
    {
        public Zone()
        {
            this._VisualEntities = new List<VisualEntity>();
            this._QueuedMessages = new List<EntityMessage>();
        }

        /// <summary>
        /// Manually adds an entity to this zone. The entity should not be assigned
        /// to another zone. Special entities (such as timers) will become active when
        /// added.
        /// </summary>
        public void AddEntity(Entity Entity)
        {
            if (Entity._Zone == null)
            {
                Entity._Zone = this;

                VisualEntity ve = Entity as VisualEntity;
                if (ve != null)
                {
                    this._VisualEntities.Add(ve);
                }

            }
            else
            {
                throw new EntityAlreadyAssignedException() { Entity = Entity, TargetZone = this };
            }
        }

        /// <summary>
        /// Updates the state of the zone by the specified amount of time.
        /// </summary>
        public void Update(double Time)
        {

        }

        /// <summary>
        /// Renders the contents of the zone.
        /// </summary>
        public void Render()
        {
            foreach (VisualEntity ve in this._VisualEntities)
            {
                ve.Render();
            }
        }

        /// <summary>
        /// Sends all queued messages.
        /// </summary>
        public void FlushMessages()
        {
            while (this._QueuedMessages.Count > 0)
            {
                List<EntityMessage> nextmessages = new List<EntityMessage>();
                foreach (EntityMessage m in this._QueuedMessages)
                {
                    this._SendMessage(m);
                    foreach (EntityMessage nm in m.Recipient._MessageQueue)
                    {
                        nextmessages.Add(nm);
                    }
                    m.Recipient._MessageQueue.Clear();
                }
                this._QueuedMessages = nextmessages;
            }
        }

        /// <summary>
        /// Queues a message to be sent.
        /// </summary>
        public void QueueMessage(EntityMessage Message)
        {
            this._QueuedMessages.Add(Message);
        }

        /// <summary>
        /// Sends a message within the zone.
        /// </summary>
        private void _SendMessage(EntityMessage Message)
        {
            if (Message.Recipient._Zone == this)
            {
                Message.Message(Message.Recipient);
            }
            else
            {
                throw new IllegalMessageException()
                {
                    EntityMessage = Message,
                    MessageZone = this,
                    TargetZone = Message.Recipient._Zone
                };
            }
        }

        private List<VisualEntity> _VisualEntities;
        private List<EntityMessage> _QueuedMessages;
    }

    /// <summary>
    /// Exception thrown when an attempt is made to assign an entity to a zone when it is
    /// already assigned to another zone.
    /// </summary>
    public class EntityAlreadyAssignedException : Exception
    {
        public Entity Entity;
        public Zone TargetZone;
    }

    /// <summary>
    /// Exception thrown when a message's recipient is not in the zone where the message was sent.
    /// </summary>
    public class IllegalMessageException : Exception
    {
        public EntityMessage EntityMessage;
        public Zone MessageZone;
        public Zone TargetZone;
    }
}