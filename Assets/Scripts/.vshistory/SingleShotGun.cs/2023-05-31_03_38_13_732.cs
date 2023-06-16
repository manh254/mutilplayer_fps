using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.ComponentModel;


public class SingleShotGun : Gun
{
    public static SingleShotGun Instance;
    private float nextTimeToFire = 0f;
    [SerializeField] Camera camera;
    [SerializeField] TMP_Text ammo;
    PhotonView PV;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioSource gunSound;
    // [SerializeField] Animator gunRecoilAni;
    public Image hitIndicator;
    public int currentAmmo;
    public ParticleSystem muzzelFlash;
    private float recoilRate = 0f;
    private float maxRecoidRate = 0.2f;
    public bool isReloading = false;
    public static bool canShoot = true;
    public static bool isShooting = false;
    Animator gunAnimator;
    [DefaultValue(false)]
    public bool IsShooting { get; set; }
    [SerializeField] private PlayerController playerController;
    private float zoomInTime = 0.3f;
    [Range(0, 3)]
    float tempTime = 0f;
    [Range(0, 3f)] float recoilRateTime = 0f;
    [SerializeField] Image image;
    void Awake()
    {
        
        Instance = this;
        //playerController = GetComponent<PlayerController>();
        gunAnimator = GetComponentInChildren<Animator>();
        gunSound = GetComponent<AudioSource>();
        currentAmmo = ((GunInfo)itemInfo).maxAmmo;
        PV = GetComponent<PhotonView>();


    }

  
    
