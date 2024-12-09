
using System.Collections;
using UnityEngine;

public class AiAttackState : AiState
{
    private float fireRate = 5;
    private float timeToFire = 0f;
    private float damageDealt = 10;
    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.SetDestination(agent.gameObject.transform.position);
        agent.animator.SetFloat("Speed", 0);
        agent.animator.SetBool("IsAiming", true);

        foreach (AiAgent currentAgent in agent.aiAgents){
            if(currentAgent.stateMachine.currentState !=  AiStateId.Attack){
                currentAgent.stateMachine.ChangeState(AiStateId.Attack);
            }
        }
        //sets other grouped agents to attack
    }

    public void Update(AiAgent agent)
    {  
        Debug.Log("enemy is attacking");
        agent.head.transform.LookAt(new Vector3(agent.playerRef.transform.position.x, agent.transform.position.y, agent.playerRef.transform.position.z ));
            RaycastHit hit; //hit provides information about what the raycast comes into contact with
            if (Physics.Raycast(agent.lineOrigin.transform.position, agent.transform.forward, out hit, Mathf.Infinity)){
                if(hit.transform.tag == "Player"){
                    //shoots when ready
                    if(Time.time>=timeToFire){
                        timeToFire = Time.time + 1f/fireRate;
                        agent.playerRef.GetComponent<PlayerController>().takeDamage(damageDealt);
                        agent.muzzleFlash.Play();
                        agent.gunSounds.Play();
                    }
                }
            }
    }

    public void Exit(AiAgent agent)
    {
        timeToFire = 0f;
        agent.animator.SetBool("IsAiming", false);
    }
}
