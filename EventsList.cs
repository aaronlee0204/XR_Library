using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsList : MonoBehaviour {

    public GameObject whiteball;
    public Light roomLight;
    Color roomLightColor;
    public void ChangeColor(float value)
    {
        var currColor = whiteball.GetComponent<Renderer>().material.color;
        Color newColor = new Color(currColor.r, currColor.g, 1- value, 1);
        print(value);
        whiteball.GetComponent<Renderer>().material.color = newColor;

    }


    public void ChangeLight(float value)
    {
        value = value / 360 * 1.5f;
        print(value);
        roomLight.intensity = value;
    }

    public void ChangeRoomLightColor(float value)
    {
        float rValue = roomLightColor.r + value;
        rValue = Mathf.Clamp(rValue, 0, 1);
        roomLight.color = new Color(rValue, roomLightColor.g, roomLightColor.b, roomLightColor.a);

    }

    private void Start()
    {
        roomLightColor = roomLight.color;
    }
}
