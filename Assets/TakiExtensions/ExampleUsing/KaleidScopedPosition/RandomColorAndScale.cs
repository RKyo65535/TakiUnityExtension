using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomColorAndScale : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value,1);
        transform.localScale = Vector3.one * (Random.value + 1);
    }

}
