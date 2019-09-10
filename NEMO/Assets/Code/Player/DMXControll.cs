using UnityEngine;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System;

public class DMXControll : MonoBehaviour
{
    public Analyser breathData;

    private SerialPort DMXController = new SerialPort();

    public int portNum = 1;

    private const int N_DMX_CHANNELS = 512;
    public int nChannels { get { return N_DMX_CHANNELS; } }

    #region --- DMX PRO API
    /// <summary>
    /// DMX PRO API
    /// https://www.enttec.com/docs/dmx_usb_pro_api_spec.pdf
    ///	The Mk2 API is available upon request to Enttec
    /// </summary>
    private const byte DMX_PRO_HEADER_SIZE = 4;
    private const byte DMX_PRO_START_MSG = 0x7E;
    private const byte DMX_PRO_LABEL_DMX = 6;
    private const byte DMX_PRO_LABEL_SERIAL_NUMBER = 10;
    private const byte DMX_PRO_START_CODE = 0;
    private const byte DMX_PRO_START_CODE_SIZE = 1;
    private const byte DMX_PRO_END_MSG = 0xE7;
    private const byte DMX_PRO_END_MSG_SIZE = 1;
    //
    private const int DMX_PRO_DATA_INDEX_OFFSET = 5;
    private const int DMX_PRO_MESSAGE_OVERHEAD = 6;
    #endregion

    /// <summary>
    /// Thread that handles the serial I/O.
    /// </summary>
    private Thread dmxThread;

    /// <summary>
    /// Flag to indicate Levels have been updated and DMX needs to be sent
    /// </summary>
    public bool updateDMX;

    public int serialPortIdx;

    /// <summary>
	/// Buffer containg level of all DMX channels.
	/// </summary>
	public byte[] DMXLevels = new byte[N_DMX_CHANNELS];

    /// <summary>
    /// DMX message buffer length.
    /// </summary>
    private const int TX_BUFFER_LENGTH = DMX_PRO_MESSAGE_OVERHEAD + N_DMX_CHANNELS;

    /// <summary>
    /// DMX message buffer.
    /// </summary>
    private byte[] TxBuffer = new byte[DMX_PRO_MESSAGE_OVERHEAD + N_DMX_CHANNELS];

    public Vector4 backColor;
    public Vector4 breathColor;
    public float delaySpeed;
    public bool direction;
    public bool directionBreath;
    public AnimationCurve curver;
    public float delay = 0.2f;

    // Start is called before the first frame update
    void Start()
    {

        OpenSerialPort();
        //Init the TX Buffer
        initTXBuffer();

        //Start the serial io thread
        dmxThread = new Thread(ThreadedIO);
        dmxThread.Start();

        //Flag to send default DMX values
        // updateDMX = true;
    }


    /// <summary>
	/// The Threaded function that processes the serial i/o.
	/// </summary>
	private void ThreadedIO()
    {
        Debug.Log("Thread Start");
        while (true)
        {
            // if (updateDMX)
            //{
            // updateDMX = false;

            float minBrt = Helpers.minValueRange(breathData.breathLowPass,250);
            float maxBrt = Helpers.maxValueRange(breathData.breathLowPass, 250);

            float newMin = 0;
            float newMax = 1;
            if (directionBreath)
            {
                newMin = 1;
                newMax = 0;
            }
            float scaleBrt = Helpers.scale(minBrt, maxBrt, newMin, newMax, breathData.breathLowPass[0]);
            scaleBrt *= curver.Evaluate(scaleBrt);
            int delay1 = Mathf.RoundToInt(delay * 25);
            float scaleBrt2 = Helpers.scale(minBrt, maxBrt, newMin, newMax, breathData.breathLowPass[delay1]);
            scaleBrt2 *= curver.Evaluate(scaleBrt2);
            int delay2 = Mathf.RoundToInt(delay * 2 * 25);
            float scaleBrt3 = Helpers.scale(minBrt, maxBrt, newMin, newMax, breathData.breathLowPass[delay2]);
            scaleBrt3 *= curver.Evaluate(scaleBrt3);

            float brtLight1 = Mathf.Round(scaleBrt*255);
            float brtLight2 = Mathf.Round(scaleBrt2 * 255);
            float brtLight3 = Mathf.Round(scaleBrt3 * 255);

            for (int i =0; i < 4; i++)
            {
                if (direction)
                {
                    DMXLevels[i] = (byte)brtLight1;
                  
                }
                else
                {
                    DMXLevels[i] = (byte)brtLight3;
                   
                }
              
            }

            

            for (int i = 4; i < 8; i++)
            {
                DMXLevels[i] = (byte)brtLight2;

            }

           

            for (int i = 8; i < 12; i++)
            {
                if (direction)
                {
                    DMXLevels[i] = (byte)brtLight3;
                   
                }
                else
                {
                    DMXLevels[i] = (byte)brtLight1;
                  
                }
            }

          


            Buffer.BlockCopy(DMXLevels, 0, TxBuffer, DMX_PRO_DATA_INDEX_OFFSET, N_DMX_CHANNELS);
            if (DMXController != null && DMXController.IsOpen) { DMXController.Write(TxBuffer, 0, TX_BUFFER_LENGTH); };
            // }

            //TODO: Recieve Serial
            //if (serialPort.BytesToRead > 0)
        }
    }

    /// <summary>
	/// Init the TxBufffer with Header and End bytes
	/// </summary>
	private void initTXBuffer()
    {
        for (int i = 0; i < TX_BUFFER_LENGTH; i++) TxBuffer[i] = (byte)255;

        TxBuffer[000] = DMX_PRO_START_MSG;
        TxBuffer[001] = DMX_PRO_LABEL_DMX;
        TxBuffer[002] = (byte)(N_DMX_CHANNELS + 1 & 255); ;
        TxBuffer[003] = (byte)((N_DMX_CHANNELS + 1 >> 8) & 255);
        TxBuffer[004] = DMX_PRO_START_CODE;
        TxBuffer[517] = DMX_PRO_END_MSG;
    }


    public void OpenSerialPort()
    {
        //ConnectToNemoController();
        string[] ports = SerialPort.GetPortNames();

        Debug.Log(ports[portNum]);

        if (DMXController != null)
        {
            DMXController.Close();
            DMXController.Dispose();
        }

        DMXController = new SerialPort(ports[portNum], 57600, Parity.None, 8, StopBits.One);

        try
        {
            DMXController.Open();
            DMXController.ReadTimeout = 50;
            Debug.Log("connecting:" + ports[portNum]);
            updateDMX = true;
        }
        catch (System.Exception e)
        {
            Debug.Log("gaat niet goed");
            Debug.LogException(e);
            serialPortIdx = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnDisable()
    {
        DMXController.Close();
        dmxThread.Abort();
    }
}
