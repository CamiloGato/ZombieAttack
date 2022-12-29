using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject attackArea;
    private Rigidbody2D _rb;
    private Animator _anim;
    
    private int lastHorizontal, lastVertical;
    private float timer;
    private bool isWalking;
    
    private void Awake()
    {
        attackArea.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        // Recibimos el input del usuario
        int horizontal = (int) Input.GetAxisRaw("Horizontal");
        int vertical = (int) Input.GetAxisRaw("Vertical");
        
        // Movimiento del jugador usando el rigidbody
        Vector2 direction = new Vector2(horizontal, vertical);
        _rb.velocity = direction * speed;
        
        // Verificamos si la velocidad es mayor a 0
        // Entonces el jugador se esta moviendo
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
        
        // Asignamos las variables al animator (Actualizamos el animator)
        _anim.SetBool("Walk", isWalking);
        _anim.SetFloat("X", lastHorizontal);
        _anim.SetFloat("Y", lastVertical);

        // Verificaremos cuando se presione el botón Fire1 para atacar
        bool isAttacking = Input.GetButtonDown("Fire1");
        if (isAttacking && timer > 0.5f)
        {
            attackArea.SetActive(true);
            _anim.SetTrigger("Attack");
            timer = 0f;
        }
        // Cuando pase 0.1s desactivaremos el ataque
        if (timer > 0.1f)
        {
            attackArea.SetActive(false);
        }

        // Actualizar posicion de nuestro Attack Area
        if (horizontal != 0 || vertical != 0)
        {
            attackArea.transform.position = 0.5f * direction + (Vector2) transform.position;
        }
        
    }
}
