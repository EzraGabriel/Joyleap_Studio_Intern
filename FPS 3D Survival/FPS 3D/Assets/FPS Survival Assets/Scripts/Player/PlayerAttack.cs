using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weaponManager;

    public float fireRate = 15f;

    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCamera;
    private bool zoomed;

    private Camera mainCam;

    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;

    [SerializeField]
    private Transform arrowBowStartPosition;
    void Awake()
    {
        weaponManager = GetComponent<WeaponManager>();

        zoomCamera = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
    }

    void WeaponShoot()
    {
        if(weaponManager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if (Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (weaponManager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                }
                else if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weaponManager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                }
                else
                {
                    if(is_Aiming)
                    {
                        weaponManager.GetCurrentSelectedWeapon().ShootAnimation();

                        if(weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowOrSpear(true);
                        }
                        if (weaponManager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                            ThrowArrowOrSpear(false);
                        }
                    }
                }
            }
        }
    }

    void ZoomInAndOut()
    {
        if(weaponManager.GetCurrentSelectedWeapon().weaponAim == Weapon_Aim.AIM)
        {
            if(Input.GetMouseButtonDown(1))
            {
                zoomCamera.Play(AnimationTags.ZOOM_IN_ANIM);
                crosshair.SetActive(false);
            }

            if (Input.GetMouseButtonUp(1))
            {
                zoomCamera.Play(AnimationTags.ZOOM_OUT_ANIM);
                crosshair.SetActive(true);
            }
        }
        if (weaponManager.GetCurrentSelectedWeapon().weaponAim == Weapon_Aim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                weaponManager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;
            }
        }
    }
    void ThrowArrowOrSpear(bool throwArrow)
    {
        if(throwArrow)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = arrowBowStartPosition.position;
            arrow.GetComponent<ArrowAndBow>().Launch(mainCam);
        }
        else
        {
            GameObject spear = Instantiate(spearPrefab);
            spear.transform.position = arrowBowStartPosition.position;
            spear.GetComponent<ArrowAndBow>().Launch(mainCam);
        }
    }

    void BulletFired()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position,mainCam.transform.forward, out hit))
        {
            if(hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<Health>().ApplyDamage(damage);
                print("kena");
            }
        }
    }
}
