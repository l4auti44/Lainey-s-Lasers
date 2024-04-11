//using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Laser stats")]
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private GameObject laserPivot;
    [SerializeField] private float laserMaxDistance = 30f;
    [SerializeField] private float damage = 15f;
    [Header("Laser Push")]
    [SerializeField] private float pushForce = 7f;
    [SerializeField] private Vector3 pushDirection = new Vector3(0, 0, 1f);
    [Header("DEBUG")]
    [SerializeField] private bool DEBUG = false;
    

    private bool firstRay = true, hitingTarget = false;
    private GameObject previousHit;

    //[SerializeField] private float knockbackStrenght = 4f;
    // Start is called before the first frame update
    void Start()
    {
        laserPivot.transform.localScale = new Vector3(1, laserMaxDistance, 1);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(laserOrigin.position, laserOrigin.up);
        if(DEBUG) Debug.DrawRay(ray.origin, ray.direction * laserMaxDistance, Color.red);

        if (Physics.Raycast(ray, out hit, laserMaxDistance))
        {
            if (firstRay)
            {
                previousHit = hit.transform.gameObject;
                firstRay = false;
            }
            laserPivot.transform.localScale = new Vector3(1, hit.distance, 1);
            // hit: Player
            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * pushForce, ForceMode.Force);
                //hit.transform.GetComponent<Rigidbody>().AddForce(-hit.transform.Find("Orientation").transform.forward * pushForce, ForceMode.Force);
                hit.transform.GetComponent<HealthSystem>().TakeDamage(damage);


            }
            //hit: Target
            if (hit.transform.CompareTag("Target"))
            {
                hitingTarget = true;
                hit.transform.GetComponent<Target>().DoAction();
            }
            else
            {
                hitingTarget = false;
            }

            //Lost target
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
