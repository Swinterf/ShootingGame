using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBackgroundOffset : MonoBehaviour
{
    [SerializeField] Vector2 scrollVelocity;

    Material material;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    }
}
