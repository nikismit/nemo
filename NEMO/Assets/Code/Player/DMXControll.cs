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
    public NemoController controller;

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
    public bool direction;
    public bool directionBreath;
    public AnimationCurve curver;
    public float delay = 0.2f;

    public Vector2[] dmxSettings = new Vector2[6];
    public Vector2[] dmxSettingsBack = new Vector2[6];
    public bool simulate;
    public float Timertje;
    public float relVal;

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

            float brtVal = 0;
            float brtVal2 = 0;
            float brtVal3 = 0;
            float brtVal4 = 0;
            float brtVal5 = 0;
            float brtVal6 = 0;

            float minBrt = -1;
            float maxBrt = 1;
            if (simulate) {
                brtVal = Mathf.Sin(2 * Mathf.PI * Timertje * 0.4f);
                brtVal2 = Mathf.Sin(2 * Mathf.PI * (Timertje+0.15f) * 0.4f);
                brtVal3 = Mathf.Sin(2 * Mathf.PI * (Timertje + 0.3f) * 0.4f);
                brtVal4 = Mathf.Sin(2 * Mathf.PI * (Timertje + 0.6f) * 0.4f);
                brtVal5 = Mathf.Sin(2 * Mathf.PI * (Timertje + 0.75f) * 0.4f);
                brtVal6 = Mathf.Sin(2 * Mathf.PI * (Timertje + 0.9f) * 0.4f);
            }

            if(simulate == false && controller._isBeltConnected == false || controller.normalBreathing == false)
            {
                    if(directionBreath ==false){
                    brtVal = -1;
                    brtVal2 = -1;
                    brtVal3 = -1;
                    brtVal4 = -1;
                    brtVal5 = -1;
                    brtVal6 = -1;
                    }

                    if(directionBreath == true){
                    brtVal = 1;
                    brtVal2 = 1;
                    brtVal3 = 1;
                    brtVal4 = 1;
                    brtVal5 = 1;
                    brtVal6 = 1;
                    }
                   
              
            }
            if(simulate == false && controller._isBeltConnected == true  && controller.normalBreathing == true)
            {
                  brtVal = breathData.breathLowPass[0];
                  brtVal2 = breathData.breathLowPass[Mathf.RoundToInt(delay*25)];
                  brtVal3 = breathData.breathLowPass[Mathf.RoundToInt(delay * 25 * 2)];
                  brtVal4 = breathData.breathLowPass[Mathf.RoundToInt(delay * 25 * 3)];
                  brtVal5 = breathData.breathLowPass[Mathf.RoundToInt(delay * 25 * 4)];
                  brtVal6 = breathData.breathLowPass[Mathf.RoundToInt(delay * 25 * 5)];
                
              /*  brtVal2 = breathData.breathLowPass[0];
                brtVal3 = breathData.breathLowPass[0];
                brtVal4 = breathData.breathLowPass[0];
                brtVal5 = breathData.breathLowPass[0];
                brtVal6 = breathData.breathLowPass[0];
                */
                minBrt = Helpers.minValueRange(breathData.breathLowPass, 250);
              maxBrt = Helpers.maxValueRange(breathData.breathLowPass, 250);
            }
           
            
            float newMin = 0;
            float newMax = 1;
            if (directionBreath)
            {
                newMin = 1;
                newMax = 0;
            }

            float scaleBrt = Helpers.scale(minBrt, maxBrt, newMin, newMax, brtVal);
            relVal = scaleBrt;
            scaleBrt *= curver.Evaluate(scaleBrt);
           
            float scaleBrt2 = Helpers.scale(minBrt, maxBrt, newMin, newMax, brtVal2);
            scaleBrt2 *= curver.Evaluate(scaleBrt2);
           
            float scaleBrt3 = Helpers.scale(minBrt, maxBrt, newMin, newMax, brtVal3);
            scaleBrt3 *= curver.Evaluate(scaleBrt3);

            float scaleBrt4 = Helpers.scale(minBrt, maxBrt, newMin, newMax, brtVal4);
            scaleBrt4 *= curver.Evaluate(scaleBrt3);

            float scaleBrt5 = Helpers.scale(minBrt, maxBrt, newMin, newMax, brtVal5);
            scaleBrt5 *= curver.Evaluate(scaleBrt5);

            float scaleBrt6 = Helpers.scale(minBrt, maxBrt, newMin, newMax, brtVal6);
            scaleBrt6 *= curver.Evaluate(scaleBrt6);

           float brtLight1 = Mathf.Round(scaleBrt*255);
            float brtLight2 = Mathf.Round(scaleBrt2 * 255);
            float brtLight3 = Mathf.Round(scaleBrt3 * 255);
            float brtLight4 = Mathf.Round(scaleBrt4 * 255);
            float brtLight5 = Mathf.Round(scaleBrt5 * 255);
            float brtLight6 = Mathf.Round(scaleBrt6 * 255);
            

            /*
            float brtLight1 = Mathf.Round(scaleBrt * 255);
            float brtLight2 = Mathf.Round(scaleBrt * 255);
            float brtLight3 = Mathf.Round(scaleBrt * 255);
            float brtLight4 = Mathf.Round(scaleBrt * 255);
            float brtLight5 = Mathf.Round(scaleBrt * 255);
            float brtLight6 = Mathf.Round(scaleBrt * 255);
            */
            //Front Left

            if (direction) {

                DMXLevels[0] = (byte)brtLight1;
                DMXLevels[1] = (byte)brtLight1;//(byte)brtLight2;
                DMXLevels[2] = (byte)brtLight1;//(byte)brtLight3;
                DMXLevels[3] = (byte)brtLight1;//(byte)brtLight4;
                DMXLevels[4] = (byte)brtLight1;//(byte)brtLight5;
                DMXLevels[5] = (byte)brtLight1;//(byte)brtLight6; 
                
                /*for (int i = Mathf.RoundToInt(dmxSettings[0].x) ; i < Mathf.RoundToInt(dmxSettings[0].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight1;

                }

                //Front Right
                for (int i = Mathf.RoundToInt(dmxSettings[1].x); i < Mathf.RoundToInt(dmxSettings[1].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight6;

                }


                //MidLeft
                for (int i = Mathf.RoundToInt(dmxSettings[2].x); i < Mathf.RoundToInt(dmxSettings[2].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight2;

                }

                //MidRight
                for (int i = Mathf.RoundToInt(dmxSettings[3].x); i < Mathf.RoundToInt(dmxSettings[3].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight5;

                }


                //BackLeft
                for (int i = Mathf.RoundToInt(dmxSettings[4].x); i < Mathf.RoundToInt(dmxSettings[4].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight3;
                }

                //BackRight
                for (int i = Mathf.RoundToInt(dmxSettings[5].x); i < Mathf.RoundToInt(dmxSettings[5].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight4;
                }*/

            }
            else
            {
              /*  DMXLevels[0] = (byte)brtLight6;
                DMXLevels[1] = (byte)brtLight5;
                DMXLevels[2] = (byte)brtLight4;
                DMXLevels[3] = (byte)brtLight3;
                DMXLevels[4] = (byte)brtLight2;
                DMXLevels[5] = (byte)brtLight1; 
*/
                DMXLevels[0] = (byte)brtLight1;
                DMXLevels[1] = (byte)brtLight1;//(byte)brtLight2;
                DMXLevels[2] = (byte)brtLight1;//(byte)brtLight3;
                DMXLevels[3] = (byte)brtLight1;//(byte)brtLight4;
                DMXLevels[4] = (byte)brtLight1;//(byte)brtLight5;
                DMXLevels[5] = (byte)brtLight1;//(byte)brtLight6; 
                /*
                for (int i = Mathf.RoundToInt(dmxSettingsBack[0].x); i < Mathf.RoundToInt(dmxSettingsBack[0].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight1;

                }

                //Front Right
                for (int i = Mathf.RoundToInt(dmxSettingsBack[1].x); i < Mathf.RoundToInt(dmxSettingsBack[1].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight6;

                }


                //MidLeft
                for (int i = Mathf.RoundToInt(dmxSettingsBack[2].x); i < Mathf.RoundToInt(dmxSettingsBack[2].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight2;

                }

                //MidRight
                for (int i = Mathf.RoundToInt(dmxSettingsBack[3].x); i < Mathf.RoundToInt(dmxSettingsBack[3].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight5;

                }


                //BackLeft
                for (int i = Mathf.RoundToInt(dmxSettingsBack[4].x); i < Mathf.RoundToInt(dmxSettingsBack[4].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight3;
                }

                //BackRight
                for (int i = Mathf.RoundToInt(dmxSettingsBack[5].x); i < Mathf.RoundToInt(dmxSettingsBack[5].y); i++)
                {
                    DMXLevels[i] = (byte)brtLight4;
                }
                */
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
        Timertje = Time.time;
    }


    void OnDisable()
    {
        DMXController.Close();
        dmxThread.Abort();
    }
}
