using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDestination : MonoBehaviour
{
    public bool active;
    public IInteractable pairObj;
    [SerializeField] Transform snapObj;
    public Vector3 snapPosition => snapObj.transform.position;
    public Quaternion snapRot => snapObj.transform.rotation;
    [SerializeField] float completionTime = 3f;
    [SerializeField] float floatSpeed = 9f;

    private void Start()
    {
        pairObj.GetComponent<Draggable>().destination = this;
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
        StartCoroutine(SmoothPositionChange());
    }

    IEnumerator SmoothPositionChange()
    {

        pairObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
        pairObj.gameObject.GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(SmoothRotation());
        
        while (pairObj.gameObject.transform.position != pairObj.ogPos)
        {
            pairObj.gameObject.transform.position = Vector3.Lerp(pairObj.gameObject.transform.position, pairObj.ogPos, Time.deltaTime * floatSpeed);
            yield return null;
        }

        pairObj.gameObject.GetComponent<BoxCollider>().enabled = true;
        pairObj.gameObject.GetComponent<Rigidbody>().useGravity = true;
        pairObj.StopDragAction();
    }

    IEnumerator SmoothRotation()
    {

        while (pairObj.gameObject.transform.rotation != pairObj.ogRot)
        {
            pairObj.gameObject.transform.rotation = Quaternion.Lerp(pairObj.gameObject.transform.rotation, pairObj.ogRot, Time.deltaTime * floatSpeed);
            yield return null;
        }
    }
}
