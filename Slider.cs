using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Slider : Vodget
{
    public GameObject text;
    public float min_y = 0.7379591f;
    public float max_y = 3.49f;
    Selector slider;
    Vector3 interactPoint = Vector3.zero;

    [System.Serializable]
    public class SliderEvent : UnityEvent<float> { }
    public SliderEvent value = new SliderEvent();


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
                isSelected = true;
                slider = cursor;
                cursor.GrabFocus(true);
            }
            else
            {
                isSelected = false;
                slider = null;
                cursor.GrabFocus(false);
            }
        }
    }
    public override void FocusUpdate(Selector cursor)
    {
        if (isSelected)
        {
            Vector3 currentPosition = transform.localPosition;
            float sliderValue = 0f;
            float currentPointYPos = (transform.InverseTransformPoint(slider.Cursor.localPosition)).y;
            currentPosition.y += currentPointYPos - interactPoint.y;
            currentPosition.y = Mathf.Clamp(currentPosition.y, min_y, max_y);
            transform.localPosition = currentPosition;
            sliderValue = (currentPosition.y - min_y) / (max_y - min_y);
            text.GetComponent<TextMesh>().text = sliderValue.ToString();

            value.Invoke(sliderValue);
        }
    }
}
