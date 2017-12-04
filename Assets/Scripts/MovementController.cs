using UnityEngine;

public class MovementController : MonoBehaviour {

    public GameManager gameManager;

    public float maxSpeed = 5f;
    public float acceleration = 2f;
    public float deceleration = 5f;

    public Vector2 movement = Vector2.zero;
    public Vector2 velocity = Vector2.zero;
    public Vector2 input = Vector2.zero;

    public Transform target = null;
    public float landingSmoothing = 1;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate() {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Move();
    }

    public void Land(GameObject flower) {
        target = flower.transform;
    }

    private void Move() {
        if (target) {
            MoveToTarget();
        }
        else {
            MoveByInput();
        }

        PreventOutOfBounds();
    }

    private void MoveToTarget() {
        float relativeTargetX = target.position.x - transform.position.x;
        float relativeTargetY = target.position.y - transform.position.y;

        if (Mathf.Abs(relativeTargetX) < 0.1 && Mathf.Abs(relativeTargetY) < 0.1) {
            ReachedTarget();
        }

        movement.x = Mathf.Lerp(0, relativeTargetX, landingSmoothing * Time.deltaTime);
        movement.y = Mathf.Lerp(0, relativeTargetY, landingSmoothing * Time.deltaTime);

        gameObject.transform.Translate(movement.x, movement.y, 0);
    }

    private void ReachedTarget() {
        Bee bee = GetComponent<Bee>();
        if (bee) {
            bee.ReachedTarget(target);
        }

        target = null;
    }

    private void MoveByInput() {
        velocity.x = UpdateVelocity(input.x, velocity.x);
        velocity.y = UpdateVelocity(input.y, velocity.y);

        if (Mathf.Abs(velocity.magnitude) > maxSpeed) {
            velocity *= maxSpeed / velocity.magnitude;
        }

        movement = velocity * Time.deltaTime;
        gameObject.transform.Translate(movement.x, movement.y, 0);
    }

    private float UpdateVelocity(float input, float velocity) {
        if (input < 0) {
            if (velocity > 0) {
                velocity -= deceleration * Time.deltaTime;
            }
            velocity -= acceleration * Time.deltaTime;
        }
        else if (input > 0) {
            if (velocity < 0) {
                velocity += deceleration * Time.deltaTime;
            }
            velocity += acceleration * Time.deltaTime;
        }
        else {
            if (velocity > 0) {
                velocity = Mathf.Max(velocity - deceleration * Time.deltaTime, 0);
            }
            else if (velocity < 0) {
                velocity = Mathf.Min(velocity + deceleration * Time.deltaTime, 0);
            }
        }

        return velocity;
    }

    public void PreventOutOfBounds() {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, gameManager.minXAndY.x, gameManager.maxXAndY.x);
        position.y = Mathf.Clamp(position.y, gameManager.minXAndY.y, gameManager.maxXAndY.y);

        transform.position = position;
    }
}
