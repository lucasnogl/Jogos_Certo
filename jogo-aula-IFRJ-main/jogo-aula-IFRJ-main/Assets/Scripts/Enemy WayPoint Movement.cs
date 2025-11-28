using UnityEngine;
using System.Collections.Generic;

public class EnemyWaypointMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float waypointReachedDistance = 0.1f;
    public bool loop = true;

    [Header("Visual")]
    public Transform visual; // <-- Arraste o objeto "Visual" aqui no inspector

    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private Vector2 movementDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy!");
            enabled = false;
            return;
        }

        // A direção inicial pode ser definida aqui, mas o flip será no primeiro GoToNextWaypoint
        SetTargetWaypoint(currentWaypointIndex);
    }

    void FixedUpdate()
    {
        MoveTowardsWaypoint();
        CheckIfWaypointReached();
    }

    void SetTargetWaypoint(int index)
    {
        if (waypoints.Count == 0) return;

        currentWaypointIndex = index;
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        movementDirection = (targetPosition - (Vector2)transform.position).normalized;

        // REMOVIDO: FlipVisual() daqui
    }

    void MoveTowardsWaypoint()
    {
        if (waypoints.Count == 0) return;

        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        movementDirection = (targetPosition - (Vector2)transform.position).normalized;

        rb.linearVelocity = movementDirection * moveSpeed;
        
        // REMOVIDO: FlipVisual() daqui
    }

    // A função FlipVisual permanece a mesma que definimos para "inimigo olha para a esquerda por padrão"
    void FlipVisual()
    {
        if (visual == null) return;

        // Se movendo para a direita (X positivo)
        if (movementDirection.x > 0.01f) 
        {
            // Se o padrão é olhar para a esquerda (X=+1), para olhar para a direita, use X=-1
            visual.localScale = new Vector3(-Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z);
        }
        // Se movendo para a esquerda (X negativo)
        else if (movementDirection.x < -0.01f)
        {
            // Para manter a visão para a esquerda (padrão)
            visual.localScale = new Vector3(Mathf.Abs(visual.localScale.x), visual.localScale.y, visual.localScale.z);
        }
    }

    void CheckIfWaypointReached()
    {
        if (waypoints.Count == 0) return;

        float distanceToWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);

        if (distanceToWaypoint <= waypointReachedDistance)
        {
            GoToNextWaypoint();
        }
    }

    void GoToNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Count)
        {
            if (loop)
            {
                currentWaypointIndex = 0;
            }
            else
            {
                enabled = false;
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        SetTargetWaypoint(currentWaypointIndex);
        FlipVisual(); // <-- Chamada ADICIONADA AQUI!
    }
}