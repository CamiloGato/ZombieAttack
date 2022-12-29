using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public float repulsion = 1f;
    public int maxHealth = 100;
    public Slider healthBar;
    
    private SpawnManager _spawnManager;
    private GameObject _player;
    private Rigidbody2D _rb;
    private Animator _anim;

    private int lastHorizontal, lastVertical;
    private bool canMove = true;
    private float timeToMove, timeToDmg;
    private int currentHealth;
    private void Awake()
    {
        _spawnManager = GameObject.Find("SpawnerManager").GetComponent<SpawnManager>();
        currentHealth = maxHealth;
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");

        healthBar.maxValue = maxHealth;
    }
    
    void Update()
    {
        // Timer
        timeToMove += Time.deltaTime;
        timeToDmg += Time.deltaTime;
        
        // Actualizar barra de vida
        healthBar.value = currentHealth;
        
        // Actualizar Movimiento
        if (timeToMove >= 1f)
        {
            canMove = true;
        }
        
        // Obtener el vector de la dirección a la cual se encuentra el jugador
        Vector2 direction = _player.transform.position - transform.position;
        direction.Normalize();
        if (canMove)
        {
            _rb.velocity = direction * speed;
        }
        
        // Actualizar en animator
        int horizontal = (int) Mathf.Sign(_rb.velocity.x);
        int vertical = (int) Mathf.Sign(_rb.velocity.y);
        
        // Creamos variable para verificar si el jugador se esta moviendo
        bool isWalking;
        
        // Comprobamos si está mirando a cualquier lado
        if (horizontal != 0)
        {
            lastHorizontal = horizontal;
        }
        
        // Verificamos si la velocidad es mayor a 0
        // Entonces el jugador se esta moviendo
        if (_rb.velocity.magnitude > 0)
        {
            isWalking = true;
            lastVertical = vertical;
        }
        else
        {
            isWalking = false;
        }
        
        // Asignamos las variables al animator
        _anim.SetBool("Walk", isWalking);
        _anim.SetFloat("X", lastHorizontal);
        _anim.SetFloat("Y", lastVertical);
        
        // Comprobamos si el enemigo ya no tiene vida
        if (currentHealth <= 0)
        {
            _spawnManager.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("AttackArea") && timeToDmg >= 1f)
        {
            timeToDmg = 0f;
            canMove = false;
            timeToMove = 0f;
            Vector2 direction = transform.position - col.transform.position;
            direction.Normalize();
            _rb.AddForce(direction * repulsion, ForceMode2D.Impulse);
            currentHealth -= 10;
            _anim.SetTrigger("Dmg");
        }
    }
}
