using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{   
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public bool playerInSights = false;
    public List<Transform> waypoints;
    public Animator animator;
    public GameObject playerRef;
    public GameObject head;
    public Ray ray;
    public GameObject lineOrigin;
    public Rigidbody rb;
    public ParticleSystem muzzleFlash;
    public AudioSource gunSounds;
    public List<AiAgent> aiAgents;

    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerRef = GameObject.Find("Test Player");
        rb = GetComponent<Rigidbody>();

        //registers new state machine and all used states
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiChasingState());
        stateMachine.RegisterState(new AiAttackState());
        stateMachine.ChangeState(initialState); //sets initial state
    }

    void Update(){
        stateMachine.Update();
    }
}
