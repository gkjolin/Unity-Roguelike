using System;
using UnityEngine;

namespace AKSaigyouji
{
    /// <summary>
    /// Makes the variable decorated with this attribute read-only. Will appear as greyed out and will not
    /// accept any changes. Note that this is not compatible with custom property drawers: if used on a variable
    /// whose type has a custom property drawer, the property drawer will be ignored and the variable will be drawn
    /// using the default.
    /// </summary>
    public sealed class ReadOnlyAttribute : PropertyAttribute
    {
        // The code responsible for making the field readonly is in ReadOnlyDrawer, an editor script. The attribute
        // itself has no data or logic.
    } 
}