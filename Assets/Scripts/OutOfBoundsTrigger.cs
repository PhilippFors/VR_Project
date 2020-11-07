﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Interactable>())
            other.GetComponent<Interactable>().ResetPosition();
    }
}
