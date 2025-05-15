using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoSackManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> potatoSacks;

    public void HidePotatoSacks() {
        foreach (var item in potatoSacks)
        {
            item.SetActive(false);
        }
    }
}
