using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("Atributos")]
    [SerializeField] private float velocidade;
    [SerializeField] private float forcaPulo;
    [SerializeField] private int vida;
    [SerializeField] private float tempoDeRecuperacao;

    [Header("Componentes Unity")]
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform poonto_ataque;
    [SerializeField] private float radiusAttack;
    [SerializeField] private LayerMask inimigoLayer;
    [SerializeField] private Image ImageVida;
    [SerializeField] private GameController gameController;


    // variáveis de auxilio;
    private bool isJump; 
    private bool isAttack;
    private bool isDead;
    private float contadorDeRecuperacao;
    private float vidaInicial;

    [Header("Audios")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClipAtk;

    // Start is called before the first frame update
    void Start()
    {
        vidaInicial = vida;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            jump();
            attack();
        }
        
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            move();
        }
    }

    public void move()
    {
        float movemento = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movemento * velocidade, rig.velocity.y);

        if(movemento > 0 && !isAttack)
        {
            transform.eulerAngles = new Vector2(0, 0);

            if (!isJump)
                anim.SetInteger("transition", 1);
        }
        else if(movemento < 0 && !isAttack)
        {
            transform.eulerAngles = new Vector2(0, 180);
            
            if(!isJump)
                anim.SetInteger("transition", 1);
        }
        else if(movemento == 0 && !isJump && !isAttack)
        {
            anim.SetInteger("transition", 0);
        }
    }

    public void jump()
    {
        if (Input.GetButton("Jump") && !isJump)
        {
            anim.SetInteger("transition", 2);
            rig.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
            isJump = true;
        }
    }

    public void attack()
    {
        if (Input.GetButtonDown("Fire1") && !isAttack)
        {
            audioSource.PlayOneShot(audioClipAtk);
            anim.SetInteger("transition", 3);
            isAttack = true;
            StartCoroutine(esperarFimAtaque());
        }
    }

    // método chamado no animation do Player no Attack, frame 5
    public void darDanoNoInimigo()
    {
        Collider2D collider2D = Physics2D.OverlapCircle(poonto_ataque.position, radiusAttack, inimigoLayer);

        if (collider2D != null)
        {
            collider2D.GetComponent<Inimigo_01_Voador>().tomandoDano();
           
        }
    }


    private IEnumerator esperarFimAtaque()
    {
        yield return new WaitForSeconds(0.500f);
        isAttack = false;
    }

    public void hitDano(float dano)
    {
        contadorDeRecuperacao += Time.deltaTime;

        if(contadorDeRecuperacao >= tempoDeRecuperacao && !isDead)
        {
            
            anim.SetTrigger("hit");
            vida--;
            ImageVida.fillAmount -= ((float)dano / (float)vidaInicial);
            gameOver();
            contadorDeRecuperacao = 0f;
            
        }
    }

    void gameOver()
    {
        if (vida <= 0 && !isDead)
        {
            anim.SetTrigger("morte");
            
            gameController.fimJogo();
            isDead = true;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            isJump = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(poonto_ataque.position, radiusAttack);
    }
}
