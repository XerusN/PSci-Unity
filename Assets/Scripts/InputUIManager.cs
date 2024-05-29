using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputUIManager : MonoBehaviour
{

    //public GameObject simulationManager;
    public CustomInput input;

    public GameObject customMenu;

    public Boolean isSelected = false;

    public GameObject meshX;
    public GameObject meshY;
    public GameObject lengthX;
    public GameObject lengthY;
    public GameObject finalTime;
    public GameObject frame;
    public GameObject reynolds;
    public GameObject density;
    public GameObject cfl;
    public GameObject fo;
    public GameObject characteristicLength;
    public GameObject characteristicSpeed;
    public GameObject uLeft;
    public GameObject uRight;
    public GameObject uDown;
    public GameObject uUp;
    public GameObject vLeft;
    public GameObject vRight;
    public GameObject vDown;
    public GameObject vUp;
    public GameObject uIni;
    public GameObject vIni;
    public GameObject poly11;
    public GameObject poly12;
    public GameObject poly13;
    public GameObject poly14;
    public GameObject poly15;
    public GameObject poly21;
    public GameObject poly22;
    public GameObject poly23;
    public GameObject poly24;
    public GameObject poly25;
    public GameObject polyRefX;
    public GameObject polyRefY;
    public GameObject square11;
    public GameObject square12;
    public GameObject square13;
    public GameObject square21;
    public GameObject square22;
    public GameObject square23;
    public GameObject squareRefX;
    public GameObject squareRefY;

    private void Awake()
    {
        //input = simulationManager.GetComponent<CustomInput>();
    }

    private void Update()
    {
        if (!isSelected)
        {
            meshX.GetComponent<TMP_InputField>().text = input.n.x.ToString();
            meshY.GetComponent<TMP_InputField>().text = input.n.y.ToString();
            lengthX.GetComponent<TMP_InputField>().text = input.lx.ToString();
            lengthY.GetComponent<TMP_InputField>().text = input.ly.ToString();
            finalTime.GetComponent<TMP_InputField>().text = input.tf.ToString();
            frame.GetComponent<TMP_InputField>().text = input.frame.ToString();
            reynolds.GetComponent<TMP_InputField>().text = input.re.ToString();
            density.GetComponent<TMP_InputField>().text = input.density.ToString();
            cfl.GetComponent<TMP_InputField>().text = input.cfl.ToString();
            fo.GetComponent<TMP_InputField>().text = input.fo.ToString();
            characteristicLength.GetComponent<TMP_InputField>().text = input.characteristicLength.ToString();
            characteristicSpeed.GetComponent<TMP_InputField>().text = input.characteristicSpeed.ToString();
            characteristicLength.GetComponent<TMP_InputField>().text = input.characteristicLength.ToString();

            if (customMenu.activeSelf)
            {
                uLeft.GetComponent<TMP_InputField>().text = input.uSides[0].ToString();
                uRight.GetComponent<TMP_InputField>().text = input.uSides[1].ToString();
                uDown.GetComponent<TMP_InputField>().text = input.uSides[2].ToString();
                uUp.GetComponent<TMP_InputField>().text = input.uSides[3].ToString();
                vLeft.GetComponent<TMP_InputField>().text = input.vSides[0].ToString();
                vRight.GetComponent<TMP_InputField>().text = input.vSides[1].ToString();
                vDown.GetComponent<TMP_InputField>().text = input.vSides[2].ToString();
                vUp.GetComponent<TMP_InputField>().text = input.vSides[3].ToString();

                uIni.GetComponent<TMP_InputField>().text = input.uIni.ToString();
                vIni.GetComponent<TMP_InputField>().text = input.vIni.ToString();

                poly11.GetComponent<TMP_InputField>().text = input.poly[0, 0].ToString();
                poly12.GetComponent<TMP_InputField>().text = input.poly[0, 1].ToString();
                poly13.GetComponent<TMP_InputField>().text = input.poly[0, 2].ToString();
                poly14.GetComponent<TMP_InputField>().text = input.poly[0, 3].ToString();
                poly15.GetComponent<TMP_InputField>().text = input.poly[0, 4].ToString();
                poly21.GetComponent<TMP_InputField>().text = input.poly[1, 0].ToString();
                poly22.GetComponent<TMP_InputField>().text = input.poly[1, 1].ToString();
                poly23.GetComponent<TMP_InputField>().text = input.poly[1, 2].ToString();
                poly24.GetComponent<TMP_InputField>().text = input.poly[1, 3].ToString();
                poly25.GetComponent<TMP_InputField>().text = input.poly[1, 4].ToString();

                polyRefX.GetComponent<TMP_InputField>().text = input.refPointPoly.x.ToString();
                polyRefY.GetComponent<TMP_InputField>().text = input.refPointPoly.y.ToString();

                square11.GetComponent<TMP_InputField>().text = input.square[0, 0].ToString();
                square12.GetComponent<TMP_InputField>().text = input.square[0, 1].ToString();
                square13.GetComponent<TMP_InputField>().text = input.square[0, 2].ToString();
                square21.GetComponent<TMP_InputField>().text = input.square[1, 0].ToString();
                square22.GetComponent<TMP_InputField>().text = input.square[1, 1].ToString();
                square23.GetComponent<TMP_InputField>().text = input.square[1, 2].ToString();

                squareRefX.GetComponent<TMP_InputField>().text = input.refPointSquare.x.ToString();
                squareRefY.GetComponent<TMP_InputField>().text = input.refPointSquare.y.ToString();
            }
        }
    }

    public void InputMeshX(string valueStr)
    {
        int value;
        if (int.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.n.x = value;
            }
        }
    }

    public void InputMeshY(string valueStr)
    {
        int value;
        if (int.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.n.y = value;
            }
        }
    }

    public void InputLengthX(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.lx = value;
            }
        }
    }

    public void InputLengthY(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.ly = value;
            }
        }
    }

    public void InputFinalTime(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.tf = value;
            }
        }
    }

    public void InputFrame(string valueStr)
    {
        int value;
        if (int.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.frame = value;
            }
        }
    }

    public void InputReynolds(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.re = value;
            }
        }
    }

    public void InputDensity(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.re = value;
            }
        }
    }

    public void InputCfl(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.cfl = value;
            }
        }
    }

    public void InputFo(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.fo = value;
            }
        }
    }

    public void InputCharacteristicLength(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.characteristicLength = value;
            }
        }
    }

    public void InputCharacteristicSpeed(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            if (value > 0)
            {
                input.characteristicSpeed = value;
            }
        }
    }

    public void DropdownScheme(int value)
    {
        value += 1;
        switch (value)
        {
            case > 0 when value == 1:
                input.scheme = "CD4";
                break;
            case > 0 when value == 2:
                input.scheme = "CD2";
                break;
            case > 0 when value == 3:
                input.scheme = "UR1";
                break;
        }
    }

    public void ToggleCustom(bool value)
    {
        input.custom = value;
    }

    public void InputULeft(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.uSides[0] = value;
        }
    }

    public void InputURight(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.uSides[1] = value;
        }
    }

    public void InputUDown(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.uSides[2] = value;
        }
    }

    public void InputUUp(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.uSides[3] = value;
        }
    }

    public void InputVLeft(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.vSides[0] = value;
        }
    }

    public void InputVRight(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.vSides[1] = value;
        }
    }

    public void InputVDown(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.vSides[2] = value;
        }
    }

    public void InputVUp(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.vSides[3] = value;
        }
    }

    public void InputUIni(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.uIni = value;
        }
    }

    public void InputVIni(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.vIni = value;
        }
    }

    public void InputPoly11(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[0, 0] = value;
        }
    }

    public void InputPoly12(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[0, 1] = value;
        }
    }

    public void InputPoly13(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[0, 2] = value;
        }
    }

    public void InputPoly14(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[0, 3] = value;
        }
    }

    public void InputPoly15(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[0, 4] = value;
        }
    }

    public void InputPoly21(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[1, 0] = value;
        }
    }

    public void InputPoly22(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[1, 1] = value;
        }
    }

    public void InputPoly23(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[1, 2] = value;
        }
    }

    public void InputPoly24(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[1, 3] = value;
        }
    }

    public void InputPoly25(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.poly[1, 4] = value;
        }
    }

    public void InputPolyRefX(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.refPointPoly.x = value;
        }
    }

    public void InputPolyRefY(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.refPointPoly.y = value;
        }
    }

    public void InputSquare11(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.square[0, 0] = value;
        }
    }

    public void InputSquare12(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.square[0, 1] = value;
        }
    }

    public void InputSquare13(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.square[0, 2] = value;
        }
    }

    public void InputSquare21(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.square[1, 0] = value;
        }
    }

    public void InputSquare22(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.square[1, 1] = value;
        }
    }

    public void InputSquare23(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.square[2, 2] = value;
        }
    }

    public void InputSquareRefX(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.refPointSquare.x = value;
        }
    }

    public void InputSquareRefY(string valueStr)
    {
        float value;
        if (float.TryParse(valueStr, out value))
        {
            input.refPointSquare.y = value;
        }
    }

    public void InputSelected()
    {
        isSelected = true;
    }

    public void InputNotSelected()
    {
        isSelected = false;
    }
}
