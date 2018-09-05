using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRWorldGrabbing : MonoBehaviour
{
    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;


    bool isLeftGripped = false;
    bool isRightGripped = false;

    Srt cameraRigSrt = new Srt();
    Srt controllerSrt = new Srt();
    Srt worldSrt = new Srt();
    Srt worldOffsetSrt = new Srt();

    // rotation variables
    public bool DollyMode = false;
    Vector3 InitialVector;

    // scale variables
    public bool AllowScale = true;
    float initialScalelength;
    float scaleNum = 1;

    private void Start()
    {
        leftController.Gripped += LeftGrip;
        leftController.Ungripped += LeftUngrip;
        rightController.Gripped += RightGrip;
        rightController.Ungripped += RightUngrip;
    }

    private void Update()
    {
        if (isLeftGripped || isRightGripped)
        {
            SetControllerSrt();
            Srt worldCameraFrame = controllerSrt * worldOffsetSrt;
            if (DollyMode)
            {
                Quaternion correction = Quaternion.FromToRotation(worldCameraFrame.localRotation * Vector3.up, Vector3.up);
                worldCameraFrame.localRotation = correction * worldCameraFrame.localRotation;
                worldCameraFrame.localPosition = correction * (worldCameraFrame.localPosition - controllerSrt.localPosition) + controllerSrt.localPosition;
            }
            worldSrt = worldCameraFrame.Inverse();

            transform.localPosition = worldSrt.localPosition;
            transform.localRotation = worldSrt.localRotation;
            transform.localScale = worldSrt.localScale;
        }

    }



    private void SetControllerSrt()
    {
        if (isLeftGripped && !isRightGripped)
        {
            controllerSrt.Set(leftController.transform.localPosition, leftController.transform.localRotation, leftController.transform.localScale);
        }
        else if (isRightGripped && !isLeftGripped)
        {
            controllerSrt.Set(rightController.transform.localPosition, rightController.transform.localRotation, rightController.transform.localScale);
        }
        // two controller
        else
        {

            // position
            Vector3 leftControllerPos = leftController.transform.localPosition;
            Vector3 rightControllerPos = rightController.transform.localPosition;
            Vector3 averagePos = (leftControllerPos + rightControllerPos) / 2;


            //rotation
            Quaternion currentRotation = Quaternion.FromToRotation(InitialVector, leftControllerPos - rightControllerPos);


            // scale
            Vector3 currentScaleVec = Vector3.one;
            if (AllowScale)
            {
                if (initialScalelength > 0.00001f)
                {
                    float currentScaleLength = (rightControllerPos - leftControllerPos).magnitude;
                    scaleNum = currentScaleLength / initialScalelength;
                    currentScaleVec = new Vector3(scaleNum, scaleNum, scaleNum);
                }
            }

            controllerSrt.Set(averagePos, currentRotation, currentScaleVec);
        }
    }

    private void SetWorldOffsetSrt()
    {
        cameraRigSrt.Set(transform.position, transform.rotation, transform.localScale);
        worldOffsetSrt = controllerSrt.Inverse() * cameraRigSrt.Inverse();
    }

    private void SetCameraRigSrt()
    {
        cameraRigSrt.Set(transform.localPosition, transform.localRotation, transform.localScale);

    }

    private void SetInitialScale()
    {
        Vector3 leftControllerPos = leftController.transform.localPosition;
        Vector3 rightControllerPos = rightController.transform.localPosition;
        initialScalelength = (leftControllerPos - rightControllerPos).magnitude;
    }

    private void SetInitialVector()
    {
        InitialVector = leftController.transform.localPosition - rightController.transform.localPosition;
    }

    private void LeftGrip(object sender, ClickedEventArgs e)
    {
        print("Left Grip");
        isLeftGripped = true;
        if (isRightGripped)
        {
            SetInitialScale();
            SetInitialVector();
        }
        SetControllerSrt();
        SetWorldOffsetSrt();
    }

    private void LeftUngrip(object sender, ClickedEventArgs e)
    {
        isLeftGripped = false;
        SetControllerSrt();
        SetWorldOffsetSrt();
    }

    private void RightGrip(object sender, ClickedEventArgs e)
    {
        print("Right Grip");
        isRightGripped = true;
        if (isLeftGripped)
        {
            SetInitialScale();
            SetInitialVector();
        }
        SetControllerSrt();
        SetWorldOffsetSrt();
    }

    private void RightUngrip(object sender, ClickedEventArgs e)
    {
        isRightGripped = false;
        SetControllerSrt();
        SetWorldOffsetSrt();
    }


}
