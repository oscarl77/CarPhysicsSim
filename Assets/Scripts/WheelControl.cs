using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public Transform wheelModel;    

    [HideInInspector] public WheelCollider WheelCollider;

    public bool steerable;
    public bool motorized;
    
    private Vector3 position;
    private Quaternion rotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        WheelCollider.GetWorldPose(out position, out rotation);
        wheelModel.transform.position = position;
        wheelModel.transform.rotation = rotation;
    }
}
