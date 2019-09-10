using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject planet = PlanetFactory.GeneratePlanet(new Material(Shader.Find("Particles/Standard Surface")), 5);
        planet.AddComponent<Planet> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
