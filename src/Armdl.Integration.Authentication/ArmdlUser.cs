using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration
{
    /// <summary>
    /// Defines the user info.
    /// </summary>
    public class ArmdlUser
    {
        /// <summary>
        /// Gets the user ID.
        /// </summary>
        public long Id { get; internal set; }

        /// <summary>
        /// Gets the name of user.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        public string Email { get; internal set; }

        /// <summary>
        /// Gets the email verification date.
        /// </summary>
        public DateTime? EmailVerifiedAtUTC { get; internal set; }

        /// <summary>
        /// Gets the user created date.
        /// </summary>
        public DateTime CreatedAtUTC { get; internal set; }

        /// <summary>
        /// Gets the user user info updated date.
        /// </summary>
        public DateTime UpdatedAtUTC { get; internal set; }

        /// <summary>
        /// Gets the personal balance of user.
        /// </summary>
        public decimal Balance { get; internal set; }

        /// <summary>
        /// Gets the API token of user.
        /// </summary>
        public string ApiToken { get; internal set; }

        /// <summary>
        /// Gets the type of user.
        /// </summary>
        public int Type { get; internal set; }

        /// <summary>
        /// Gets the organisation name.
        /// </summary>
        public string OrganizationName { get; internal set; }

        /// <summary>
        /// Gets the organisation INN.
        /// </summary>
        public string OrganizationInn { get; internal set; }

        /// <summary>
        /// Gets the organisation address.
        /// </summary>
        public string OrganizationAddress { get; internal set; }

        /// <summary>
        /// Gets the license info for current module by which authorization doing.
        /// </summary>
        public ArmdlLicense License { get; internal set; }
    }
}
