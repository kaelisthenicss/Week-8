using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float speed = 0.5f;
    public Transform parent;     

    public float spawnZ = 300f;
    public float endZ = 0f;

    private float[] lanes = new float[] { -2.5f, 0f, 2.5f };

    private Item currentItem;
    public List<Item> items = new List<Item>();

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, 1f);
    }

    private void SpawnObstacle()
    {
        if (currentItem == null)
        {
            int laneIndex = Random.Range(0, lanes.Length);
            float laneX = lanes[laneIndex];

            currentItem = Instantiate(itemToSpawn, parent);
            currentItem.itemPosition = new Vector3(laneX, -2f, spawnZ);

            items.Add(currentItem);
        }
    }

    private void Update()
    {
        if (currentItem != null)
        {
            currentItem.itemPosition.z -= speed;

            if (currentItem.itemPosition.z <= endZ)
            {
                Destroy(currentItem.gameObject);
                items.Remove(currentItem);
                currentItem = null;
            }
        }
    }
}
