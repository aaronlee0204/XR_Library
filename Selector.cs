using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Selector : MonoBehaviour
{
    public enum ButtonType { Trigger, Bumper }
    public abstract void GrabFocus(bool val);

    // Inheriting selectors must all maintain a model of a 3D cursor. 
    protected Srt cursor = new Srt();
    public Srt Cursor
    {
        get { return cursor; }
    }
}
