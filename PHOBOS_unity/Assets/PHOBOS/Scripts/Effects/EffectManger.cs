using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManger : MonoBehaviour
{
    private ParticleSystem ps;
    // Start is called before the first frame update
    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    public void Update()
    {
        if (ps)
        {
            if (!ps.isPlaying)
            {
                Destroy(gameObject);
            }
        }

    }
}
