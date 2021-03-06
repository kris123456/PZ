﻿using System;

namespace SkyCrab.Common_classes
{
    public class SkyCrabException : Exception
    {

        protected SkyCrabException() :
            base()
        {
        }
        
        public SkyCrabException(String message) :
            base(message)
        {
        }
        
        public SkyCrabException(String message, Exception innerException) :
            base(message, innerException)
        {
        }

    }
}
