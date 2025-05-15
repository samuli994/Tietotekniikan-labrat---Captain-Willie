using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Interactable : MonoBehaviour
{
    static public bool cleaning = false;
    public bool canInteract = false;
    void Awake()
    {
        gameObject.tag = "Interactable";
    }

    virtual public void Interact()
    {
        Debug.LogWarning("Did you forget to override Interact()?");
    }
}
