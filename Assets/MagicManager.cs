using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell
{
    public int spellAnim;
    public GameObject projectile;
    public float projectileSpeed;
    public float projectileSize;
    public int spellDamageMod;
    public string damageType;
    public Color color;

    public bool isMultiShot;
    
}

public class MagicManager : MonoBehaviour
{
    public List<Spell> spells;
    Transform leftHand;
    Transform rightHand;
    Spell currentSpell;

    private GameObject rightShot;

    private GameObject leftShot;


    bool followUpStarted;
    string currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        leftHand = GetComponent<BaseStats>().leftHand;
        rightHand = GetComponent<BaseStats>().rightHand;
    }



    // Update is called once per frame
    void Update()
    {
        if(rightShot != null)
        {
            if(rightShot.transform.parent == rightHand)
            {
                rightShot.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 90, 0);
            }
            
        }

        if (leftShot != null)
        {
            if (leftShot.transform.parent == leftHand)
            {
                leftShot.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 90, 0);
            }

        }



        AnimatorClipInfo[] animatorInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        currentAnimation = animatorInfo[0].clip.name;

        if (Input.GetKeyDown(KeyCode.F) && currentAnimation != "Armature_Follow_Up_Mage_Punch 1")
        {
            
            if (currentAnimation != "Armature_Mage Punch")
            {
                followUpStarted = false;

                currentSpell = spells[0];
                GetComponent<Animator>().SetInteger("SpellType", spells[0].spellAnim);
                GetComponent<Animator>().SetTrigger("SpellHit");
            } else
            {
                
                followUpStarted = true;
            }
            
        }

    }

    public void checkFollowUp()
    {
        if(followUpStarted == true)
        {
            return;
        } else
        {
            GetComponent<Animator>().SetInteger("SpellType", 0);
        }
    }



    public void shootProjectile(int hand)
    {
        if(hand == 1)
        {
            rightShot.GetComponent<Projectile>().isMoving = true;
            rightShot.transform.parent = null;
            rightShot.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 90, 0);
        } else
        {
            leftShot.GetComponent<Projectile>().isMoving = true;
            leftShot.transform.parent = null;
            leftShot.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 90, 0);
        }


    }

    public void createProjectile(int hand)
    {
        if(hand == 1)
        {
            rightShot = Instantiate(currentSpell.projectile);
            rightShot.GetComponent<Projectile>().origin = gameObject;
            rightShot.transform.position = GetComponent<BaseStats>().rightHand.position;

            rightShot.GetComponent<MeshRenderer>().material.SetColor("_Color", currentSpell.color);
            rightShot.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 90, 0);

            rightShot.GetComponent<Projectile>().isMoving = false;
            rightShot.transform.SetParent(rightHand);

            rightShot.GetComponent<Projectile>().maxSize = rightShot.transform.localScale;
            rightShot.transform.localScale = rightShot.transform.localScale / 10;
        } else
        {
            leftShot = Instantiate(currentSpell.projectile);
            leftShot.GetComponent<Projectile>().origin = gameObject;
            leftShot.transform.position = GetComponent<BaseStats>().leftHand.position;

            leftShot.GetComponent<MeshRenderer>().material.SetColor("_Color", currentSpell.color);
            rightShot.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 90, 0);

            leftShot.GetComponent<Projectile>().isMoving = false;
            leftShot.transform.SetParent(leftHand);

            leftShot.GetComponent<Projectile>().maxSize = leftShot.transform.localScale;
            leftShot.transform.localScale = leftShot.transform.localScale / 10;
        }

        

    }
}
