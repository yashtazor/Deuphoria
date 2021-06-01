using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public GameObject firePoint;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private AudioSource gunShot;
    Camera cam;
    private GameObject player;
    HUD playerHUD;


    public LayerMask zombielayer;
    public float soundRange = 30f;



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        player = GameObject.Find("The Adventurer Blake (1)");
        playerHUD = player.GetComponent<HUD>();
        gunShot = GetComponent<AudioSource>();
        muzzleFlash.transform.position = firePoint.transform.position + new Vector3(0f, 0f, 0.00819f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (playerHUD.bulletCount > 0)
            {
                Shoot();
                playerHUD.bulletCount -= 1;
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            Pickup();
        }
    }
    void Shoot()
    {
        
        muzzleFlash.Play();
        gunShot.Play();
        RaycastHit hit;
        if(Physics.Raycast(firePoint.transform.position,firePoint.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }
            GameObject impactGO=Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }


        //Detecting shooting sound
        Collider[] zombies = Physics.OverlapSphere(transform.position, soundRange, zombielayer);

        for (int i = 0; i < zombies.Length; i++)
        {
            zombies[i].GetComponent<EnemyAI>().OnAware();
        }

    }
    void Pickup()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.tag == "Bullet")
            {
                Debug.Log(hit.transform.gameObject.tag);
                Destroy(hit.transform.gameObject);
                playerHUD.bulletCount += 5;
            }
            if(hit.transform.gameObject.tag == "Health")
            {
                Debug.Log(hit.transform.gameObject.tag);
                Destroy(hit.transform.gameObject);
                playerHUD.potionCount += 1;
            }

        }
    }
}
