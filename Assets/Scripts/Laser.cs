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
    [Header("Cooldown")]
    private bool cooldown = false;
    [SerializeField] private float cooldownTimer = 0.5f;
    private float _cooldownTimer = 0.5f;
    [Header("DEBUG")]
    [SerializeField] private bool DEBUG = false;
    

    private bool firstRay = true, hitingTarget = false;
    private GameObject previousHit;
    [SerializeField] private float timerForPush = 0.1f;
    private float _timerForPush;
    private string hitTag;
 
    // Start is called before the first frame update
    void Start()
    {
        _cooldownTimer = cooldownTimer;
        _timerForPush = timerForPush;
        laserPivot.transform.localScale = new Vector3(1, laserMaxDistance, 1);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(laserOrigin.position, laserOrigin.up);
        
        if (DEBUG) RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(ray, 0.1f, maxDistance: laserMaxDistance,RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor);

        
        if (Physics.SphereCast(ray, 0.1f, out hit, laserMaxDistance))
        {
            if (firstRay)
            {
                previousHit = hit.transform.gameObject;
                firstRay = false;
            }
            laserPivot.transform.localScale = new Vector3(1, hit.distance + 0.08f, 1); //0.08 is an offset that fixed laser visual bug (shortening)
            hitTag = hit.transform.tag;
            switch (hitTag)
            {
                case "Player":
                    //HIT PLAYER
                    Transform playerPos = hit.transform.Find("Orientation").transform;
                    Vector3 forceDirection = playerPos.position - hit.point;
                    if (_timerForPush <= 0)
                    {
                        hit.transform.GetComponent<Rigidbody>().AddForce(forceDirection * pushForce, ForceMode.Force);
                        _timerForPush = timerForPush;
                    }
                    else
                    {
                        _timerForPush -= Time.deltaTime;
                    }


                    if (!cooldown)
                    {
                        //take hit
                        hit.transform.GetComponent<HealthSystem>().TakeDamage(damage);
                        cooldown = true;
                    }
                    break;

                case "Target":
                    hitingTarget = true;
                    hit.transform.GetComponent<Target>().DoAction();
                    break;

                case "DestructibleObject":
                    if (hit.transform.GetComponent<DestructibleObject>())
                    {
                        hit.transform.GetComponent<DestructibleObject>().ResetGameObject();
                    }
                    break;

                default:
                    break;
            }

            
            if (!hit.transform.CompareTag("Target"))
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
        if (cooldown)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                cooldown = false;
                _cooldownTimer = cooldownTimer;
            }
        }
    }
}
