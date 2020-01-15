using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViridaxGameStudios.Controllers
{
    public class AIController : MonoBehaviour
    {
        #region Variables
        [Tooltip("All the Tags that the character will consider as an enemy. NOTE: The default reaction is to attack.")]
        public List<string> enemyTags = new List<string>();
        [Tooltip("The health of the character. WARNING: A value of 0 or below will kill the character instantly!")]
        public float m_HitPoints = 10f;
        [Tooltip("The maximum mana the charcter has to cast spells and special abilities.")]
        public float m_Mana = 0f;
        [Tooltip("The movement speed of the character")]
        public float m_MoveSpeed = 5f;
        [Tooltip("The radius which the character can detect other objects")]
        public float m_DetectionRadius = 20f;
        [Tooltip("The maximum range that the character can be in order to attack a target.")]
        public float m_AttackRange = 7f;
        [Tooltip("The Icon to display when the character has detected something (usually an enemy/the player)")]
        public GameObject AlertedIcon;

        Animator anim;
        private GameObject Target;
        private bool isStunned = false;
        private bool enemyFound = false;
        #endregion


        #region Main Methods
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

            if (!isStunned)
            {
                ScanForObjects(gameObject.transform.position, m_DetectionRadius);
                if (Target != null)
                {
                    FollowTarget();
                }
            }

        }
        #endregion

        #region Helper Methods
        public void ReceiveDamage(float damage)
        {
            //
            //Method Name : void ReceiveDamage(float damage)
            //Purpose     : This method receives damage from various sources and applies it to the character.
            //Re-use      : none
            //Input       : float damage
            //Output      : none
            //
            isStunned = true;
            m_HitPoints -= damage;
            if (m_HitPoints <= 0)
            {
                anim.SetBool("isDead", true);
                //CharacterDead() method should be called after the death animation has finished playing using an Animation Event. 
                //Alternatively, you can implement your own logic here to suit your needs.
            }
            else
            {
                anim.SetTrigger("isDamaged");
                //The ResumeFromDamage method is called by the damaged animation to continue normal functionality like object detection and following.
            }
        }
        public void ResumeFromDamage()
        {
            //
            //Method Name : void ResumeFromDamage()
            //Purpose     : This method allows the character to resume its normal functionality after the damage animation has played.
            //Re-use      : none
            //Input       : none
            //Output      : none
            //
            isStunned = false;
        }
        public void CharacterDead()
        {
            //
            //Method Name : void CharacterDead()
            //Purpose     : This method destroys the character GameObject as soon as the death animation has finished playing.
            //Re-use      : none
            //Input       : none
            //Output      : none
            //
            Destroy(gameObject);
        }

        protected void FollowTarget()
        {
            //
            //Method Name : void FollowTarget()
            //Purpose     : This method moves the character to where the target position is. In most casess, the player position.
            //Re-use      : none
            //Input       : none
            //Output      : none
            //
            if (Target != null)
            {
                float distance = Vector3.Distance(transform.position, Target.transform.position);
                if (distance <= m_AttackRange)
                {
                    anim.SetBool("isRunning", false);
                    anim.SetTrigger("attack");
                    transform.LookAt(Target.transform);
                }
                else
                {
                    transform.position += transform.forward * m_MoveSpeed * Time.deltaTime;
                    anim.SetBool("isRunning", true);
                }

            }
        }
        private void ScanForObjects(Vector3 center, float radius)
        {
            //
            //Method Name : void ScanForObjects(Vector3 center, float radius) 
            //Purpose     : This method uses the Physics.OverlapSphere method to scan for objects within a given radius.
            //Re-use      : none
            //Input       : Vector3 center, float radius
            //Output      : none
            //
            Collider[] hitColliders = Physics.OverlapSphere(center, radius);
            int i = 0;
            enemyFound = false;
            while (i < hitColliders.Length)
            {
                //Filter out all GameObjects to get only those that are enemies.
                if (enemyTags.Contains(hitColliders[i].tag))
                {
                    Target = hitColliders[i].gameObject;
                    transform.LookAt(Target.transform);
                    enemyFound = true;
                    //Display the alerted Icon.
                    AlertedIcon.SetActive(true);
                }
                i++;
            }
            if (!enemyFound)
            {
                Target = null;
                anim.SetBool("isRunning", false);
                //Hide the alertedIcon when an enemy has been lost.
                AlertedIcon.SetActive(false);
            }
        }
        #endregion
    }
}

