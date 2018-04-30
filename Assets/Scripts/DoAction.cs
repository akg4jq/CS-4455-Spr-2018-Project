﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoAction : MonoBehaviour
{
    public bool shield_on;
    public GameObject Start_position;
    public Rigidbody bullet;
    public float power;
    public PowerUpManager powerUpManager;
    Animator animator;
    public ParticleSystem particle;
    //float time = 0f;
    bool tomatoIgnore;

    void Start()
    {
        tomatoIgnore = false;
        power = 10f;
        animator = GetComponent<Animator>();
        shield_on = false;
    }
    
    void Update()
    {
        //Debug.LogError("tomatoIgnore:" + tomatoIgnore);
        if (Input.GetButton("Fire1")) //button pressed
        {
            if (Collectibles.pickle_acquired && powerUpManager.pickle_enabled) //pickle selected
            {
                Debug.Log("Pressed");
                if (power < 80f)
                {
                    power += 2f;
                }
            }
            else if (Collectibles.cheese_acquired && powerUpManager.cheese_enabled) //Collectibles.cheese_acquired && 
            {
                //super jump
                animator.SetBool("HighJump", true);
            }
            else if (Collectibles.tomato_acquired && powerUpManager.tomato_enabled) //Collectibles.tomato_acquired &&
            {
                //squish enabled
                animator.SetBool("Squished", true);
                Physics.IgnoreLayerCollision(14, 10);
            }
            else if (Collectibles.lettuce_acquired && powerUpManager.lettuce_enabled)
            {
                //shield enabled
                particle.gameObject.SetActive(true);
                shield_on = true;
            }

        }

        if (Input.GetButtonUp("Fire1")) //button let go
        {
            if (Collectibles.pickle_acquired && powerUpManager.pickle_enabled) //pickle selected
            {
                animator.SetBool("Shoot", true) ;
            }
            //else if (Collectibles.cheese_acquired && powerUpManager.cheese_enabled)
            //{
            //    //super jump
            //}
            else if (Collectibles.tomato_acquired && powerUpManager.tomato_enabled && !tomatoIgnore) //Collectibles.tomato_acquired && 
            {
                    //squish disabled
                    animator.SetBool("Squished", false);
                    Physics.IgnoreLayerCollision(14, 10, false);
            }
            else if (Collectibles.lettuce_acquired && powerUpManager.lettuce_enabled)
            {
                //shield disabled
                particle.gameObject.SetActive(false);
                shield_on = false;
            }
        }
        
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.LogError(other.transform.position.y);
        if (other.tag == "TomatoIgnore")
        {
            //Debug.LogError("ignore: " + other.transform.position.y);
            tomatoIgnore = true;
        }
        if (other.tag == "EndLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Collectibles.pickle_acquired = false;
            Collectibles.cheese_acquired = false;
            Collectibles.tomato_acquired = false;
            Collectibles.lettuce_acquired = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "TomatoIgnore")
        {
            tomatoIgnore = false;
            animator.SetBool("Squished", false);
        }
    }

    public void Shooting()
    {
        Rigidbody instantiated_projectile = Instantiate(bullet, Start_position.transform.position, Start_position.transform.rotation);
        instantiated_projectile.AddForce(Start_position.transform.forward * power/10 + new Vector3(0f, 5f/10, 0f), ForceMode.Impulse);
        power = 10f;
       
    }
    
}
