using UnityEngine;
using System.Collections;
public class OutOfBoundsTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        IInteractable i = other.GetComponent<IInteractable>();
        if (i != null)
            if (!i.interactable && i.throwable)
            {
                i.ThrowAction();
                StartCoroutine(WaitForReset(i));
            }
            else
            {
                i.ResetOgPosition();
            }
    }

    IEnumerator WaitForReset(IInteractable i)
    {
        Vector3 newPos = i.ogPos;
        yield return new WaitForSeconds(i.waitPosReset);
        if (Physics.CheckBox(newPos + new Vector3(0, 0.1f, 0), new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore))
        {
            while (Physics.CheckBox(newPos + new Vector3(0, 0.1f, 0), new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, InteractUtilities.instance.mask, QueryTriggerInteraction.Ignore))
            {
                newPos = InteractUtilities.instance.FindRandominArea();
            }
        }
        i.ogPos = newPos;
        i.interactable = true;
        i.ResetOgPosition();
    }
}
