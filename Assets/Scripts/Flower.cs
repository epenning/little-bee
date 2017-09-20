using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    static readonly string ANIMATION_POLLINATE_TRIGGER = "pollinate";
    static readonly string ANIMATION_SEED_TRIGGER = "seed";
    static readonly string ANIMATION_DISPERSE_TRIGGER = "disperse";

    public enum Stage { Sprout, Flower, Pollinated, SeedHead, Dead };
    public static readonly float sproutTime = 10;
    public static readonly float flowerTime = 1000;
    public static readonly float pollinatedTime = 10;
    public static readonly float seedHeadTime = 1000;

    private Animator animator;

    public Stage stage;
    public float stageTimer;

    private void Start() { 
        animator = GetComponent<Animator>();

        SetStage(stage);
    }

    private void FixedUpdate() {
        stageTimer -= Time.deltaTime;

        if (stageTimer <= 0) {
            if (stage == Stage.Sprout) {
                Bloom();
            }
            if (stage == Stage.Pollinated) {
                MakeSeeds();
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

    public void Animate(string parameter) {
        animator.SetTrigger(parameter);
    }

    public void Bloom() {
        SetStage(Stage.Flower);

        // update sprite
    }

    public void Pollinate() {
        if (stage == Stage.Flower) {
            SetStage(Stage.Pollinated);

            Animate(ANIMATION_POLLINATE_TRIGGER);
        }
    }

    public void MakeSeeds() {
        SetStage(Stage.SeedHead);

        Animate(ANIMATION_SEED_TRIGGER);
    }

    public void DisperseSeeds() {
        Animate(ANIMATION_DISPERSE_TRIGGER);

        // spread seeds

        SetStage(Stage.Dead);
    }

    public void Die() {
        if (stage == Stage.SeedHead) {
            DisperseSeeds();
        }

        Destroy(gameObject);
    }
}
