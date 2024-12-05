﻿using System;

namespace MTC_Game.Exceptions
{
    /// <summary>This class represents a user-specific exception.</summary>
    public class UserException : Exception
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Creates a new instance of this class.</summary>
        public UserException() : base()
        { }


        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="message">Message.</param>
        public UserException(string message) : base(message)
        { }
    }
}
