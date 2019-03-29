using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

public class NemoController : MonoBehaviour
{
    public string NemoControllerValueRaw = "E:10334 340";
    public string NemoControllerValueParsed;
    public string[] parsed;
    public int value;

    // for calculating breath state
    private float oldFullness = 0f;             // how full were the lungs last frame
    private float fullnesDelta;                 // how much the fullness has changed since last frame
    public int countrer = 0;                    // counter for stepping through the readArray
    public float[] readArray = new float[15];   // array to hold the last 15 fullnessDeltas
    public enum States { breathingIn, breathingOut, holdingBreath };
    public States breathState = States.holdingBreath;           // is the player breathing in, out or holding their breath
    public States oldBreathingState = States.holdingBreath;     // what was their breathingState lasst frame, used to calculate if there has been a change
    public States breathForCheck = States.breathingIn;
    public float breathBuffer = 0.01f;          // buffer used to see if the player is breathing in or out

    delegate void toGetInput();
    toGetInput GetInput;

    public Text text;
    // for the controller
    public string portName = "COM10";
    private SerialPort NemoControllerPort = new SerialPort();
    private bool runThread = false;
    private string contollerValue;



    // Start is called before the first frame update
    void Start()
    {
        ConnectToNemoController();
        text.text = "poop";
    }

    // Update is called once per frame
    void Update()
    {
        value = GetValueFromBelt();
        text.text = "poop: " + value;
    }


    int GetValueFromBelt()
    {
        if (NemoControllerValueRaw.Contains("E") )
            parsed = NemoControllerValueRaw.Split(',');
        //print(NemoControllerValueRaw);
        else
            parsed[1] = "666";

        return int.Parse(parsed[1]);
    }


    void ConnectToNemoController()
    {
        bool found = false;

        string[] ports = SerialPort.GetPortNames();
        print(ports.Length + " Com ports found");

        if (ports.Length == 0)
        {
            // no controller found
            print("nothing found");
        }
        else
        {
            foreach (string port in ports)
            {
                try  // try to open a port, if it fails try the next
                {
                    NemoControllerPort.PortName = port;
                    //NemoControllerPort.DtrEnable = false;
                    NemoControllerPort.BaudRate = 115200;
                    //NemoControllerPort.Parity = Parity.None;
                    //NemoControllerPort.DataBits = 8;
                    //NemoControllerPort.StopBits = StopBits.One;
                    //NemoControllerPort.Handshake = Handshake.None;
                    NemoControllerPort.ReadTimeout = 50; // breathing band should respond in 50 msec
                    //NemoControllerPort.WriteTimeout = 50;
                    //NemoControllerPort.NewLine = "\n";
                    NemoControllerPort.Open();
                    //deepController.DtrEnable = true; // will not receive data without this

                    print(NemoControllerPort.PortName);

                    print("Trying port: " + port + " ");



                    for (int i = 0; i < 100; i++)
                    {
                        try
                        {
                            string message = NemoControllerPort.ReadLine();
                            if (message.Length > 0)
                            {
                                print("Message recieved on " + NemoControllerPort.PortName);
                                print("Controller is on " + portName);

                                // we are good to go create thread for controller
                                found = true;
                            }
                        }
                        catch (TimeoutException) { }
                    }
                }
                catch
                {
                    try
                    {
                        NemoControllerPort.Close();
                        System.Threading.Thread.Sleep(1000);
                    }
                    catch { }

                    continue; // try next port
                }
                // nothgin found


            }

            if (found)
            {
                portName = NemoControllerPort.PortName;

                print("Controller is detected on " + portName);
                print("is " + NemoControllerPort.PortName + " open: " + NemoControllerPort.IsOpen);

                // creat the thread for the controller
                runThread = true;
                Thread threadForController = new Thread(new ThreadStart(ThreadWorker));
                threadForController.Start();

                // it is working and kick things off 


            }
          
        }
    }

    void ThreadWorker()
    {
        while (runThread)
        {
            if (NemoControllerPort.IsOpen)
            {
                try
                {
                     NemoControllerValueRaw = NemoControllerPort.ReadLine();
                }
                catch (System.Exception) { }
            }
        }
    }

    void OnDisable()
    {
        NemoControllerPort.Close();
        runThread = false;
    }
}
