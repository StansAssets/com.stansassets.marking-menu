﻿using System;

namespace StansAssets.MarkingMenu
{
    public class CustomActionRegistrationAttribute : Attribute
    {
        public bool Enabled { get; }

        public CustomActionRegistrationAttribute(bool enabled)
        {
            Enabled = enabled;
        }
    }
}