using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeInstatiator : MonoBehaviour {
    public GameObject coffeeParticle;

	// Use this for initialization
	void Start () {
		if (coffeeParticle != null)
        {
            StartCoroutine("InstantiateCoffee");
        }
	}
	
	IEnumerator InstantiateCoffee()
    {
        while (true)
        {
            yield return new WaitForSeconds (0.2f);
            Instantiate(coffeeParticle, transform);
        }
    }
}
