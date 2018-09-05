using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectSelection : Selector
{

    LineRenderer lineRenderer;
    SteamVR_TrackedController controller;
    Vector3 grabPoint;
    Vodget grabbedObject;
    bool isFocusGrabbed = false;
    public override void GrabFocus(bool val)
    {
        isFocusGrabbed = val;

        if (true)
        {
            // Save last hit point to local
            grabPoint = controller.transform.InverseTransformPoint(grabPoint);
        }
    }

    private void TriggerPressed(object sender, ClickedEventArgs e)
    {
        if (grabbedObject)
        {
            SetCursor();
            grabbedObject.Button(this, ButtonType.Trigger, true);
        }
    }

    private void TriggerReleased(object sender, ClickedEventArgs e)
    {
        if (grabbedObject)
        {
            grabbedObject.Button(this, ButtonType.Trigger, false);
            grabPoint = Vector3.zero;
            SetCursor();
        }
    }


    void SetCursor()
    {
        cursor.Set(transform.position, transform.rotation, Vector3.one);
    }

    // Use this for initialization
    void Start()
    {

        controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += TriggerPressed;
        controller.TriggerUnclicked += TriggerReleased;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isFocusGrabbed)
        {
            return;
        }
        if (other.transform.gameObject.GetComponent<Vodget>())
        {
            Vodget hitVodget = other.transform.gameObject.GetComponent<Vodget>();
            SetCursor();

            // if there's no obj selected before
            if (grabbedObject == null)
            {
                grabPoint = transform.position;
                grabbedObject = hitVodget;
                hitVodget.Focus(this, true);
            }

            // if the hit object is not the obj the one that selector previously focused
            else if (grabbedObject != other.transform.gameObject)
            {
                if (grabbedObject)
                    grabbedObject.Focus(this, false);

                grabbedObject = hitVodget;
                hitVodget.Focus(this, true);
                grabPoint = transform.position;
            }

            // if the hit object is the same as the obj
            else
            {
                //hitVodget.Focus(this, true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (grabbedObject)
        {
            SetCursor();
            grabbedObject.Focus(this, false);
            grabbedObject = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedObject)
        {
            cursor.Set(transform.position, transform.rotation, Vector3.one);
            grabbedObject.FocusUpdate(this);
        }
    }
}
