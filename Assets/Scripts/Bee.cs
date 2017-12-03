using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour {

    MovementController movementController;
    public Animator wingAnimator;
    public Animator legAnimator;
    public ParticleSystem trail;

    static readonly string ANIMATION_LAND_TRIGGER = "land";
    static readonly string ANIMATION_SPEED_FLOAT = "speed";
    static readonly float ANIMATION_SPEED_MULTIPLIER = 0.02f;

    private void Start() {
        movementController = GetComponent<MovementController>();
    }

    private void Update() {
        wingAnimator.SetFloat(ANIMATION_SPEED_FLOAT, (ANIMATION_SPEED_MULTIPLIER * movementController.velocity.magnitude) + 1);

        FlipXIfNecessary();

        UpdateParticlesRotation();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        CheckFlower(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        CheckFlower(collision);
    }

    private void CheckFlower(Collider2D collision) {
        if (collision.CompareTag("Flower")) {
            Flower flower = collision.GetComponentInParent<Flower>();

            if (flower.stage.Equals(Flower.Stage.Flower) && movementController.velocity.magnitude < 10) {
                legAnimator.SetTrigger(ANIMATION_LAND_TRIGGER);

                movementController.Land(collision.gameObject);
            }
            else if (flower.stage.Equals(Flower.Stage.SeedHead) && movementController.velocity.magnitude > 20) {
                flower.DisperseSeeds();
            }
        }
    }

    public void ReachedTarget(Transform target) {
        if (target.CompareTag("Flower")) {
            Pollinate(target.gameObject);
        }
    }

    private void Pollinate(GameObject flowerObject) {
        Flower flower = flowerObject.GetComponentInParent<Flower>();
        flower.Pollinate();
    }

    private void FlipXIfNecessary() {
        float xSpeed = movementController.velocity.x;
        Vector3 scale = transform.localScale;

        if (xSpeed > 0) {
            scale.x = Mathf.Abs(transform.localScale.x);
        } else if (xSpeed < 0) {
            scale.x = -Mathf.Abs(transform.localScale.x);
        }

        transform.localScale = scale;
    }

    private void UpdateParticlesRotation() {
        var main = trail.main;
        float angle = Vector2.Angle(Vector2.right, movementController.velocity);
        Vector3 cross = Vector3.Cross(Vector2.right, movementController.velocity);

        if (cross.z > 0)
            angle = 360 - angle;

        main.startRotationZ = Mathf.Deg2Rad * angle;
    }
}
