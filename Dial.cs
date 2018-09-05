using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dial : Vodget
{

    [System.Serializable]
    public class DialEvent : UnityEvent<float> { }

    public DialEvent degrees = new DialEvent();


    Vector3 interactPoint = Vector3.zero;
    Selector notch;

    public override void Focus(Selector cursor, bool state)
    {
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
                interactPoint = transform.InverseTransformPoint(cursor.Cursor.localPosition);
                interactPoint.z = 0;
                isSelected = true;
                notch = cursor;
                cursor.GrabFocus(true);
            }
            else
            {
                isSelected = false;
                notch = null;
                cursor.GrabFocus(false);
            }
        }
    }


    public override void FocusUpdate(Selector cursor)
    {
        if (isSelected)
        {
            Quaternion currentRotation = transform.localRotation;
            float dialValue = 0;

            Vector3 currentPoint = transform.InverseTransformPoint(cursor.Cursor.localPosition);
            currentPoint.z = 0;

            Quaternion deltaRotation = Quaternion.FromToRotation(interactPoint, currentPoint);

            currentRotation *= deltaRotation;
            transform.localRotation = currentRotation;


            // get dialValue
            Vector3 controlNorm = transform.localRotation * Vector3.up;
            controlNorm.Normalize();
            float angle = Vector3.Dot(controlNorm, Vector3.up);
            angle = Mathf.Acos(angle);
            angle *= Mathf.Rad2Deg;

            if (Vector3.Dot(controlNorm, Vector3.right) > 0f)
            {
                angle = 360f - angle;
            }

            dialValue = angle;
            print(angle);
            degrees.Invoke(dialValue);
        }
    }

}
