using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUtilities : MonoBehaviour
{
    public static InteractUtilities instance;
    public BoxCollider boxCollider;
    public GameObject area;
    [SerializeField] float floatSpeed = 9f;
    public LayerMask mask;
    public DragController dragController;

    private void Awake()
    {
        instance = this;
    }
    public Coroutine StartSmoothPositionChange(IInteractable i, Vector3 newPos = new Vector3(), Quaternion newRot = new Quaternion())
    {
        return StartCoroutine(SmoothPositionChange(i, newPos, newRot));
    }

    IEnumerator SmoothPositionChange(IInteractable i, Vector3 newPos, Quaternion newRot)
    {
        Rigidbody rb = i.GetComponent<Rigidbody>();
        if (rb)
            rb.useGravity = false;
        // i.GetComponent<BoxCollider>().enabled = false;

        if (Physics.CheckBox(newPos + new Vector3(0, 0.1f, 0), new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore))
        {
            while (Physics.CheckBox(newPos + new Vector3(0, 0.1f, 0), new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, mask, QueryTriggerInteraction.Ignore))
            {
                newPos = FindRandominArea();
                yield return null;
            }
        }

        // rotco = StartCoroutine(SmoothRotation(i, newRot, floatSpeed));
        while (i.transform.position != newPos & i.transform.rotation != newRot)
        {
            i.transform.position = Vector3.Lerp(i.transform.position, newPos, Time.deltaTime * floatSpeed);
            i.transform.rotation = Quaternion.Lerp(i.transform.rotation, newRot, Time.deltaTime * (floatSpeed + 3f));
            yield return null;
        }

        // i.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(0.2f);

        i.StopDragAction();
        dragController.StopDrag(rb);
    }

    IEnumerator SmoothRotation(IInteractable i, Quaternion newRot, float floatSpeed)
    {
        while (i.transform.rotation != newRot)
        {

            yield return null;
        }
    }

    Vector3 FindRandominArea()
    {
        return new Vector3(UnityEngine.Random.Range(area.transform.position.x - area.transform.localScale.x * boxCollider.size.x * 0.5f,
                                                             area.transform.position.x + area.transform.localScale.x * boxCollider.size.x * 0.5f),
                                    area.transform.position.y,
                                    UnityEngine.Random.Range(area.transform.position.z - area.transform.localScale.z * boxCollider.size.z * 0.5f,
                                                             area.transform.position.z + area.transform.localScale.z * boxCollider.size.z * 0.5f));
    }


}
