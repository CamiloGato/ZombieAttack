using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject attackArea;
    public SpawnManager spawnManager; 
    
    private Rigidbody2D _rb;
    private Animator _anim;
    
    private int lastHorizontal, lastVertical;
    private float timerAttack;
    private bool isWalking;
    
    private void Awake()
    {
        attackArea.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        timerAttack += Time.deltaTime;
        
        // Input Jugador
        int horizontal = (int) Input.GetAxisRaw("Horizontal");
        int vertical = (int) Input.GetAxisRaw("Vertical");
        
        UpdateMovement(horizontal, vertical);
        UpdateAttackArea(horizontal, vertical);
        CheckAttack();
        UpdateAnimator();
        CheckGameOver();

    }

    private void CheckGameOver()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawnManager.gameOver = true;
        }
    }
    
    /// <summary>
    /// Actualiza la posición del jugador
    /// </summary>
    /// <param name="horizontal"> Dirección eje X </param>
    /// <param name="vertical"> Direccion eje Y</param>
    private void UpdateMovement(int horizontal, int vertical)
    {
        
        // Mover al jugador
        Vector2 direction = new Vector2(horizontal, vertical);
        _rb.velocity = direction * speed;
        
        // Verificamos si el jugador se esta moviendo
        if (_rb.velocity.magnitude > 0)
        {
            isWalking = true;
            lastHorizontal = horizontal;
            lastVertical = vertical;
        }
        else
        {
            isWalking = false;
        }
        
    }

    /// <summary>
    /// Chequea si el jugador presiono el boton de ataque para activar el area de ataque
    /// </summary>
    private void CheckAttack()
    {
        // Verificamos si ataca y el timer de ataque es mayor a 0.5f 
        if (Input.GetButtonDown("Fire1") && timerAttack > 0.5f)
        {
            attackArea.SetActive(true);
            _anim.SetTrigger("Attack");
            timerAttack = 0f;
        }
        
        // Cuando pase 0.1s desactivaremos el ataque
        if (timerAttack > 0.1f)
        {
            attackArea.SetActive(false);
        }
    }
    
    
    /// <summary>
    /// Se encarga de actualizar el animator del jugador
    /// </summary>
    private void UpdateAnimator()
    {
        _anim.SetBool("Walk", isWalking);
        _anim.SetFloat("X", lastHorizontal);
        _anim.SetFloat("Y", lastVertical);
    }
    
    /// <summary>
    /// Actualiza la posicion del area de ataque
    /// </summary>
    /// <param name="horizontal">Dirección eje X</param>
    /// <param name="vertical">Dirección eje Y</param>
    private void UpdateAttackArea(int horizontal, int vertical)
    {
        
        Vector2 direction = new Vector2(horizontal, vertical);
        
        if (horizontal != 0 || vertical != 0)
        {
            attackArea.transform.position = 0.5f * direction + (Vector2) transform.position;
        }
        
    }
    
}
