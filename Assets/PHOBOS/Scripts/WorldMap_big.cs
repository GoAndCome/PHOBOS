using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap_big : MonoBehaviour
{
    public GameObject Readying;

    public void OnClickOptionButton()
    {
        Readying.SetActive(false);
        Readying.SetActive(true);
    }

}
