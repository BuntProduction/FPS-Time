using UnityEngine;

public class PointInTime : MonoBehaviour
{
    public Vector3 position;
    public Quaternion rotation;

    public PointInTime (Vector3 _position, Quaternion _rotation)
    {
    	rotation = _rotation;
    	position = _position;
    }
}
