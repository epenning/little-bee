using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    public enum Stage { Sprout, Flower, Pollinated, SeedHead };
    public static readonly float sproutTime = 10;
    public static readonly float flowerTime = 1000;
    public static readonly float pollinatedTime = 30;
    public static readonly float seedHeadTime = 1000;

    public Stage stage;
    public float stageTimer;

    private void Start() {
        SetStage(stage);
    }

    private void FixedUpdate() {
        stageTimer -= Time.deltaTime;

        if (stageTimer <= 0) {
            if (stage == Stage.Sprout) {
                Bloom();
            }
            if (stage == Stage.Pollinated) {
                SetStage(Stage.SeedHead);
            }
            else {
                Die();
            }
        }
    }

    public void SetStage(Stage stage) {
        this.stage = stage;

        switch (stage) {
            case Stage.Sprout:
                stageTimer = sproutTime;
                break;
            case Stage.Flower:
                stageTimer = flowerTime;
                break;
            case Stage.Pollinated:
                stageTimer = pollinatedTime;
                break;
            case Stage.SeedHead:
                stageTimer = seedHeadTime;
                break;
        }
    }

    public void Bloom() {
        SetStage(Stage.Flower);

        // update sprite
    }

    public void Pollinate() {
        if (stage == Stage.Flower) {
            SetStage(Stage.Pollinated);

            // update sprite
        }
    }

    public void LosePollen() {
        // update sprite
    }

    public void MakeSeeds() {
        SetStage(Stage.SeedHead);

        // update sprite
        // spawn seeds (attached to plant)
    }

    public void DisperseSeeds() {
        // spread seeds
    }

    public void Die() {
        if (stage == Stage.SeedHead) {
            DisperseSeeds();
        }

        Destroy(gameObject);
    }
}
