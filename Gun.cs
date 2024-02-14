using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class gunn : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate = 15f;

    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 2.5f;

    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject hitMarker;
    public GameObject impactEffect;

    public Animator animator;


    private float nextTimeToFire = 0f;

    void Start ()
    {
        currentAmmo = maxAmmo;
    }


    void Update()
    {
        if (isReloading)
            return;

        if(currentAmmo<=0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }


    }

    IEnumerator Reload()
    {
        isReloading = true;
        UnityEngine.Debug.Log("reload");


        animator.SetBool("reload", true);

        yield return new WaitForSeconds(reloadTime);

        animator.SetBool("reload", false);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Target enemy = hit.transform.GetComponent<Target>();
            if(enemy !=null)
            {
                enemy.TakeDamage(damage);
                HitActive();
                Invoke("HitDisable", 0.2f);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

        }

    }

    private void HitActive()
    {
        hitMarker.SetActive(true);
    }
    private void HitDisable()
    {
        hitMarker.SetActive(false);
    }
    
}