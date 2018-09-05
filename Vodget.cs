using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Vodget : MonoBehaviour
{
    public bool isSelected = false;
    // Focus is called by selectors at the moment a vodget is potentially selected and deselected.
    public abstract void Focus(Selector cursor, bool state);

    // Button is called by selectors on vodgets that have focus. 
    // Note: It is common for vodgets to call the selectors GrabFocus(true) 
    public abstract void Button(Selector cursor, Selector.ButtonType button, bool state);
    public abstract void FocusUpdate(Selector cursor);
}
