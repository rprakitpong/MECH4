using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class ConnectBtn : MonoBehaviour
{
    private SerialPort serialPort = null;

    private int currData = 0; // 1 = Ax, 2 = Ay, 3 = Az
    private int a_def = 130;
    private int a_x = 130; // default values
    private int a_y = 130;
    private int a_z = 130;

    // Start is called before the first frame update
    void Start()
    {
        Dropdown dd = this.gameObject.GetComponent<Dropdown>();
        // initialize com port selection with available ports
        foreach (string s in SerialPort.GetPortNames())
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = s;
            dd.options.Add(od);
        }

        dd.onValueChanged.AddListener(delegate { ConnectSerial(dd); });       
    }

    private void ConnectSerial(Dropdown change)
    {
        string comPort = change.options[change.value].text; // get selected port value
        if (!isPort0to9(comPort))
        {
            comPort = "\\\\.\\" + comPort; // this is a unity thing. if port is > 9, you need this string in front
        }
        if (serialPort == null) // if the previous port is not closed and null, then don't make a new connection
        {
            serialPort = new SerialPort(comPort, 9600, Parity.None, 8, StopBits.One);
            while (!serialPort.IsOpen)
            {
                serialPort.Open();
            } // TA recommended something like this in case open doesn't actually open
            serialPort.ReadTimeout = 300;
            serialPort.DiscardInBuffer(); // remove data before reading is supposed to start
        }
    }

    private bool isPort0to9(string comPort)
    {
        return (comPort == "COM0" ||
            comPort == "COM1" ||
            comPort == "COM2" ||
            comPort == "COM3" ||
            comPort == "COM4" ||
            comPort == "COM5" ||
            comPort == "COM6" ||
            comPort == "COM7" ||
            comPort == "COM8" ||
            comPort == "COM9");
    }

    void OnApplicationQuit()
    {
        if (serialPort != null)
        {
            serialPort.Close(); // close port on game stop
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (serialPort != null)
        {
            while (serialPort.BytesToRead > 0)
            {
                int res = serialPort.ReadByte(); // read byte until there're no more bytes to read

                if (res == 255)
                {
                    currData = 1;

                    float angle = GetAngle(a_x, a_y, a_z); // calculate angle of tilt of board
                    EventManager.Instance.PublishFenceAngleEvent(angle); // publish it as new angle
                }
                else
                {
                    switch (currData)
                    {
                        case 1:
                            a_x = res - a_def; // normalize a per default value
                            break;
                        case 2:
                            a_y = res - a_def;
                            break;
                        case 3:
                            a_z = res - a_def;
                            break;
                    }
                    currData = currData + 1;
                }
            }
        }
    }

    private float GetAngle(int ax, int ay, int az)
    {
        float angle = Mathf.Atan2((float)ax, (float)ay); // simple atan to calculate angle
        //Debug.Log("ax: " + ax + ", ay: " + ay + ", atan: " + angle);
        return angle; //radian
    }
}
