using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{
    [HideInInspector]
    public GameObject otherObject;
    [HideInInspector]
    public GameObject objectInHand;
    private SteamVR_TrackedObject trackedObject;

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
        if (Controller.GetHairTriggerDown())
            if (otherObject)
                Grab();
        if (Controller.GetHairTriggerUp())
            if (objectInHand)
                Drop();
    }
    public void Grab()
    {
        //Move Object to hand
        objectInHand = otherObject;
        otherObject = null;
        //Adds new joint that connects object to hand
        var joint = AddFixedJoint();
        //Add to controller
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        //Create new joint and makes it so it does not break easy
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    public void Drop()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        objectInHand = null;
    }

    private void SetCollidingObject(Collider col)
    {
        if (otherObject || !col.GetComponent<Rigidbody>())
            return;
        otherObject = col.gameObject;
    }

    public void OnTriggerExit(Collider other)
    {
        if (!otherObject)
            return;
        otherObject = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
}
