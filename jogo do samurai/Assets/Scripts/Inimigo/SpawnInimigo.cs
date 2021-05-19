using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnInimigo : MonoBehaviour
{

    [SerializeField] private float tempoMin;
    [SerializeField] private float tempoMax;
    [SerializeField] private GameObject inimigoPrefab;
    

    private float count;
    private float tempoSpawn;

    // Start is called before the first frame update
    void Start()
    {
        tempoSpawn = Random.Range(tempoMin, tempoMax);
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;

        if(count > tempoSpawn)
        {
            Vector3 inimigo = new Vector3(transform.position.x, transform.position.y, 0f);
            Instantiate(inimigoPrefab, inimigo, transform.rotation);
            tempoSpawn = Random.Range(tempoMin, tempoMax);
            count = 0f;
        }
    }
}
