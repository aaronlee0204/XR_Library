using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : Vodget
{

    //public bool isSelected = false;
    Srt offsetSrt = new Srt();

    public override void Focus(Selector cursor, bool state)
    {
        // change the object to red if it's on focus, otherwise it's white
        if (state)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;

        }
    }


    public override void Button(Selector cursor, Selector.ButtonType button, bool state)
    {
        if (button == Selector.ButtonType.Trigger)
        {
            if (state)
            {

                Srt objectSrt = new Srt(transform.position, transform.rotation, transform.lossyScale);
                offsetSrt = cursor.Cursor.Inverse() * objectSrt;
                isSelected = true;

            }
            else
            {
                isSelected = false;
            }
            cursor.GrabFocus(state);
        }


    }


    public override void FocusUpdate(Selector cursor)
    {
        if (isSelected)
        {
            Srt objectSrt = cursor.Cursor * offsetSrt;
            transform.position = objectSrt.localPosition;
            transform.rotation = objectSrt.localRotation;
        }
    }


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
