using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HUD : MonoBehaviour
{
    public float maxHealth = 100;
    public float curHealth;
    public Slider healthBar;
    public Text death;
    public Text bullets;
    public Text potions;
    public int bulletCount;
    public int potionCount;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        healthBar.value = curHealth;
        healthBar.maxValue = 100;
        animator = GetComponent<Animator>();
        death.gameObject.SetActive(false);
        bulletCount = 10;
        potionCount = 1;
        bullets.text = "Bullets : X" + bulletCount;
        potions.text = "Potions : X" + potionCount;
    }
    //Screenshot
    // Update is called once per frame
    void Update()
    {
        //health bar mechanics
        if(Input.GetKeyDown(KeyCode.F))
        {
            sendDamage(10);
        }
        if(healthBar.value==0)
        {
            //death.gameObject.SetActive(true);
            SceneManager.LoadScene("YouDied");
        }
        //Inventory pickup
        bullets.text = "Bullets : X" + bulletCount;
        potions.text = "Potions : X" + potionCount;
        //Potion drinking
        if(Input.GetKeyDown(KeyCode.E) && potionCount>0)
        {
            potionCount -= 1;
            if(curHealth != maxHealth)
            {
                curHealth += maxHealth - curHealth;
                healthBar.value = curHealth;
            }
        }
    }
    public void sendDamage(float damage)
    {
        //health damage calculation
        curHealth -= damage;
        healthBar.value = curHealth;
    }
    //..Screenshot
}
