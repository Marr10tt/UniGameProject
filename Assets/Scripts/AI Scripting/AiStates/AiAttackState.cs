
using System.Collections;
using UnityEngine;

public class AiAttackState : AiState
{
    private float fireRate = 5;
    private float timeToFire = 0f;
    private float damageDealt = 5;
    private float ammoInGun = 15;
    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Enter(AiAgent agent)
    {
        //sets the target to be the player
        agent.navMeshAgent.stoppingDistance = 10;
        agent.navMeshAgent.speed = 6;
        agent.navMeshAgent.SetDestination(agent.gameObject.transform.position);
        agent.animator.SetFloat("Speed", 0);
        agent.animator.SetBool("IsAiming", true);

        //alerts all other agents in the group to attack
        foreach (AiAgent currentAgent in agent.aiAgents){
            if(currentAgent.stateMachine.currentState !=  AiStateId.Attack){
                currentAgent.stateMachine.ChangeState(AiStateId.Attack);
            }
        }
    }

    public void Update(AiAgent agent)
    {  
        //change animation based on agent speed, if not moving, stop walk animation
        if(agent.navMeshAgent.velocity.magnitude <= 0.15f){
            agent.animator.SetFloat("Speed", 0);
        }
        else{
            agent.animator.SetFloat("Speed", 4);
        }

        //monitors hits and ai acts accordingly
        agent.head.transform.LookAt(new Vector3(agent.playerRef.transform.position.x, agent.transform.position.y, agent.playerRef.transform.position.z ));
            RaycastHit hit; //hit provides information about what the raycast comes into contact with
            //checks if hit the correct thing + has ammo in gun + is in engagement range
            if (Physics.Raycast(agent.lineOrigin.transform.position, agent.transform.forward, out hit, Mathf.Infinity) && ammoInGun>0 && agent.navMeshAgent.remainingDistance < 20){
                if(hit.transform.tag == "Player"){
                    //shoots when ready and carries on moving to player
                    if(Time.time>=timeToFire){
                        timeToFire = Time.time + 1f/fireRate;
                        agent.playerRef.GetComponent<PlayerController>().takeDamage(damageDealt);
                        agent.muzzleFlash.Play();
                        agent.gunSounds.Play();
                        ammoInGun--;
                        agent.navMeshAgent.SetDestination(agent.playerRef.transform.position);
                    }
                }
            }
            //if out of ammo or out of engagement range, move closer or stay where they are
            else if(agent.navMeshAgent.remainingDistance>=16){
                agent.navMeshAgent.destination = agent.playerRef.transform.position;
            }
            else{
                agent.navMeshAgent.destination = agent.transform.position;
            }
    }

    public void Exit(AiAgent agent)
    {
        timeToFire = 0f;
        agent.animator.SetBool("IsAiming", false);
    }
}
