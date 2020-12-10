using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InteractUtilities : MonoBehaviour
{
    public static InteractUtilities instance;
    [SerializeField] BoxCollider boxArea;
    [SerializeField] GameObject area;
    public LayerMask mask;

    private void Awake()
    {
        instance = this;
    }

    public Coroutine StartSmoothPositionChange(IInteractable i, Vector3 newPos, Quaternion newRot, bool colliderOff = false, bool gravityOff = false, bool resetVel = false)
    {
        return StartCoroutine(SmoothPositionChange(i, newPos, newRot, colliderOff, gravityOff, resetVel));
    }

    IEnumerator SmoothPositionChange(IInteractable i, Vector3 newPos, Quaternion newRot, bool colliderOff, bool gravityOff, bool resetVel)
    {
        BoxCollider boxCol = null;
        MeshCollider meshCol = null;
        CapsuleCollider capCol = null;
        SphereCollider sphCol = null;

        Rigidbody rb = null;

        if (colliderOff)
        {
            if (FindCol<BoxCollider>(i, out boxCol))
                boxCol.enabled = false;

            else if (FindCol<CapsuleCollider>(i, out capCol))
                capCol.enabled = false;

            else if (FindCol<MeshCollider>(i, out meshCol))
                meshCol.enabled = false;

            else if (FindCol<SphereCollider>(i, out sphCol))
                sphCol.enabled = false;
        }

        if (gravityOff)
        {
            rb = i.GetComponent<Rigidbody>();
            if (rb != null)
                rb.useGravity = false;
        }

        if (Physics.CheckBox(newPos + new Vector3(0, 0.1f, 0), new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore))
        {
            while (Physics.CheckBox(newPos + new Vector3(0, 0.1f, 0), new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, mask, QueryTriggerInteraction.Ignore))
            {
                newPos = FindRandominArea();
            }
        }

        i.transform.DORotateQuaternion(newRot, GameSettings.instance.tweenSpeed);
        yield return i.transform.DOMove(newPos, GameSettings.instance.tweenSpeed);

        if (colliderOff)
        {
            if (boxCol != null)
                boxCol.enabled = true;

            else if (meshCol != null)
                meshCol.enabled = true;

            else if (capCol != null)
                capCol.enabled = true;

            else if (sphCol != null)
                sphCol.enabled = true;
        }

        if (gravityOff)
        {
            if (resetVel)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            if (rb != null)
                rb.useGravity = true;
        }
    }

    public Vector3 FindRandominArea()
    {
        return new Vector3(UnityEngine.Random.Range(area.transform.position.x - area.transform.localScale.x * boxArea.size.x * 0.5f,
                                                             area.transform.position.x + area.transform.localScale.x * boxArea.size.x * 0.5f),
                                    area.transform.position.y,
                                    UnityEngine.Random.Range(area.transform.position.z - area.transform.localScale.z * boxArea.size.z * 0.5f,
                                                             area.transform.position.z + area.transform.localScale.z * boxArea.size.z * 0.5f));
    }

    public bool FindCol<T>(IInteractable i, out T col)
    {
        col = i.GetComponent<T>();
        return col != null;
    }
}
