using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandedWeaponScript : MonoBehaviour
{
    public bool rightHandHasWeapon;
    public bool leftHandHasWeapon;
    public bool rightHandHasShield;
    public bool leftHandHasShield;

    public string currentAnimation;
    public string previousAnimation;

    public Transform leftHand;
    public Transform rightHand;


    private Transform handSwinging;

    private bool isSwinging;
    private bool canSwing;

    public List<GameObject> hitThings;

    // Start is called before the first frame update
    void Start()
    {
        canSwing = true;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] animatorInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        currentAnimation = animatorInfo[0].clip.name;

        if((leftHandHasWeapon && rightHandHasWeapon) || rightHandHasWeapon)
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

        if (Input.GetMouseButton(0) && (currentAnimation == "Armature_Walking _Top" || currentAnimation == "Armature_Idle_Top") && canSwing)
        {
            canSwing = false;
            GetComponent<Animator>().SetTrigger("Hit");
        } else if (Input.GetMouseButton(0) && currentAnimation == "Armature_One Handed Swing Right")
        {
            GetComponent<Animator>().SetBool("Still Hitting", true);
        }

        if(currentAnimation == "Armature_One Handed Swing Left")
        {
            GetComponent<Animator>().SetBool("Still Hitting", false);
        }

        
        

        previousAnimation = currentAnimation;
    }


    public void StartSwinging(int hand)
    {
        if(hand == 1)
        {
            handSwinging = rightHand;
        } else
        {
            handSwinging = leftHand;
        }

        isSwinging = true;
    }

    public void EndSwinging()
    {
        isSwinging = false;
        hitThings.Clear();
    }
}
