using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour {

    public GameObject sproutPrefab;
    public string sproutPrefabPath = "Prefabs/Sprout";
    public Transform sproutParent;

    static readonly string ANIMATION_BLOOM_TRIGGER = "bloom";
    static readonly string ANIMATION_POLLINATE_TRIGGER = "pollinate";
    static readonly string ANIMATION_SEED_TRIGGER = "seed";
    static readonly string ANIMATION_DISPERSE_TRIGGER = "disperse";

    static readonly string PETALS_GAMEOBJECT = "Petals";
    static readonly string SEEDHEAD_GAMEOBJECT = "flower1_seed_head";

    public enum Stage { Sprout, Flower, Pollinated, SeedHead, Dead };
    public static readonly float sproutTime = 10;
    public static readonly float flowerTime = 1000;
    public static readonly float pollinatedTime = 10;
    public static readonly float seedHeadTime = 1000;
    public static readonly int numberOfNewSprouts = 4;
    public static readonly float newSproutRadius = 60;

    private Animator animator;

    public Stage stage;
    public float stageTimer;

    private void Start() {
        sproutPrefab = Resources.Load(sproutPrefabPath) as GameObject;
        animator = GetComponent<Animator>();
        if (!sproutParent) {
            sproutParent = GameObject.FindGameObjectWithTag("Flower Parent").transform;
        }

        if (stage.Equals(Stage.Flower)) {
            Bloom();
        }
        else {
            SetStage(stage);
        }
    }

    private void FixedUpdate() {
        stageTimer -= Time.deltaTime;

        if (stageTimer <= 0) {
            if (stage == Stage.Sprout) {
                Bloom();
            }
            else if (stage == Stage.Pollinated) {
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

        // rotate flower upright, except petals and seedhead
        Quaternion rotation = transform.localRotation;

        foreach (Transform child in transform) {
            if (child.name.Equals(PETALS_GAMEOBJECT) ||
                child.name.Equals(SEEDHEAD_GAMEOBJECT)) {
                child.localRotation = rotation;
            }
        }

        transform.localRotation = Quaternion.identity;

        Animate(ANIMATION_BLOOM_TRIGGER);
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
        
        for (int i=0; i<numberOfNewSprouts; i++) {
            SpawnSeed();
        }

        SetStage(Stage.Dead);
    }

    public void SpawnSeed() {
        float angle = Random.Range(0, 360);
        float distance = Random.Range(0, newSproutRadius);

        float relativeX = distance * Mathf.Cos(angle);
        float relativeY = distance * Mathf.Sin(angle);

        float rotationAngle = Random.Range(0, 360);

        Vector3 position = new Vector3(transform.position.x + relativeX, transform.position.y + relativeY, transform.position.z);
        Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        Instantiate(sproutPrefab, position, rotation, sproutParent);
    }

    public void Die() {
        if (stage == Stage.SeedHead) {
            DisperseSeeds();
        }

        Destroy(gameObject);
    }
}