    private void OnEnable()
    {
        if (currentAmmo > 0 && !isReloading)
        {
            canShoot = true;
        }
    }
    public override void Use()
    {
        Shoot();
    }
    void Update()
    {
        if(currentAmmo>0 && !isReloading)
        {
            canShoot = true;
        }
        Debug.Log(tempTime);
        Mathf.Clamp(tempTime, 0f, 0.5f);

        if (Input.GetKey(KeyCode.Mouse0) && currentAmmo > 0 && playerController.IsMoving == true)
        {

            recoilRate = Mathf.SmoothStep(recoilRate, maxRecoidRate, recoilRateTime / 3f);
            recoilRateTime += Time.deltaTime;

        }
        else
        {
            if (recoilRateTime > 0f)
            {
                recoilRateTime -= Time.deltaTime;

            }
            recoilRate = Mathf.SmoothStep(0, recoilRate, recoilRateTime / 3f);

        }
        recoilRateTime = Mathf.Clamp(recoilRateTime, 0f, 3f);
        //Debug.Log("rc: " + recoilRate);


        if (Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            StartCoroutine(Reload());
        }

        if (playerController.itemIndex == 0 && Input.GetKeyUp(KeyCode.Mouse0) && isReloading == false)
        {
            gunAnimator.enabled = false;

        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            ZoomIn();
        }
        else ZoomOut();
        Debug.Log(canShoot);
    }
    void Shoot()
    {

        if (playerController.itemIndex == 0 && currentAmmo > 0)
        {
            
            if (canShoot == true)
            {
                gunAnimator.enabled = true;
                isShooting = true;
                if (Time.timeSinceLevelLoad >= nextTimeToFire)
                {
                    StartCoroutine(StartShootingAnimation());

                    muzzelFlash.Play();
                    Vector3 spreadIntensity = camera.transform.forward + new Vector3(Random.Range(-recoilRate, recoilRate), Random.Range(-recoilRate, recoilRate), Random.Range(-recoilRate, recoilRate));


                    gunSound.Play();
                    currentAmmo -= 1;
                    nextTimeToFire = Time.timeSinceLevelLoad + 1 / ((GunInfo)itemInfo).fireRate;

                    //            float x = Random.Range(-1f, 1f) * 0.1f;
                    //float y = Random.Range(-1f, 1f) * 0.1f;

                    //            camera.transform.position += new Vector3(x, y, y);

                    Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                    ray.origin = camera.transform.position;
                    if (Physics.Raycast(ray.origin, spreadIntensity, out RaycastHit hit, ((GunInfo)itemInfo).range))
                    {

                        //Neu gameObject co IDamagable thi apply damage len nhan vat do.
                        hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                        if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
                        {
                            StartCoroutine(showHitIndicator());
                        }
                        PV.RPC("RPC_Shoot", RpcTarget.All, hit.point);
                    }
                }
            }
        }
        if (playerController.itemIndex == 1 && currentAmmo > 0)
        {
            gunAnimator.enabled = true;
            if (canShoot == true)
            {

                isShooting = true;
                if (playerController.itemIndex == 1)
                {
                    if (Time.timeSinceLevelLoad >= nextTimeToFire)
                    {
                        canShoot = true;
                    }
                    else
                    {
                        canShoot = false;

                    }
                    StartCoroutine(StartShootingAnimation());

                    muzzelFlash.Play();


                    gunSound.PlayOneShot( shootSound);
                    currentAmmo -= 1;
                    nextTimeToFire = Time.timeSinceLevelLoad + 1 / ((GunInfo)itemInfo).fireRate;

                    //            float x = Random.Range(-1f, 1f) * 0.1f;
                    //float y = Random.Range(-1f, 1f) * 0.1f;

                    //            camera.transform.position += new Vector3(x, y, y);

                    Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                    ray.origin = camera.transform.position;
                    if (Physics.Raycast(ray, out RaycastHit hit, ((GunInfo)itemInfo).range))
                    {

                        //Neu gameObject co IDamagable thi apply damage len nhan vat do.
                        hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                        if (hit.collider.gameObject.GetComponent<IDamageable>() != null)
                        {
                            StartCoroutine(showHitIndicator());
                        }
                        PV.RPC("RPC_Shoot", RpcTarget.All, hit.point);
                    }
                }
            }


        }
        if (currentAmmo <= 0)
        {
            isReloading = true;
            canShoot = false;
            Debug.Log("Reloading ...");
            StartCoroutine(Reload());
        }



    }
    public void ZoomOut()
    {
        
        transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(-0.275f, 0.15f, -0.39f), Mathf.Clamp(tempTime / zoomInTime, 0f, 1));
        transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.55f, 1.55f, 1.55f), Mathf.Clamp(tempTime / zoomInTime, 0f, 1));
        tempTime -= Time.deltaTime;
        tempTime = Mathf.Clamp(tempTime, 0f, zoomInTime);
    }
    public void ZoomIn()
    {
        
        transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(-0.275f,0.15f, -0.39f), Mathf.Clamp(tempTime / zoomInTime, 0f, 1f));
        transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.55f, 1.55f, 1.55f), Mathf.Clamp(tempTime / zoomInTime, 0f, 1));
        tempTime += Time.deltaTime;
        tempTime = Mathf.Clamp(tempTime, 0f, zoomInTime);
    }


    public IEnumerator Reload()
    {
        gunSound.PlayOneShot(reloadSound);
        canShoot = false;
        isReloading = true;
        gunAnimator.enabled = true;
        StartCoroutine(StartReloadingAnimation());
        yield return new WaitForSeconds(2f);
        currentAmmo = ((GunInfo)itemInfo).maxAmmo;
        canShoot = true;
        //Debug.Log("ok");
        isReloading = false;
        yield break;
    }

    public IEnumerator StartShootingAnimation()
    {
        gunAnimator.enabled = true;
        gunAnimator.Play("ShootingAnimation");
        yield return new WaitForSeconds(0.5f);
        if (playerController.itemIndex == 1)
        {
            gunAnimator.Play("Idle");
        }
        yield break;

    }

    

    public IEnumerator StartReloadingAnimation()
    {


        this.gameObject.GetComponentInChildren<Animator>().Play("ReloadingAnimation");

        yield return null;

    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y, -10f);
            yield return new WaitForEndOfFrame();

            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;
    }




    IEnumerator showHitIndicator()
    {
        hitIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitIndicator.gameObject.SetActive(false);
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition)
    {

        GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition, Quaternion.identity);
        Destroy(bulletImpactObj, 5f);
    }
}
