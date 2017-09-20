using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour {

    MovementController movementController;

    private void Start() {
        movementController = GetComponent<MovementController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        CheckFlower(collision);
    }

    private void OnTriggerStay2D(Collider2D collision) {
        CheckFlower(collision);
    }

    private void CheckFlower(Collider2D collision) {
        if (collision.CompareTag("Flower")) {
            Flower flower = collision.gameObject.GetComponent<Flower>();

            if (flower.stage.Equals(Flower.Stage.Flower) && movementController.velocity.magnitude < 10) {
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
        Flower flower = flowerObject.GetComponent<Flower>();
        flower.Pollinate();
    }
}
