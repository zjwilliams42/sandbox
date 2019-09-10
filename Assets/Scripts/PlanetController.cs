using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Planet planet = PlanetFactory.GeneratePlanet(new Material(Shader.Find("Specular")), 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
