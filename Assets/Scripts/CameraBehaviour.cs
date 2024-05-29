using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    public GameObject visualizerUI;
    public float scrollScale = 0.1f;
    public float mvtSpeed = 0.0025f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (visualizerUI.activeSelf)
        {
            float scroll = Input.mouseScrollDelta.y;
            if (this.GetComponent<Camera>().orthographicSize - scroll * scrollScale > 0)
            {
                this.GetComponent<Camera>().orthographicSize -= scroll * scrollScale;
            }

            this.transform.position += new Vector3(Input.GetAxis("Horizontal") * mvtSpeed, Input.GetAxis("Vertical") * mvtSpeed, 0f);
        }
    }
}
