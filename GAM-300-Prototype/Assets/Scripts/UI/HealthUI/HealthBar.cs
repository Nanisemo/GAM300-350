using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;

    // public PlayerController playerController;

    List<Hearts> hearts = new List<Hearts>();

    public int
        maxHealth,
        currentHealth;

    private void Update()
    {
        // TODO: Link the playercontroller.currrenthealth and maxhealth to here
        // All values must x2 its value due to how the enum for the generating works

        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        // determine how many hearts to make total
        // based off the max health
        float maxHealthRemainder = maxHealth % 2;
        int heartsToMake = (int)((maxHealth / 2) + maxHealthRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart(); // make max health container
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainer = (int)Mathf.Clamp(currentHealth - (i * 2), 0, 2);
            hearts[i].SetHeartImage((Hearts.HeartStatus)heartStatusRemainer);
        }
    }

    public void CreateEmptyHeart() 
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        Hearts heartComponent = newHeart.GetComponent<Hearts>();
        heartComponent.SetHeartImage(Hearts.HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
            Destroy(t.gameObject);

        hearts = new List<Hearts>();
    }
}
