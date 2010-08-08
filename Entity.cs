using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Evoo
{
    /// <summary>
    /// An object within a zone that can communicate with other entities in its zone. Note that entities
    /// should not directly call methods on other entities, and should instead use messages.
    /// </summary>
    public class Entity
    {
        public Entity()
        {
            this._MessageQueue = new List<EntityMessage>();
        }

        /// <summary>
        /// Gets the zone this entity is in, or null if the entity is yet to be assigned
        /// a zone. Entites may communicate only if they are in the same zone.
        /// </summary>
        public Zone Zone 
        {
            get
            {
                return this._Zone;
            }
        }

        /// <summary>
        /// Causes the entity to send a message to another entity.
        /// </summary>
        protected void Send(EntityMessage Message)
        {
            this._MessageQueue.Add(Message);
        }

        internal List<EntityMessage> _MessageQueue;
        internal Zone _Zone;
    }

    /// <summary>
    /// A message that can be sent by an entity to another entity.
    /// </summary>
    public struct EntityMessage
    {
        /// <summary>
        /// The receiving entity of this message.
        /// </summary>
        public Entity Recipient;

        /// <summary>
        /// The action to be performed to send this message.
        /// </summary>
        public Action<Entity> Message;
    }

    /// <summary>
    /// A template for a message that takes no arguments.
    /// </summary>
    public delegate EntityMessage EventMessageTemplate();

    /// <summary>
    /// An entity that can be drawn.
    /// </summary>
    public abstract class VisualEntity : Entity
    {
        /// <summary>
        /// Called when the visual entity needs to be rendered. It should be rendered using zone coordinates.
        /// </summary>
        public abstract void Render();
    }

    /// <summary>
    /// Information about a camera's location.
    /// </summary>
    public struct CameraInfo
    {
        public Vector Pos;
        public Vector Dir;
        public Vector Up;
    }
}