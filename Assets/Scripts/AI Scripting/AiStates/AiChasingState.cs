using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChasingState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Chasing;
    }

    public void Enter(AiAgent agent)
    {
        agent.animator.SetFloat("Speed", 2);
    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.SetDestination(agent.waypoints[0].position);

        //change animation based on distance to finish point, if not moving, stop walk animation
        if(Vector3.Distance(agent.waypoints[0].position, agent.transform.position) <= 0.15f){
            agent.animator.SetFloat("Speed", 0);
        }
        else{
            agent.animator.SetFloat("Speed", 2);
        }

        //checks player visibility, if visible, enemy stops
        if (agent.playerInSights == true){
            agent.stateMachine.ChangeState(AiStateId.Attack);
        }
    }

    public void Exit(AiAgent agent)
    {

    }
}
