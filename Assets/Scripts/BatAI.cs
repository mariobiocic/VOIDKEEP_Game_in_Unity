using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float moveSpeed = 3f;
    public float waypointRadius = 0.3f;
    public float recalculatePath = 0.5f;

    [Header("Grid Settings")]
    public LayerMask obstacleLayer;
    public float nodeSize = 1f;

    [Header("Combat")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1.2f;
    public int attackDamage = 1;

    private Animator anim;
    private Rigidbody2D rb;

    private enum State { Fly, Attack, Dead }
    private State currentState = State.Fly;

    private List<Vector2> path = new List<Vector2>();
    private int currentWaypoint = 0;
    private float attackTimer = 0f;

    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        anim.SetBool("fly", true);

        StartCoroutine(UpdatePathLoop());
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        float distToPlayer = Vector2.Distance(rb.position, target.position);

        // --- ATTACK ---
        if (distToPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                SetState(State.Attack);
                attackTimer = attackCooldown;

                if (target != null)
                    target.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
            }
            return;
        }

        if (currentState == State.Attack)
            SetState(State.Fly);

        if (path.Count > 0 && currentWaypoint < path.Count)
            MoveAlongPath();
        else
            rb.linearVelocity = Vector2.zero;
    }

    void MoveAlongPath()
    {
        Vector2 wp = path[currentWaypoint];
        Vector2 dir = (wp - rb.position).normalized;

        rb.MovePosition(Vector2.MoveTowards(rb.position, wp, moveSpeed * Time.deltaTime));

        if (Mathf.Abs(dir.x) > 0.01f)
            SetFlip(dir.x < 0);

        if (Vector2.Distance(rb.position, wp) < waypointRadius)
            currentWaypoint++;
    }

    IEnumerator UpdatePathLoop()
    {
        while (true)
        {
            if (currentState != State.Dead && target != null)
            {
                List<Vector2> newPath = FindPath(rb.position, target.position);
                if (newPath != null && newPath.Count > 0)
                {
                    path = newPath;
                    currentWaypoint = 0;
                }
            }
            yield return new WaitForSeconds(recalculatePath);
        }
    }

   

    List<Vector2> FindPath(Vector2 start, Vector2 end)
    {
        Node startNode = new Node(SnapToGrid(start));
        Node endNode = new Node(SnapToGrid(end));

        List<Node> open = new List<Node>();
        HashSet<Vector2> closed = new HashSet<Vector2>();

        open.Add(startNode);

        int maxIterations = 200;
        int iter = 0;

        while (open.Count > 0 && iter < maxIterations)
        {
            iter++;

            Node current = open[0];
            for (int i = 1; i < open.Count; i++)
                if (open[i].F < current.F) current = open[i];

            open.Remove(current);
            closed.Add(current.position);

            if (Vector2.Distance(current.position, endNode.position) < nodeSize)
                return RetracePath(current);

            foreach (Vector2 dir in GetNeighborDirs())
            {
                Vector2 neighborPos = current.position + dir * nodeSize;

                if (closed.Contains(neighborPos)) continue;
                if (IsObstacle(neighborPos)) continue;

                float newG = current.g + nodeSize;
                Node existing = open.Find(n => n.position == neighborPos);

                if (existing == null)
                {
                    Node neighbor = new Node(neighborPos);
                    neighbor.g = newG;
                    neighbor.h = Vector2.Distance(neighborPos, endNode.position);
                    neighbor.parent = current;
                    open.Add(neighbor);
                }
                else if (newG < existing.g)
                {
                    existing.g = newG;
                    existing.parent = current;
                }
            }
        }

        return null;
    }

    List<Vector2> RetracePath(Node endNode)
    {
        List<Vector2> result = new List<Vector2>();
        Node current = endNode;
        while (current != null)
        {
            result.Add(current.position);
            current = current.parent;
        }
        result.Reverse();
        return result;
    }

    Vector2 SnapToGrid(Vector2 pos)
    {
        return new Vector2(
            Mathf.Round(pos.x / nodeSize) * nodeSize,
            Mathf.Round(pos.y / nodeSize) * nodeSize
        );
    }

    bool IsObstacle(Vector2 pos)
    {
        return Physics2D.OverlapCircle(pos, nodeSize * 0.4f, obstacleLayer);
    }

    Vector2[] GetNeighborDirs()
    {
        return new Vector2[]
        {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };
    }

   

    void SetState(State newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        anim.SetBool("isAttacking", false);

        switch (newState)
        {
            case State.Attack:
                anim.SetBool("isAttacking", true);
                break;
            case State.Dead:
                anim.SetBool("fly", false);
                anim.SetTrigger("die");
                break;
        }
    }

    public void Die()
    {
        if (currentState == State.Dead) return;
        SetState(State.Dead);
        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;
    }

    void OnDrawGizmos()
    {
        if (path == null) return;
        Gizmos.color = Color.cyan;
        for (int i = currentWaypoint; i < path.Count - 1; i++)
            Gizmos.DrawLine(path[i], path[i + 1]);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void SetFlip(bool flip)
    {
        Vector3 scale = spriteRenderer.transform.localScale;
        float baseX = Mathf.Abs(originalScale.x);
        scale.x = flip ? baseX : -baseX;
        scale.y = originalScale.y;
        scale.z = originalScale.z;

        spriteRenderer.transform.localScale = scale;
    }
}

public class Node
{
    public Vector2 position;
    public float g;
    public float h;
    public float F => g + h;
    public Node parent;

    public Node(Vector2 pos)
    {
        position = pos;
    }
}