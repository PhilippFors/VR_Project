using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUI : MonoBehaviour
{
    [SerializeField] Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        StartCoroutine(lookatCam());
    }

    IEnumerator lookatCam()
    {
        while (true)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(-(mainCam.transform.position - gameObject.transform.position));
            yield return null;
        }
    }
}
