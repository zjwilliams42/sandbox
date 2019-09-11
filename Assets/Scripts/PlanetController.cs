using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public GameObject area_Display;

    // Start is called before the first frame update
    void Start()
    {
        GameObject planet = PlanetFactory.GeneratePlanet(new Material(Shader.Find("Particles/Standard Surface")), 5, 0);
        planet.GetComponent<Planet>().s_Display = area_Display;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
