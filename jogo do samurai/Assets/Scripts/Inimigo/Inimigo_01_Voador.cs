using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo_01_Voador : MonoBehaviour
{
    [SerializeField] private int vida;
    [SerializeField] private float velocidade;
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private Animator anim;
    [SerializeField] private bool isRight;
    [SerializeField] private float distanciaParaParar;
    [SerializeField] private float dano;

    private float velocidadeInicial;
    private float playerPosX;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        velocidadeInicial = velocidade;
    }

    // Update is called once per frame
    void Update()
    {
        verDistanciaDoPlayer();
        mudarDirecao();
    }

    private void FixedUpdate()
    {

        move();
    }

    public void verDistanciaDoPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        playerPosX = transform.position.x - player.position.x;

        if(distance <= distanciaParaParar)
        {
            player.GetComponent<Player>().hitDano(dano);
            velocidade = 0f;
        }
        else
        {
            velocidade = velocidadeInicial;
        }
    }

    public void mudarDirecao()
    {
        if(playerPosX > 0)
        {
            isRight = false;
        }else if(playerPosX < 0)
        {
            isRight = true;
        }
    }

    public void move()
    {
        if (isRight)
        {
            rig.velocity = new Vector2(velocidade, rig.velocity.y);
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            rig.velocity = new Vector2(-velocidade, rig.velocity.y);
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void tomandoDano()
    {
        vida--;

        if (vida <= 0)
        {
            velocidade = 0;
            anim.SetTrigger("death");
            Destroy(gameObject, 0.333f);
        }
        else
        {
            anim.SetTrigger("hit");
            
        }
    }
}
