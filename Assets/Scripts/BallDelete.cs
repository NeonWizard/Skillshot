using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDelete : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // prob shouldn't be using ienumerator but it works for now
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3);
        Object.Destroy(this.gameObject);
    }
}
