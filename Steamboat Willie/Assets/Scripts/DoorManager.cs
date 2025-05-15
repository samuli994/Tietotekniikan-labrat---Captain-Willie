using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> doors;
    public void CloseDoors()
    {
        foreach (var door in doors)
        {
            door.GetComponent<DoorInteract>().doorOpen = false;
            door.GetComponent<Animator>().SetBool("DoorOpen", false);
        }
    }

}
