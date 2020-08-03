using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration
{
    /// <summary>
    /// Defines license details info.
    /// </summary>
    public class ArmdlLicense
    {
        /// <summary>
        /// Gets the license ID.
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether is valid license for current module.
        /// </summary>
        public bool IsValid { get; internal set; }

        /// <summary>
        /// Gets the license status.
        /// </summary>
        public string Status { get; internal set; }

        /// <summary>
        /// Gets the related user ID.
        /// </summary>
        public long UserId { get; internal set; }

        /// <summary>
        /// Gets the module ID (as usual is it same as client ID).
        /// </summary>
        public long ModuleId { get; internal set; }

        /// <summary>
        /// Gets the license expiration date.
        /// </summary>
        public DateTime EndedAtUTC { get; internal set; }

        /// <summary>
        /// Gets the license creation date.
        /// </summary>
        public DateTime? CreatedAtUTC { get; internal set; }

        /// <summary>
        /// Gets the last updating date of license.
        /// </summary>
        public DateTime? UpdatedAtUTC { get; internal set; }

        /// <summary>
        /// Gets the license beginning date.
        /// </summary>
        public DateTime StartedAtUTC { get; internal set; }

        /// <summary>
        /// Gets the related token value.
        /// </summary>
        public string Token { get; internal set; }
    }
}
