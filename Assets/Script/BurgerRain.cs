using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerRain : MonoBehaviour
{
    [SerializeField] public List<GameObject> burgersIngredients;

    public float spawnwidth = 9f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(IngredientSpawn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator IngredientSpawn()
    {
        while (true)
        {
            SpawnBurger();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SpawnBurger()
    {
        int randomIndex = Random.Range(0, burgersIngredients.Count);
        GameObject randomBurger = burgersIngredients[randomIndex];
        float randpos = Random.Range(-spawnwidth, spawnwidth);
        Vector3 spawnPosition = new Vector3(randpos, 6, transform.position.z);
        GameObject newBurger = Instantiate(randomBurger, spawnPosition, Quaternion.identity);

    }
}
