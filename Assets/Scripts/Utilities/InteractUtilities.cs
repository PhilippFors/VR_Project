using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUtilities : MonoBehaviour
{
    public static InteractUtilities instance;

    private void Awake()
    {
        instance = this;
    }
    public void StartSmoothPositionChange(IInteractable i, float floatSpeed, Vector3 pos = new Vector3(), Quaternion rotat = new Quaternion())
    {
        StartCoroutine(SmoothPositionChange(i, pos, rotat, floatSpeed));
    }

    bool ret = false;

    IEnumerator SmoothPositionChange(IInteractable i, Vector3 pos, Quaternion rotat, float floatSpeed)
    {
        i.GetComponent<Rigidbody>().useGravity = false;
        i.GetComponent<BoxCollider>().enabled = false;

        ret = false;
        while (!ret)
        {
            i.transform.position = Vector3.Lerp(i.transform.position, pos, Time.deltaTime * floatSpeed);
            i.transform.rotation = Quaternion.Lerp(i.transform.rotation, rotat, Time.deltaTime * floatSpeed);
            if (i.transform.position == pos)
                ret = true;
            yield return null;
        }

        i.GetComponent<BoxCollider>().enabled = true;
        i.GetComponent<Rigidbody>().useGravity = true;
        i.StopDragAction();
    }
}
