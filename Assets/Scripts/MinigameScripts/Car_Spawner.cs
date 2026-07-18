using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    public GameObject[] car;
    private int LockInCounter = 0;
    private float RandomXPos;
    private int RandomInt;
    private int LastRandomInt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        StartCoroutine(SpawnCars());
    }

    // Update is called once per frame
    void Update() {
        
    }

    void Cars() {
        int RandomSkin = Random.Range(0, car.Length);
        //LastRandomInt = RandomInt;
        //RandomInt = Random.Range(0, 3);

        RollRandomPos();
        //float RandomXPos = Random.Range(-3.2f, 3.2f);
        
        if (LockInCounter == 5 || LockInCounter == 10 || LockInCounter == 15 || LockInCounter == 20 || LockInCounter == 25 || LockInCounter == 30 || LockInCounter == 35 || LockInCounter == 40) {
            Instantiate(car[RandomSkin], new Vector3(RandomXPos, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            RollRandomPos();
            Instantiate(car[RandomSkin], new Vector3(RandomXPos, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
        } else if (LockInCounter == 6 || LockInCounter == 38) {
            Instantiate(car[RandomSkin], new Vector3(RandomXPos, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            RollRandomPos();
            Instantiate(car[RandomSkin], new Vector3(RandomXPos, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            RollRandomPos();
            Instantiate(car[RandomSkin], new Vector3(RandomXPos, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
            RollRandomPos();

        } else {
            Instantiate(car[RandomSkin], new Vector3(RandomXPos, transform.position.y, transform.position.z), Quaternion.Euler(0, 0, 0));
        }

        if (LockInCounter > 40) {
            LockInCounter = 0;
        }
        else {
            LockInCounter++;
        }
    }

    void RollRandomPos() {
        LastRandomInt = RandomInt;
        RandomInt = Random.Range(0, 4);

        for (int i = 0; i < 5; i++) {
            if (RandomInt == LastRandomInt) {
                RandomInt = Random.Range(0, 4);
                print("Roll Again.");
            } else {
                print("New Number");
                break;
            }
        }
        switch (RandomInt) {
            case 0:
                RandomXPos = -2.45f;
                break;
            case 1:
                RandomXPos = -0.77f;
                break;
            case 2:
                RandomXPos = 0.85f;
                break;
            case 3:
                RandomXPos = 2.51f;
                break;
        }
    }

    IEnumerator SpawnCars() {
        while (true) {
            if (LockInCounter > 20) {
                yield return new WaitForSeconds(0.8f);
                Cars();
            } else {
                yield return new WaitForSeconds(1);
                Cars();
            }

        }
    }
}
