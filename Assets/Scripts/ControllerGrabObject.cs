using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    private GameObject collidingObject;
    private GameObject objectInHand;

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

        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
                GrabObject();
        }
        if (Controller.GetHairTriggerUp())
        {
            if (objectInHand)
                ReleaseObject();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        SetCollidingObject(other.collider);
    }
    public void OnCollisionStay(Collision other)
    {
        SetCollidingObject(other.collider);
    }
    private void OnCollisionExit(Collision collider)
    {
        if (!collidingObject)
            return;
        collidingObject = null;
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }
    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }
    private void ReleaseObject()
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
        if (collidingObject || !col.GetComponent<Rigidbody>())
            return;
        collidingObject = col.gameObject;
    }
}
