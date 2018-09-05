using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRaySelection : Selector
{

    LineRenderer lineRenderer;
    SteamVR_TrackedController controller;
    Vector3 grabPoint;
    Vodget grabbedVodget;
    bool isFocusGrabbed = false;
    RaycastHit hit;

    public override void GrabFocus(bool val)
    {
        isFocusGrabbed = val;

        if (val)
        {
            // Save last hit point to local
            grabPoint = transform.InverseTransformPoint(hit.point);
        }
    }

    private void TriggerPressed(object sender, ClickedEventArgs e)
    {
        if (grabbedVodget)
        {
            if (!grabbedVodget.isSelected)
            {
                SetCursor();
                grabbedVodget.Button(this, ButtonType.Trigger, true);
            }
        }
    }

    private void TriggerReleased(object sender, ClickedEventArgs e)
    {
        if (grabbedVodget)
        {
            SetCursor();
            grabbedVodget.Button(this, ButtonType.Trigger, false);
            //grabPoint = Vector3.zero;
            grabbedVodget = null;
        }
    }

    public void SetCursor()
    {
        if (isFocusGrabbed)
        {
            cursor.Set(transform.TransformPoint(grabPoint), transform.rotation, Vector3.one);
        }
        else
        {
            if (grabbedVodget == null)
            {
                cursor.Set(transform.position, transform.rotation, Vector3.one);
            }
            else
            {
                cursor.Set(hit.point, transform.rotation, Vector3.one);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, new Vector3(0f, 0f, 100f));
        controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += TriggerPressed;
        controller.TriggerUnclicked += TriggerReleased;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFocusGrabbed)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, int.MaxValue))
            {
                Vodget hitVodget = hit.transform.gameObject.GetComponent<Vodget>();
                if (hitVodget)
                {

                    // if there's no obj selected before
                    if (!hitVodget.isSelected)
                    {
                        if (grabbedVodget == null)
                        {
                            grabPoint = hit.point;
                            grabbedVodget = hitVodget;
                            SetCursor();
                            hitVodget.Focus(this, true);
                        }

                        // if the hit object is not the obj the one that selector previously focused
                        else if (grabbedVodget != hitVodget)
                        {
                            if (grabbedVodget)
                            {
                                grabbedVodget.Focus(this, false);
                            }

                            grabbedVodget = hitVodget;
                            SetCursor();
                            hitVodget.Focus(this, true);
                            grabPoint = hit.point;
                        }
                    }

                }

                // ray hit, but not hitting at a vodget
                else
                {
                    if (grabbedVodget != null)
                    {
                        grabbedVodget.Focus(this, false);
                        grabbedVodget = null;
                    }
                }

            }

            //ray hit nothing
            else
            {
                //not shooting at any vodget
                if (grabbedVodget != null)
                {
                    SetCursor();
                    grabbedVodget.Focus(this, false);
                    grabbedVodget = null;
                }
            }
        }

        if (grabbedVodget)
        {
            SetCursor();
            grabbedVodget.FocusUpdate(this);
        }
    }
}
