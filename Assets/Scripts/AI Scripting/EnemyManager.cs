using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float health;
    public float initialHealth = 100f;
    AiAgent agent;

    void Start(){
        agent = GetComponent<AiAgent>();
        health = initialHealth;
    }
    public void takeDamage(float damage){
        health -= damage;
        if (health <= 0){
            Kill();
        }
    }

    private void Kill(){
        agent.stateMachine.ChangeState(AiStateId.Death);
    }
}
