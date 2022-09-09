using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    struct PlayerCameras
    {
        public Camera main;
        public Camera overlay;
    }

    private List<PlayerCameras> _playersCameras;
    // Start is called before the first frame update
    void Start()
    {
        _playersCameras = new List<PlayerCameras>();
        int nbPlayer = 3;
        float sizeX = 1.0f;
        float sizeY = 1.0f;
        int moduloX = 1;
        int moduloY = 1;
        if (nbPlayer == 2)
        {
            sizeX /= 2;
            moduloX = 2;
        } else if (nbPlayer > 2 && nbPlayer <= 4)
        {
            sizeX /= 2;
            sizeY /= 2;
            moduloX = 2;
            moduloY = 2;
        }
        else if (nbPlayer > 4 && nbPlayer <= 6)
        {
            sizeX /= 3;
            sizeY /= 2;
            moduloX = 3;
            moduloY = 2;
        }
        else if (nbPlayer > 6 && nbPlayer <= 9)
        {
            sizeX /= 3;
            sizeY /= 3;
            moduloX = 3;
            moduloY = 3;
        }
        int j = 1;
        for (int i = 0; i < nbPlayer; i++)
        {
            GameObject player = GameObject.Instantiate(Resources.Load("Prefabs/CameraPlayer", typeof(GameObject))) as GameObject;
            PlayerCameras pc = new PlayerCameras();
            pc.main = player.transform.GetChild(0).gameObject.GetComponent<Camera>();
            pc.overlay = player.transform.GetChild(1).gameObject.GetComponent<Camera>();
            _playersCameras.Add(pc);
            
            pc.main.rect = new Rect((i % moduloX) * sizeX, (j % moduloY) * sizeY, sizeX, sizeY);
            pc.overlay.rect = new Rect((i % moduloX) * sizeX, (j % moduloY) * sizeY, sizeX, sizeY);
            if ((i+1) % moduloX == 0)
            {
                j++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}