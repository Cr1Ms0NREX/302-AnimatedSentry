using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Player Health
    public Transform pfHealthBar;
    public Transform healthBarPosition;

    public void Start()
    {
        // Health System
        HealthSystem healthSystem = new HealthSystem(100);

        Transform healthBarTransform = Instantiate(pfHealthBar, healthBarPosition.position, Quaternion.identity);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        //healthBar.Setup(healthSystem);

    }
    public void Update()
    {
        
        // Health System
        HealthSystem healthSystem = new HealthSystem(100);
        Transform healthBarTransform = Instantiate(pfHealthBar, healthBarPosition.position, Quaternion.identity);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        //healthBar.Setup(healthSystem);
        Destroy(gameObject);

    }
}
