using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class attack
{
    public int shape;
    public int damage;
    public Transform parentPos;
    public Vector3 relativePos;
    public float scaleX;
    public float scaleY;
    public float scaleZ;
}

public class Enemy : MonoBehaviour
{
    public static GameObject player;
    public float agroDistance;
    private bool isAgroed;
    public string walkingAnimName;
    public float speed;
    public float neededProximity;
    public LayerMask targetLayer;
    public GameObject hitEffect;
    public List<attack> attacks;
    GameObject idealRot;
    Quaternion targetRotation;

    int attackNum;
    bool isAttacking;
    List<GameObject> cantHit;
    // Start is called before the first frame update
    void Start()
    {
        idealRot = new GameObject();
        idealRot.transform.SetParent(transform);
        idealRot.transform.position = transform.position;
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        int randNum = Random.Range(1, 100);

        if(Vector3.Distance(player.transform.position, transform.position) < agroDistance)
        {
            GetComponent<Animator>().SetBool("isAgroed", true);
            isAgroed = true;
        }
        if(isAgroed == true)
        {
            GetComponent<Animator>().SetFloat("distance", Vector3.Distance(player.transform.position, transform.position));

            GetComponent<Animator>().SetFloat("neededMovement", Vector3.Distance(player.transform.position, transform.position) - neededProximity);
            GetComponent<Animator>().SetInteger("randomNumber", randNum);

            AnimatorClipInfo[] animatorInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
            string currentAnimation = "";
            if (animatorInfo[0].clip != null)
            {
                currentAnimation = animatorInfo[0].clip.name;
            }



            idealRot.transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            idealRot.transform.Rotate(0, 90, 0);



            if (currentAnimation == walkingAnimName)
            {
                GetComponent<Animator>().SetBool("isWalking", true);
                targetRotation = idealRot.transform.rotation;
                transform.GetComponent<Rigidbody>().velocity = (player.transform.position - transform.position) / Vector3.Distance(player.transform.position, transform.position);
            }
            else
            {

                GetComponent<Animator>().SetBool("isWalking", false);
            }
            if (currentAnimation == "Enemy_Null")
            {
                targetRotation = idealRot.transform.rotation;
                GetComponent<Animator>().SetBool("isWalking", true);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
        }
        
        

        if(isAttacking == true)
        {
            Hit(attackNum);
        }
    }

    public void Hit(int attackNum)
    {
        Collider[] overLap = Physics.OverlapSphere(new Vector3(0, 0, 0), 0);
        if (attacks[attackNum].shape == 0)
        {
            overLap = Physics.OverlapSphere(attacks[attackNum].parentPos.position + attacks[attackNum].relativePos, attacks[attackNum].scaleX, targetLayer);
        }


        foreach(Collider thing in overLap)
        {
            GameObject enemy = thing.gameObject;
            if (thing.GetComponent<Redirector>())
            {
                enemy = thing.GetComponent<Redirector>().Direction;
            }

            if (enemy.GetComponent<DamageTaker>() && !cantHit.Contains(enemy))
            {
                enemy.GetComponent<DamageTaker>().takeDamage(attacks[attackNum].damage);
                //print("attack");

                if(hitEffect != null)
                {
                    GameObject newEffect = Instantiate(hitEffect);
                    hitEffect.transform.position = thing.transform.position;
                    hitEffect.transform.LookAt(new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z));
                    hitEffect.transform.Rotate(180, 180, 180);
                    hitEffect.GetComponent<ParticleSystem>().Play();
                }

                cantHit.Add(enemy);
            }
        }
        
    }

    public void startAttack(int attackNum)
    {
        isAttacking = true;
        this.attackNum = attackNum;
        cantHit = new List<GameObject>();
    }

    public void endAttack()
    {
        isAttacking = false;
        cantHit.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, agroDistance);
        Gizmos.DrawWireSphere(transform.position, neededProximity);

        foreach(attack area in attacks)
        {
            if(area.shape == 0 && area.parentPos != null)
            {
                Gizmos.DrawWireSphere(area.parentPos.position, area.scaleX);
            } else if (area.shape == 1 && area.parentPos != null)
            {
                Gizmos.DrawWireCube(area.parentPos.position, new Vector3(area.scaleX, area.scaleY, area.scaleZ));
            }
        }
    }
}
