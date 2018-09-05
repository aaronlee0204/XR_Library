using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class EyeHandRaySelection : Selector
{

    LineRenderer lineRenderer;
    SteamVR_TrackedController controller;
    Vector3 grabPoint;
    Vodget grabbedVodget;
    bool isFocusGrabbed = false;
    float eyeHandDistance = 0;
    float eyeGrabptDistance = 0;
    RaycastHit hit;
    float distRatio = 0;
    public override void GrabFocus(bool val)
    {
        isFocusGrabbed = val;

        if (val)
        {
            // Save last hit point to local
            eyeHandDistance = (GetEyePosition() - transform.position).magnitude;
            eyeGrabptDistance = (grabPoint - GetEyePosition()).magnitude;
            distRatio = eyeGrabptDistance / eyeHandDistance;
            grabPoint = transform.InverseTransformPoint(hit.point);
        }
    }

    private void TriggerPressed(object sender, ClickedEventArgs e)
    {
        if (grabbedVodget)
        {
            grabbedVodget.Button(this, ButtonType.Trigger, true);
        }
    }

    private void TriggerReleased(object sender, ClickedEventArgs e)
    {
        if (grabbedVodget)
        {
            grabbedVodget.Button(this, ButtonType.Trigger, false);
            grabPoint = Vector3.zero;
            grabbedVodget = null;

        }
    }

    Vector3 GetEyePosition()
    {
        return transform.parent.TransformPoint(UnityEngine.XR.InputTracking.GetLocalPosition(XRNode.RightEye));
    }

    void DrawRay()
    {
        lineRenderer.SetPosition(0, GetEyePosition());
        lineRenderer.SetPosition(1, transform.position);
    }
    // Use this for initialization
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        //lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;


        controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += TriggerPressed;
        controller.TriggerUnclicked += TriggerReleased;
    }


    public void SetCursor()
    {
        if (isFocusGrabbed)
        {
            Vector3 handEyeVector = transform.position - GetEyePosition();
            float currentEyeGrabptDist = handEyeVector.magnitude * distRatio;

            cursor.localPosition = GetEyePosition() + handEyeVector.normalized * currentEyeGrabptDist;
            cursor.localRotation = transform.rotation;
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


    // Update is called once per frame
    void Update()
    {
        //DrawRay();
        if (!isFocusGrabbed)
        {
            if (Physics.Raycast(GetEyePosition(), transform.position - GetEyePosition(), out hit, int.MaxValue))
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


    /*
    if (isFocusGrabbed)
    {
        // Convert local grab pt to world
        Vector3 handEyeVector = transform.position - GetEyePosition();
        float currentEyeGrabptDist = handEyeVector.magnitude * distRatio;

        cursor.localPosition = GetEyePosition() + handEyeVector.normalized * currentEyeGrabptDist;
        cursor.localRotation = transform.rotation;

    }
    else
    {

        if (Physics.Raycast(GetEyePosition(), transform.position - GetEyePosition(), out hit, int.MaxValue))
        {
            Vodget hitVodget = hit.transform.gameObject.GetComponent<Vodget>();
            if (hitVodget)
            {
                cursor.Set(hit.point, transform.rotation, Vector3.one);

                // if there's no obj selected before
                if (grabbedVodget == null)
                {
                    grabPoint = hit.point;
                    grabbedVodget = hitVodget;
                    hitVodget.Focus(this, true);
                }

                // if the hit object is not the obj the one that selector previously focused
                else if (grabbedVodget != hitVodget)
                {
                    if (grabbedVodget)
                        grabbedVodget.Focus(this, false);

                    grabbedVodget = hitVodget;
                    hitVodget.Focus(this, true);
                    grabPoint = hit.point;
                }

                // if the hit object is the same as the obj
                else
                {
                    //hitVodget.Focus(this, true);
                }
            }

            // not grabbing any vodget
            else
            {
                if (grabbedVodget)
                {
                    cursor.Set(transform.position, transform.rotation, Vector3.one);
                    grabbedVodget.Focus(this, false);
                }
            }
        }
    }

    if (grabbedVodget)
    {
        //cursor.Set(transform.position, transform.rotation, Vector3.one);
        grabbedVodget.FocusUpdate(this);
    }

    DrawRay();
}
}

*/
}