using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandedWeaponScript : MonoBehaviour
{
    bool rightHandHasWeapon;
    bool leftHandHasWeapon;
    bool rightHandHasShield;
    bool leftHandHasShield;

    public string currentAnimation;
    public string previousAnimation;

    Transform leftHand;
    Transform rightHand;
    public GameObject hitParticle;


    private Transform handSwinging;

    private bool isSwinging;
    private bool canSwing;

    public List<GameObject> hitThings;
    private Transform weaponSwung;
    float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 1;
        canSwing = true;

        leftHand = GetComponent<BaseStats>().leftHand;
        rightHand = GetComponent<BaseStats>().rightHand;
    }

    // Update is called once per frame
    void Update()
    {



        bool isBlocking = GetComponent<BaseStats>().isBlocking;
        
            leftHandHasWeapon = GetComponent<BaseStats>().leftHandHasWeapon;
            rightHandHasWeapon = GetComponent<BaseStats>().rightHandHasWeapon;

            AnimatorClipInfo[] animatorInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
            currentAnimation = animatorInfo[0].clip.name;

            if ((leftHandHasWeapon && rightHandHasWeapon) || rightHandHasWeapon)
            {
                GetComponent<Animator>().SetInteger("WeaponType", 1);
            }
            else
            {
                GetComponent<Animator>().SetInteger("WeaponType", 2);
            }

            if (previousAnimation == "Armature_One Handed Swing Right" || previousAnimation == "Armature_One Handed Swing Left" && currentAnimation != "Armature_One Handed Swing Left" && currentAnimation != "Armature_One Handed Swing Right")
            {
                canSwing = true;
            }


        if(isBlocking == true)
        {
            if (!GetComponent<momementScript>().speedMods.Contains(0.4f))
            {
                GetComponent<momementScript>().speedMods.Add(0.4f);
            }




            GetComponent<Animator>().SetBool("IsBlocking", true);
            if (timeLeft == 1)
            {
                GetComponent<Animator>().SetTrigger("StartBlocking");
            }

            timeLeft = 0;
        }
        else 
        {
            if (GetComponent<momementScript>().speedMods.Contains(0.4f))
            {
                GetComponent<momementScript>().speedMods.Remove(0.4f);
            }
            GetComponent<Animator>().SetBool("IsBlocking", false);
            timeLeft = 1;
            if (Input.GetMouseButton(0) && (currentAnimation == "Armature_Walking _Top" || currentAnimation == "Armature_Idle_Top") && canSwing)
            {
                canSwing = false;
                GetComponent<Animator>().SetTrigger("Hit");
            }
            else if (Input.GetMouseButton(0) && currentAnimation == "Armature_One Handed Swing Right" && leftHandHasWeapon)
            {
                GetComponent<Animator>().SetBool("Still Hitting", true);
            }

            if (currentAnimation == "Armature_One Handed Swing Left")
            {
                GetComponent<Animator>().SetBool("Still Hitting", false);
            }




            previousAnimation = currentAnimation;


            if (isSwinging == true)
            {
                for (int i = 0; i < weaponSwung.GetComponent<ItemScript>().hitBoxes.amount.Count; i++)
                {
                    List<float> parameters = weaponSwung.GetComponent<ItemScript>().hitBoxes.amount[i].parameters;
                    Collider[] thingsNowHit = Physics.OverlapBox(weaponSwung.position + new Vector3(parameters[0], parameters[1], parameters[2]), new Vector3(parameters[3], parameters[4], parameters[5]), weaponSwung.rotation);
                    foreach (Collider thing in thingsNowHit)
                    {
                        if (thing.GetComponent<DamageTaker>() && !hitThings.Contains(thing.gameObject))
                        {
                            hitSomething(thing.ClosestPoint(weaponSwung.position), thing.gameObject);
                            hitThings.Add(thing.gameObject);
                        }
                        else if (thing.GetComponent<Redirector>() && !hitThings.Contains(thing.GetComponent<Redirector>().Direction))
                        {
                            if (thing.GetComponent<Redirector>().Direction.GetComponent<DamageTaker>())
                            {
                                hitSomething(thing.ClosestPoint(weaponSwung.position), thing.GetComponent<Redirector>().Direction);
                                hitThings.Add(thing.GetComponent<Redirector>().Direction);
                            }
                        }
                    }
                }
            }
        }
    }


    public void StartSwinging(int hand)
    {
        hitThings.Clear();
        if(hand == 1)
        {
            handSwinging = rightHand;
            foreach (Transform child in rightHand.transform)
            {
                if (child.GetComponent<ItemScript>().itemType == 1)
                {
                    weaponSwung = child;
                    break;
                }
                weaponSwung = null;
            }
        } else
        {
            handSwinging = leftHand;
            foreach (Transform child in leftHand.transform)
            {
                if (child.GetComponent<ItemScript>().itemType == 1)
                {
                    weaponSwung = child;
                    break;
                }
                weaponSwung = null;
            }
        }

        isSwinging = true;
    }

    public void EndSwinging()
    {
        isSwinging = false;
        hitThings.Clear();
    }

    void hitSomething(Vector3 hitArea, GameObject thingHit)
    {
        thingHit.GetComponent<DamageTaker>().takeDamage(weaponSwung.GetComponent<ItemScript>().weaponDamage);
        GameObject part = Instantiate(hitParticle, hitArea, transform.rotation);
        part.transform.LookAt(transform.position);
        part.GetComponent<ParticleSystem>().Play();
    }
}
