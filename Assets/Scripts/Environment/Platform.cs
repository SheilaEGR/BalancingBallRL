using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Platform : MonoBehaviour
{
    public float maxInitialAngle = 10;
    public float maxAngularSpeed = 10;

    private Rigidbody2D rb;
    private float angularSpeed = 0;

    public void Restart()
    {
        rb.rotation = Random.Range(-maxInitialAngle, maxInitialAngle);
    }

    public void SetAngularSpeed(float angularSpeed)
    {
        this.angularSpeed = angularSpeed;
        //this.angularSpeed = this.angularSpeed < -maxAngularSpeed ? -maxAngularSpeed : this.angularSpeed;
        //this.angularSpeed = this.angularSpeed > maxAngularSpeed ? maxAngularSpeed : this.angularSpeed;
    }

    public float GetRotation()
    {
        return rb.rotation;
    }

    public float GetAngularSpeed()
    {
        return angularSpeed;
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

    private void FixedUpdate() 
    {
        rb.MoveRotation(rb.rotation + angularSpeed*Time.fixedDeltaTime);
    }
}
