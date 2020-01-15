using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI1 : MonoBehaviour
{
    public int health = 5;
    Transform localP;
    private NavMeshAgent agent;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if(HordeMode.main != null)
            localP = HordeMode.main.loadedPlayer.transform;

        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(move());
        //animator.Play("Unarmed-WalkRun-Blend");
    }

    // Update is called once per frame
    IEnumerator move()
    {
        while (true)
        {
            previousTarget = agent.destination;
            agent.destination = localP.position;
            if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                agent.destination = previousTarget;
                yield return new WaitForSecondsRealtime(0.5f);
            }
            yield return new WaitForSecondsRealtime(0.25f);
            //animator.SetFloat("Velocity Z", agent.speed);
        }
    }

    void TakeDamage(int i)
    {
        health -= i;
        //animator.Play("Unarmed-GetHit-F1");
        HitMarker.Hit();
        if (health <= 0)
        {
            HordeMode.main.RemoveZombie(this.gameObject);
            Destroy(this.gameObject);
        }
        //animator.Play("Unarmed-WalkRun-Blend");
    }

    Vector3 previousTarget;
    private float timeCoolDown;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            timeCoolDown = Time.time;
           // animator.Play("Unarmed-Attack-L1");
            collision.gameObject.GetComponent<Health>().health -= 5;
            //animator.Play("Unarmed-WalkRun-Blend");
        }
        if(collision.gameObject.name.Contains("ullet"))
        {
            health -= 1;
            HitMarker.Hit();
            if (health <= 0)
            {
                HordeMode.main.RemoveZombie(this.gameObject);
                Destroy(this.gameObject);
            }
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Time.time > timeCoolDown + 0.5f)
            {
                timeCoolDown = Time.time;
                collision.gameObject.GetComponent<Health>().health -= 5;
            }
        }

    }
}
