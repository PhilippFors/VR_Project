using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDestination : MonoBehaviour
{
    public bool active = false;
    public bool onDestination = false;

    public IInteractable pairObj;

    [SerializeField] Transform snapObj;
    public Vector3 snapPosition => snapObj.transform.position;
    public Quaternion snapRot => snapObj.transform.rotation;

    [SerializeField] float completionTime = 3f;
    [SerializeField] float floatSpeed = 9f;
    
    private void Start()
    {
        // pairObj.GetComponent<Draggable>().destination = this;
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
            other.gameObject.GetComponent<Draggable>().destination = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IInteractable>().draggable)
        {
            onDestination = false;
            other.gameObject.GetComponent<Draggable>().destination = null;
        }
    }
}
