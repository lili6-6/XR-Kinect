using UnityEngine;

public class CharacterPhysicsSetup : MonoBehaviour
{
    private bool ignoreInitialized = false;

   
    void Start()
    {
        if (ignoreInitialized) return;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[j]);
            }
        }
        Debug.Log("CharacterPhysicsSetup: Ignored collisions between " + colliders.Length + " colliders.");

        ignoreInitialized = true;
    }

}
