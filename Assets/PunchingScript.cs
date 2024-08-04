using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingScript : MonoBehaviour
{
    float timeLeft = 0;
    bool firstHitLanded = false;
     bool is_blocking;
    public float attackSpeed;
    public List<int> DamageBonuses;
    public Transform handR;
    public Transform handL;
    public float attackRange;
    public GameObject hitParticle;
    public int damage;
    string previousAnimation;
    string currentAnimation;


    private List<GameObject> hitThings;
    // Start is called before the first frame update
    void Start()
    {
        hitThings = new List<GameObject>();
        currentAnimation = "Armature_Idle_Top";
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BaseStats>().rightHandHasWeapon == false && GetComponent<BaseStats>().leftHandHasWeapon == false)
        {
            GetComponent<Animator>().SetInteger("WeaponType", 0);
            GetComponent<Animator>().SetFloat("AttackSpeed", attackSpeed);
            if (Input.GetMouseButton(1))
            {
                if (!GetComponent<momementScript>().speedMods.Contains(0.4f))
                {
                    GetComponent<momementScript>().speedMods.Add(0.4f);
                }




                GetComponent<Animator>().SetBool("IsBlocking", true);
                is_blocking = true;
                if (timeLeft == 1)
                {
                    GetComponent<Animator>().SetTrigger("StartBlocking");
                }

                timeLeft = 0;
            }
            else
            {
                AnimatorClipInfo[] animatorInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
                currentAnimation = animatorInfo[0].clip.name;


                if (Input.GetMouseButton(0) && currentAnimation == "Armature_Idle_Top" && firstHitLanded == false)
                {
                    GetComponent<Animator>().SetTrigger("Hit");
                    firstHitLanded = true;
                }
                else if (Input.GetMouseButton(0) && currentAnimation == "Armature_Punch1")
                {
                    GetComponent<Animator>().SetBool("Still Hitting", true);
                }
                else if (Input.GetMouseButton(0) && currentAnimation == "Armature_Walking _Top" && firstHitLanded == false)
                {
                    GetComponent<Animator>().SetTrigger("Hit");
                    firstHitLanded = true;
                }

                //Check States Here:

                if (currentAnimation != "Armature_Punch1" && previousAnimation == "Armature_Punch1")
                {
                    firstHitLanded = false;
                }

                //print(currentAnimation);

                previousAnimation = currentAnimation;

                if (GetComponent<momementScript>().speedMods.Contains(0.4f))
                {
                    GetComponent<momementScript>().speedMods.Remove(0.4f);
                }
                GetComponent<Animator>().SetBool("IsBlocking", false);
                timeLeft = 1;

            }
        }
    }

    void Punch(int hand)
    {
        //print("Hit");

        Vector3 hitPosition = new Vector3(0,0,0);

        if (hand == 1)
        {
            hitPosition = handR.position;
        } else
        {
            hitPosition = handL.position;
        }


        Collider[] hitArea = Physics.OverlapSphere(hitPosition, attackRange);


        foreach (Collider thing in hitArea)
        {
            //print(thing);
            if (thing.GetComponent<DamageTaker>() && !hitThings.Contains(thing.gameObject))
            {
                thing.GetComponent<DamageTaker>().takeDamage(10);
                GameObject part = Instantiate(hitParticle, hitPosition, transform.rotation);
                part.transform.LookAt(transform.position);
                part.GetComponent<ParticleSystem>().Play();
                hitThings.Add(thing.gameObject);
            } else if (thing.GetComponent<Redirector>())
            {
                if (thing.GetComponent<Redirector>().Direction.GetComponent<DamageTaker>() && !hitThings.Contains(thing.GetComponent<Redirector>().Direction))
                {
                    thing.GetComponent<Redirector>().Direction.GetComponent<DamageTaker>().takeDamage(10);
                    GameObject part = Instantiate(hitParticle, hitPosition, transform.rotation);
                    part.transform.LookAt(transform.position);
                    part.GetComponent<ParticleSystem>().Play();
                    hitThings.Add(thing.GetComponent<Redirector>().Direction);
                }
            }
            
        }


        hitThings.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(handL.position, attackRange);
    }
}
