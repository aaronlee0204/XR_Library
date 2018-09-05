using UnityEngine;

//Extension methods must be defined in a static class
public static class TransformXtra
{

    static public void SetLocalSRT(this Transform A, Srt from)
    {
        A.localPosition = from.localPosition;
        A.localRotation = from.localRotation;
        A.localScale = from.localScale;
    }

    static public void SetLocalSRT(this Transform A, Vector3 p, Quaternion r, Vector3 s)
    {
        A.localPosition = p;
        A.localRotation = r;
        A.localScale = s;
    }

    static public Srt GetLocalSrt(this Transform A)
    {
        return new Srt(A);
    }

    static public Srt Inverse( this Transform A )
    {
        Srt retval = new Srt(A);
        return retval.Inverse();
    }

    // Rotate a transform about a sibling pivot point.
    static public void RotateAboutSiblingPoint(this Transform A, Vector3 pivot_pt, Quaternion dquat)
    {
        // If we created an Srt as a pivot frame converting frame to be a child would only change 
        // the frames position. 
        // Srt frame_pivot = ( pivot_pt, identity, 1f )
        // Srt frame_child = ( frame.localPosition - pivot_pt, frame.localRotation, frame.localScale )
        //
        // We then rotate frame_pivot by the users dquat.
        // frame_pivot = ( pivot_pt, dquat, 1f ) 
        // 
        // Converting frame_child back to world through frame_pivot yeilds.
        A.localPosition = (dquat * (A.localPosition - pivot_pt)) + pivot_pt;
        A.localRotation = dquat * A.localRotation;
    }

    // A point is transformed to its parent frame by first scaling, then rotating, then translating.
    public static Vector3 TransformPointLocal(this Transform A, Vector3 child_pt)
    {
        Vector3 position = Vector3.Scale(child_pt, A.localScale);
        position = A.localRotation * position;
        position += A.localPosition;
        return position;
    }

    // A point is transformed to a child frame by reversing the position, then reversing the rotation 
    // and then reversing scale.
    public static Vector3 InverseTransformPointLocal(this Transform A, Vector3 sibling_pt)
    {
        Vector3 position = sibling_pt - A.localPosition;
        position = Quaternion.Inverse(A.localRotation) * position;
        position = Vector3.Scale(position, Srt.ScaleInverse(A.localScale));
        return position;
    }

    // Transform a child rotation by a transform.
    public static Quaternion TransformRotationLocal(this Transform A, Quaternion child_rot)
    {
        return A.localRotation * child_rot;
    }
    // Transform a sibling rotation to be a child of a transform.
    public static Quaternion InverseTransformRotationLocal(this Transform A, Quaternion sibling_rot)
    {
        return Quaternion.Inverse(A.localRotation) * sibling_rot;
    }

    // Transform a child scale by a transform.
    public static Vector3 TransformScaleLocal(this Transform A, Vector3 child_scale)
    {
        return Vector3.Scale(A.localScale, child_scale);
    }
    // Transform a sibling scale to be a child of a transform.
    public static Vector3 InverseTransformScaleLocal(this Transform A, Vector3 sibling_scale)
    {
        return Vector3.Scale(Srt.ScaleInverse(A.localScale), sibling_scale);
    }

}
