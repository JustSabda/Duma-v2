﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Character
{
    /*
    This script is part of a larger solution that inherits from a Character script
    If you plan to use this as a standalone script, there are four variables I make in
    the Character script that you will need to make in this script.

    [HideInInspector]
    public bool takingDamage;
    [HideInInspector]
    public bool isDead;
    private Animator anim;
    private Rigidbody2D rb;

    */

    //The maximum number of health points the player can have
    public int maxHealthPoints;
    public int currentHealthPoints;
    //How high the player goes when they receive damage
    [SerializeField]
    private float verticalKnockbackForce;
    //How far backwards the player goes when they receive damage
    [SerializeField]
    private float horizontalKnockbackForce;
    //The max amount of time after receiving damage that the player can no longer receive damage
    [SerializeField]
    private float invulnerabilityTime;
    //How long movement should be disabled after receiving damage
    [SerializeField]
    private float cancelMovementTime;

    //Bool that manages if the player can receive more damage
    [HideInInspector]
    public bool hit;
    //A reference point of whatever caused damage so the player can knockback in the appropriate direction
    [HideInInspector]
    public GameObject enemy;

    //The current number of health points on the player after damage is applied
    
    
    //Unique for this solution if you player uses a CapsuleCollider2D; if you don't have a CapsuleCollider2D, you probably won't need to reference you exact collider type as you don't need to change the direction
    private CapsuleCollider2D playerCollider;

    private PlayerMovement player;

    /*Only use this Start() method if you don't have a Character script but still want to use this solution; if you are using a Character script then you can delete or ignore this method entirely
    private void Start()
    {
        //This is for testing and certain games that refil health when starting a new scene; a lot of games currentHealth doesn't reset to maxHealth at the start of each scene, but for this tutorial it helps manage the values. I would use a PlayerPref int value to manage currentHealth between scenes
        currentHealthPoints = maxHealthPoints;
        //Unique for this solution; I need to reference this Collider type because I need to change the direction of the CapusleCollider2D through code, I can't do it through the animator
        playerCollider = GetComponent<CapsuleCollider2D>();
        //Reference to the Rigidbody2D component to handle knockback force
        rb = GetComponent<Rigidbody2D>();
        //Reference to the Animator component to play animations
        anim = GetComponent<Animator>();
    }
    */

    //Method called in the Character script that acts like a Start() method
    protected override void Initializtion()
    {
        base.Initializtion();
        //This is for testing and certain games that refil health when starting a new scene; a lot of games currentHealth doesn't reset to maxHealth at the start of each scene, but for this tutorial it helps manage the values. I would use a PlayerPref int value to manage currentHealth between scenes
        currentHealthPoints = maxHealthPoints;
        //Unique for this solution; I need to reference this Collider type because I need to change the direction of the CapusleCollider2D through code, I can't do it through the animator
        playerCollider = GetComponent<CapsuleCollider2D>();
        player = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        //The hit bool is set to true in the DamageField script, and changed to false later in this script; this manages if knockback should be applied
        if (hit && !GameManager.Instance.isGameOver)
        {
            HandleKnockBack();
        }

        if (UIManager.Instance.healthImage != null)
            UIManager.Instance.UI_Health(currentHealthPoints, maxHealthPoints);


    }

    //This method is called by any script that would need to handle damage; for this tutorial it is called by the DamageField script
    public void Damage(int amount)
    {
        //First checks to see if the player is currently in an invulnerable state; if not it runs the following logic.
        if (!hit)
        {



            //First sets invulnerable to true
            hit = true;
            //Reduces currentHealthPoints by the amount value that was set by whatever script called this method, for this tutorial in the OnTriggerEnter2D() method
            currentHealthPoints -= amount;

            AudioManager.Instance.PlaySFX("Hit SFX");
            //If currentHealthPoints is below zero, player is dead, and then we handle all the logic to manage the dead state
            if (currentHealthPoints <= 0)
            {
                currentHealthPoints = 0;
                //Unique for my solution in which I use a CapsuleCollider2D; this sets the direction of the CapsuleCollider2D horizontally for the animation of the player dying, this is unique only if you player has a CapsuleCollider2D
                //playerCollider.direction = CapsuleDirection2D.Horizontal;
                //A bool in the Character script is set to true so other scripts can manage logic differently if the player is dead; if you don't have a Character script, then use the commented out variables I provided in this solution so you can manage this state
                character.isDead = true;
                //player.isDead = true;
                GameManager.Instance.isGameOver = true;
                //StartCoroutine(GameOver());
                
                //Play the dead animation
                //anim.SetBool("Dead", true);


            }
        }
    }

 
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.isGameOver = true;
    }


    //This method will move the player in a backwards slightly upwards direction when taking damage; this is a very common feature in nearly all platformers
    private void HandleKnockBack()
    {
        //Bool that lets other scripts know the player is currently taking damage; if you don't have a Character script, then use the commented out variables I provided in this solution so you can manage this state
        character.takingDamage = true;
        //Plays the damage animation
        //anim.SetBool("Damage", true);

        if (enemy.gameObject.tag == "Spike")
        {
            rb.AddForce(Vector2.up * verticalKnockbackForce);
        }
        else
        {
            //Adds a slightly upwards knockback force, this value probably shouldn't be as strong as the horizontal
            //rb.AddForce(Vector2.up * verticalKnockbackForce);
            //This if statement checks to see if you are facing left or right when taking damage; depending on what direction you are facing, the knockback force will be applied appropriately backwards 
            if (transform.position.x < enemy.transform.position.x)
            {
                //If the player is facing right, then backwards knockback would be going to the left
                rb.AddForce(Vector2.left * horizontalKnockbackForce);
            }
            else
            {
                //If the player is facing left, then backwards knockback would be going to the right
                rb.AddForce(Vector2.right * horizontalKnockbackForce);
            }
        }
        //This method is called very quickly to stop knockback forces from being applied
        Invoke("CancelHit", invulnerabilityTime);
        //This method is called less quickly to allow player control again after taking damage
        Invoke("EnableMovement", cancelMovementTime);
    }

    //Method that changes the hit value back to false to stop knockback forces from constantly being applied
    private void CancelHit()
    {
        hit = false;
    }

    //Method that allows player movement again after taking damage; also turns off the animation for taking damage if the player is still alive and gets the player out of the taking damage state as well.
    private void EnableMovement()
    {
        if (!character.isDead)
        {
            //anim.SetBool("Damage", false);
            character.takingDamage = false;
        }
    }
}
