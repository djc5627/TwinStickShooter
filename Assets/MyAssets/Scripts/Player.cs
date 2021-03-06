﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int playerNumber = 1;
    public int maxHealth = 3;
    [Tooltip("Number of shots per second")]
    public float fireRate = 10f;
    [Tooltip("Force of Bullets")]
    public float bulForce = 3f;
    [Tooltip("Movement force multiplier")]
    public float forceMult = 10f;
    public float maxSpeed = 5f;
    [Tooltip("Drag for deacceleration for when player stops")]
    public float stopDrag = 5f;
    [Tooltip("UI Health text")]
    public Text healthText;
    public Text moneyText;
    public float thrustForceMult = 10000f;
    public float thrustMaxSpeed = 200f;
    public float thrustStopDrag = 10f;
    public float thrustDuration = .2f;
    [Tooltip("Time between thrust finish and next thrust ready")]
    public float thrustDelay = .05f;
    public float firePointOffset = .2f;

    public GameObject ammo;
    public Transform firePoint;
    public Transform centerPivot;
    public ParticleSystem thrustEffect;


    private int currHealth;
    private int moneyCount = 0;
    private float nextFire = 0f;

    private bool isThrusting = false;

    //For setting back to def after thrust
    private float initForceMult;
    private float initMaxSpeed;
    private float initStopDrag;

    

    private Rigidbody2D rb;
    private InputManager playerInput;

    // Use this for initialization
    void Start()
    {
        initForceMult = forceMult;
        initMaxSpeed = maxSpeed;
        initStopDrag = stopDrag;

        currHealth = maxHealth;
        moneyCount = 0;
        healthText.text = "Health: " + currHealth;
        moneyText.text = "Money: " + moneyCount;
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        AimFire();
        ThurstChecker();
    }

    //Move physics here
    void FixedUpdate()
    {
        Move();
    }

    //Move the player in direction of left stick by adding force
    private void Move()
    {
        //Keep player from rotating from previous collision
        rb.angularVelocity = 0;

        //If left stick not at default pos rotate the player and move
        if (playerInput.movedL_P1)
        {
            if (!playerInput.movedR_P1)
                rb.rotation = playerInput.rotZL_P1 - 90;
            rb.AddForce(playerInput.rawL_P1 * forceMult);
            rb.drag = 0f;
        }
        //else deaccelerate to a stop by setting drag
        else
        {
            rb.drag = stopDrag;
        }
        //Cap speed at max
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    //Aim from Rstick and fire bullet by firerate
    private void AimFire()
    {
        //If right stick not at default pos rotate the firepoint & player and shoot
        if (playerInput.movedR_P1)
        {
            centerPivot.rotation = Quaternion.Euler(0f, 0f, playerInput.rotZR_P1 - 90);
            Debug.DrawRay(firePoint.position, firePoint.up, Color.green);
            rb.rotation = playerInput.rotZR_P1 - 90;

            //shoot at firerate = bullets/sec
            if (Time.time > nextFire)
            {
                //ShotgunFire(3, 10f);
                //DefualtFire();
                BurstFire();
                nextFire = Time.time + 1 / fireRate;
            }
        }
    }

    //Fire a single bullet
    private void FireBullet()
    {
        //Instantiate the bullet in direction of Rstick and set velocity
        GameObject bul = Instantiate(ammo, firePoint.position, centerPivot.rotation, GameObject.Find("BulletContainer").transform);
        Rigidbody2D bulRB = bul.GetComponent<Rigidbody2D>();
        bulRB.AddForce(bul.transform.up * 100f * bulForce);
    }


    //Fire burst of bullets
    private void BurstFire()
    {
        //Arc within which bullets will be fired
        float arcDeg = 45f;
        int bulletCount = 10;
        for (int i=0; i < bulletCount; i++)
        {
            float rotZ = Random.Range(centerPivot.rotation.eulerAngles.z - arcDeg / 2, centerPivot.rotation.eulerAngles.z + arcDeg / 2);

            GameObject tempBul = Instantiate(ammo, firePoint.position, Quaternion.Euler(0f, 0f, rotZ), GameObject.Find("BulletContainer").transform);
            Rigidbody2D tempRB = tempBul.GetComponent<Rigidbody2D>();

            tempRB.AddForce(tempRB.transform.up * 100f * bulForce);
        }
    }

    //The default firing mode
    private void DefualtFire()
    {
        //Instantiate the bullet in direction of Rstick and set velocity
        Vector3 offset = firePoint.TransformVector(new Vector3(.2f, 0f, 0f));
        GameObject bul1 = Instantiate(ammo, firePoint.position + offset, centerPivot.rotation, GameObject.Find("BulletContainer").transform);
        GameObject bul2 = Instantiate(ammo, firePoint.position - offset, centerPivot.rotation, GameObject.Find("BulletContainer").transform);
        Rigidbody2D bulRB1 = bul1.GetComponent<Rigidbody2D>();
        Rigidbody2D bulRB2 = bul2.GetComponent<Rigidbody2D>();
        bulRB1.AddForce(bul1.transform.up * 100f * bulForce);
        bulRB2.AddForce(bul2.transform.up * 100f * bulForce);
    }

    //Shoot bCount bullets in an arc of arcDeg degrees where player aiming
    //bCount MUST BE ODD
    private void ShotgunFire(int bCount, float arcDeg)
    {
        //throw log/return if not odd #
        if (bCount % 2 != 1)
        {
            Debug.Log("Bullet count must be odd for shotgun!!!");
            return;
        }
        int range = (bCount - 1) / 2;
        //angle between each bullet to cover arcDeg
        float spread = arcDeg / (bCount - 1);  

        for (int i=-range; i<= range; i++)
        {
            GameObject tempBul = Instantiate(ammo, firePoint.position, Quaternion.Euler(0f, 0f, centerPivot.rotation.eulerAngles.z + (i*spread)), GameObject.Find("BulletContainer").transform);
            Rigidbody2D tempRB = tempBul.GetComponent<Rigidbody2D>();

            tempRB.AddForce(tempRB.transform.up * 100f * bulForce);

        }
    }

    //Handle player dying
    //For now just reload the scene
    //TODO: Dont call in update for perf
    private void Death()
    {
        if (currHealth < 1)
        {
            SceneManager.LoadScene("Prototype Rooms", LoadSceneMode.Single);
            Debug.Log("Player Died!!!");
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //If collide with enemy, take damage
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            incHealth(-1);
            healthText.text = "Health: " + currHealth;
            Destroy(col.gameObject);
        }

        //If collide enemy bullet, take damage

        else if(col.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            incHealth(-1);
            healthText.text = "Health: " + currHealth;
            Destroy(col.gameObject);
        }
    }

    //Increment money by amt
    //For use by coin
    public void incMoney(int amt)
    {
        moneyCount += amt;
        moneyText.text = "Money: " + moneyCount;
    }

    //increment current health by inc and check if dead
    public void incHealth(int inc)
    {
        currHealth += inc;
        Death();
    }

    //Go faster with thrust by changing mov variables
    private void ThurstChecker()
    {
        if (playerInput.thrustIsPressed && !isThrusting)
        {
            StartCoroutine(Thurst());
        }

    }


    //Go faster with thrust by changing mov variables
    IEnumerator Thurst()
    {
        isThrusting = true;

        ParticleSystem effect = Instantiate(thrustEffect, transform);
        

        //dont allow player to change dir during thrust
        forceMult = 0f;
        maxSpeed = thrustMaxSpeed;
        stopDrag = thrustStopDrag;
        rb.AddForce(playerInput.normL_P1 * thrustForceMult);

        yield return new WaitForSeconds(thrustDuration);

        forceMult = initForceMult;
        maxSpeed = initMaxSpeed;
        stopDrag = initStopDrag;

        effect.Stop();
        Destroy(effect.gameObject, .5f);

        yield return new WaitForSeconds(thrustDelay);
        isThrusting = false;
        yield return null;
        
    }
}
