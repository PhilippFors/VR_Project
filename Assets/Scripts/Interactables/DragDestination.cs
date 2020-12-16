using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDestination : MonoBehaviour
{
    public bool active = false;
    public bool onDestination = false;

    public IInteractable pairObj;

    [SerializeField] Transform snapObj;
    public Vector3 snapPosition;
    public Quaternion snapRot;

    [SerializeField] float completionTime = 3f;
    [SerializeField] float floatSpeed = 9f;

    private void Start()
    {
        if (snapObj == null)
        {
            snapPosition = gameObject.transform.position;
            snapRot = gameObject.transform.rotation;
        }
        else
        {
            snapPosition = snapObj.transform.position;
            snapRot = snapObj.transform.rotation;
        }
    }

    public void WaitForCompletionStart()
    {
        StartCoroutine(WaitForCompletion());
    }

    IEnumerator WaitForCompletion()
    {
        active = true;
        yield return new WaitForSeconds(completionTime);
        active = false;

        yield return InteractUtilities.instance.StartSmoothPositionChange(pairObj, pairObj.ogPos, pairObj.ogRot, true, true);

        pairObj.StopDragAction();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<IInteractable>().draggable)
        {
            onDestination = true;
            other.gameObject.GetComponent<IInteractable>().destination = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IInteractable>().draggable)
        {
            onDestination = false;
            other.gameObject.GetComponent<IInteractable>().destination = null;
        }
    }
}
