using System;
using System.Collections.Generic;
using OpenTK;

namespace Evoo
{
    /// <summary>
    /// An entity that marks the location and orientation of a player camera.
    /// </summary>
    public class CameraEntity : Entity
    {
        /// <summary>
        /// Gets the camera orientation information.
        /// </summary>
        public CameraInfo Info
        {
            get
            {
                return this._Info;
            }
        }

        /// <summary>
        /// Creates a message template to reorient the camera.
        /// </summary>
        public ReorientCameraMessageTemplate Reorient
        {
            get
            {
                return delegate(CameraInfo CameraInfo)
                {
                    return new EntityMessage()
                    {
                        Recipient = this,
                        Message = delegate
                        {
                            this._Info = CameraInfo;
                        }
                    };
                };
            }
        }

        private CameraInfo _Info;
    }

    /// <summary>
    /// A message template that creates a reorientation message for a camera
    /// </summary>
    public delegate EntityMessage ReorientCameraMessageTemplate(CameraInfo CameraInfo);
}