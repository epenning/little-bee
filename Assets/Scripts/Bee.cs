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
        if (collision.CompareTag("Flower") && movementController.velocity.magnitude < 10) {
            movementController.Land(collision.gameObject);
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
