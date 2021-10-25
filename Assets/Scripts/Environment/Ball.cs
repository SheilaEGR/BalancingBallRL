using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    public float maxPositionX = 2f;
    public Transform leftBorder;
    public Transform rightBorder;

    private Rigidbody2D rb;
    private bool offPlatform = false;

    public void Restart()
    {
        rb.position = new Vector2( Random.Range(-maxPositionX, maxPositionX), 2f);
        rb.velocity = Vector2.zero;
        offPlatform = false;
    }

    public bool Fell()
    {
        return offPlatform;
    }

    public float GetDistanceToLeft()
    {
        Vector2 distVec = transform.position - leftBorder.position;
        return distVec.magnitude;
    }

    public float GetDistanceToRight()
    {
        Vector2 distVec = rightBorder.position - transform.position;
        return distVec.magnitude;
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    // ===== UNITY METHODS
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() 
    {
        Restart();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Wall")
        {
            offPlatform = true;
        }
    }
}
