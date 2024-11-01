using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [Header("Gold Drop")]
    public int goldAmount;

    [Header("Consumable Drops")]
    public GameObject[] consumables; //array to hold various consumables
    
    [Range(0, 1)] public float dropChance = 0.10f; //base drop chance set to 10%

    public void DropLoot() 
    {
        DropGold();
        DropConsumables();
    }

    private void DropGold() 
    {
        //replace with gold drop logic to add to player gold count
        Debug.Log($"{this.gameObject.name} dropped {goldAmount} gold.");
    }

    private void DropConsumables() 
    {
        foreach (var consumable in consumables)
        {
            if (Random.value <= dropChance)  // Check against the drop chance
            {
                //Instantiate(consumable, transform.position, Quaternion.identity);  // Drop consumable at enemy position
                Debug.Log($"{this.gameObject.name} dropped {consumable.name}");
            }
        }
    }
}
