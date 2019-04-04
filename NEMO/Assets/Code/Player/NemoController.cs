using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine.SceneManagement;

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

    public delegate void GameStartHandler();
    public event GameStartHandler GameStartEvent;
    public delegate void GameEndHandler();
    public event GameEndHandler GameEndEvent;
    public delegate void GameResetHandler();
    public event GameResetHandler GameResetEvent;

    public GameEvent ControllerConnectedEvent;      // Event that invokes on finding connection with controller
    public GameEvent BeltConnectedEvent;            // Event that invokes on connecting the belt
    public GameEvent BeltDisconnectedEvent;         // Event that invokes on disconnecting the belt

    private bool _isBeltConnected = false;

    [SerializeField]
    private bool _debug = false;                    // Show debug messages

    private IEnumerator TryConnect()
    {
        while ((value == 666 || value == 0) && (!runThread))
        {
            ConnectToNemoController();
            yield return new WaitForSeconds(1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TryConnect());
        //text.text = "poop";
    }

    // Update is called once per frame
    void Update()
    {
        if (!runThread)
            return;

        value = GetValueFromBelt();
        //text.text = "poop: " + value;

        // Belt is connected
        if (value != 666 && value != 0 && !_isBeltConnected)
        {
            _isBeltConnected = true;
            BeltConnectedEvent.Invoke();

            DebugMessage("BELT CONNECTED");
        }

        // Belt is disconnected
        if (NemoControllerValueRaw == "B:0" && _isBeltConnected)
        {
            _isBeltConnected = false;
            BeltDisconnectedEvent.Invoke();

            DebugMessage("BELT DISCONNECTED");
        }
    }


    int GetValueFromBelt()
    {
        if (NemoControllerValueRaw.Contains("E"))
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
        DebugMessage(ports.Length + " Com ports found");

        if (ports.Length == 0)
        {
            // no controller found
            DebugMessage("nothing found");
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

                    DebugMessage(NemoControllerPort.PortName);

                    DebugMessage("Trying port: " + port + " ");



                    for (int i = 0; i < 100; i++)
                    {
                        try
                        {
                            string message = NemoControllerPort.ReadLine();
                            if (message.Length > 0)
                            {
                                DebugMessage("Message recieved on " + NemoControllerPort.PortName);
                                DebugMessage("Controller is on " + portName);

                                // we are good to go create thread for controller
                                found = true;
                                ControllerConnectedEvent.Invoke();		// Invoke the controller connected event
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

                DebugMessage("Controller is detected on " + portName);
                DebugMessage("is " + NemoControllerPort.PortName + " open: " + NemoControllerPort.IsOpen);

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

    private void DebugMessage(string message)
    {
        if (_debug)
            Debug.Log(message);
    }
}
