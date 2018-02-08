using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    private GameObject pickup;
    
    public SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObject.index); }
    }
    void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }
    private void Update()
    {
        if (Controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && pickup != null)
        {
            pickup.transform.parent = this.transform;
            pickup.GetComponent<Rigidbody>().useGravity = false;
        }
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip) && pickup != null)
        {
            pickup.transform.parent = null;
            pickup.GetComponent<Rigidbody>().useGravity = true;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        pickup = collider.gameObject;
    }

    // Remove all items no longer colliding with to avoid further processing
    private void OnTriggerExit(Collider collider)
    {
        pickup = null;
    }
}
