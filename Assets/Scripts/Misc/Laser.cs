//using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private GameObject laserPivot;
    [SerializeField] private float laserMaxDistance = 30f;
    [SerializeField] private float damage = 15f;

    private bool firstRay = true, hitingTarget = false;
    private GameObject previousHit;
    private AudioSource damageAudio;
    //[SerializeField] private float knockbackStrenght = 4f;

    //[SerializeField] private bool DEBUG = false;
    // Start is called before the first frame update
    void Start()
    {
        damageAudio = GetComponent<AudioSource>();
        laserPivot.transform.localScale = new Vector3(1, laserMaxDistance, 1);
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;   
        Ray ray = new Ray(laserOrigin.position, laserOrigin.up);
        Debug.DrawRay(ray.origin, ray.direction * laserMaxDistance, Color.red);

        if (Physics.Raycast(ray, out hit, laserMaxDistance))
        {
            if (firstRay)
            {
                previousHit = hit.transform.gameObject;
                firstRay = false;
            }
            laserPivot.transform.localScale = new Vector3(1, hit.distance, 1);
            if (hit.transform.CompareTag("Player"))
            {
                //if (!damageAudio.isPlaying && !GameController.isPaused)
                //{
                    //damageAudio.Play();
                //}
                
                hit.transform.parent.GetComponent<HealthSystem>().TakeDamage(damage);


            }
            if (hit.transform.CompareTag("Target"))
            {
                hitingTarget = true;
                hit.transform.GetComponent<Target>().DoAction();
            }
            else
            {
                hitingTarget = false;
            }
            if (hit.transform.name != previousHit.name && !hitingTarget)
            {
                if (previousHit.GetComponent<Target>())
                {
                    previousHit.GetComponent<Target>().Undo();
                }
            }

            previousHit = hit.transform.gameObject;
            
        }
        else
        {
            laserPivot.transform.localScale = new Vector3(1, laserMaxDistance, 1);
        }
    }
}
