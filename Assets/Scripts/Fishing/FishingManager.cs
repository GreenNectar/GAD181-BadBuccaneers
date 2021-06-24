using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fish;
    [SerializeField]
    private GameObject shark;
    [SerializeField]
    private float spawnTime = 4f;
    [SerializeField, Range(0f, 1f)]
    private float sharkChance = 0.5f;

    private List<FishingController> controllers = new List<FishingController>();

    // Start is called before the first frame update
    void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        // Spawn a fish periodically
        InvokeRepeating("SpawnFish", 2f, spawnTime);
    }

    public void StopSpawning()
    {
        CancelInvoke();
    }

    public void Add(FishingController fishingController)
    {
        // Add the fishing controller to this manager
        controllers.Add(fishingController);
    }

    /// <summary>
    /// Spawns a fish at a random height and direction
    /// </summary>
    private void SpawnFish()
    {
        // Makes sure all the fish are spawned in the same side and depth on all players
        bool isShark = Random.Range(0f, 1f) <= sharkChance;
        bool isLeft = Random.Range(0f, 1f) > 0.5f;
        float depth = Random.Range(0f, 1f);

        foreach (var controller in controllers)
        {
            var f = Instantiate(isShark ? shark : fish, controller.transform);
            f.SetActive(true);

            if (isLeft)
            {
                f.transform.localPosition = Vector3.left * 10f;
                f.transform.localRotation = Quaternion.Euler(0, 90f, 0);
            }
            else
            {
                f.transform.localPosition = Vector3.right * 10f;
                f.transform.localRotation = Quaternion.Euler(0, 90f + 180f, 0);
            }

            Vector3 temp = f.transform.position;
            temp.y = depth.Remap(0f, 1f, controller.startingHeight - controller.fishSpawnDepth, controller.startingHeight - controller.depth);
            f.transform.position = temp;
        }
    }

    public void ClearAllBuckets()
    {
        foreach (var controller in controllers)
        {
            controller.ClearFish();
        }
    }
}
