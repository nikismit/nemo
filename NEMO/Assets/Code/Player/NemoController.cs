using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine.SceneManagement;
using CM;

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

    public bool normalBreathing;
    public float breathOutTimer;
    public float breathInTimer;
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

    public bool _isBeltConnected = false;
    public int portSelect= 0;
    private IEnumerator TryConnect()
    {
        while ((value == 666 || value == 0) && (!runThread))
        {
            ConnectToNemoController();
            yield return new WaitForSeconds(1);
        }
    }

	private void Awake()
	{
		CM_Debug.AddCategory("NEMO Controller");
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

        

        value = GetValueFromBelt();;
        //text.text = "poop: " + value;

        // Belt is connected
        if (value != 666 && value != 0 && !_isBeltConnected)
        {
            _isBeltConnected = true;
            BeltConnectedEvent.Invoke();

            CM_Debug.Log("NEMO Controller", "BELT CONNECTED");
        }

        // Belt is disconnected
        if (NemoControllerValueRaw == "B:0" && _isBeltConnected)
        {
            _isBeltConnected = false;
            BeltDisconnectedEvent.Invoke();

			CM_Debug.Log("NEMO Controller", "BELT DISCONNECTED");
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
		CM_Debug.Log("NEMO Controller", ports.Length + " Com ports found");

        if (ports.Length == 0)
        {
			// no controller found
			CM_Debug.Log("NEMO Controller", "nothing found");
        }
        else
        {
            //foreach (string port in ports)
            //{
                try  // try to open a port, if it fails try the next
                {
                    NemoControllerPort.PortName = ports[portSelect];
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

					CM_Debug.Log("NEMO Controller", NemoControllerPort.PortName);

					CM_Debug.Log("NEMO Controller", "Trying port: " + ports[portSelect] + " ");



                    for (int i = 0; i < 100; i++)
                    {
                        try
                        {
                            string message = NemoControllerPort.ReadLine();
                            if (message.Length > 0)
                            {
								CM_Debug.Log("NEMO Controller", "Message recieved on " + NemoControllerPort.PortName);
								CM_Debug.Log("NEMO Controller", "Controller is on " + portName);

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

                    //continue; // try next port
                }
                // nothgin found


            //}

            if (found)
            {
                portName = NemoControllerPort.PortName;

				CM_Debug.Log("NEMO Controller", "Controller is detected on " + portName);
				CM_Debug.Log("NEMO Controller", "is " + NemoControllerPort.PortName + " open: " + NemoControllerPort.IsOpen);

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
