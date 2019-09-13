using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public GameObject area_Display;

    // Start is called before the first frame update
    void Start()
    {
        int seed = Random.Range(-2147483647, 2147483647);
        GameObject planet = PlanetFactory.GeneratePlanet(seed);
        planet.GetComponent<Planet>().s_Display = area_Display;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
