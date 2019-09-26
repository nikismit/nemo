using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class YiPPStarterConnector : MonoBehaviour
{
    //----- SINGLETON -----------------------------------------------------------//
	public static 		YiPPStarterConnector 	Instance { get; private set; }
	//---------------------------------------------------------------------------//
	private         	UdpClient       		UDPClient = null;
    private             int             		portReceive = 9277;
    private             int             		portSend = 9477;
    private             IPAddress               localhost = new IPAddress(new byte[]{127,0,0,1});
    private             float                   waitTillQuit = 0.1f;
	//---------------------------------------------------------------------------//
    private             float                   heartbeatTimer = 0;
    private             float                   heartbeatTime = 0.5f; //seconds
    //---------------------------------------------------------------------------//

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UDPClient = new UdpClient();
		UDPClient.Client.SetSocketOption( SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true );
		UDPClient.Client.Bind( new IPEndPoint(IPAddress.Any, portReceive) );

        heartbeatTimer = heartbeatTime;
    }

    void Update()
    {
        heartbeatTimer -= Time.deltaTime;
        if(heartbeatTimer <= 0)
        {
            heartbeatTimer = heartbeatTime;
            sendUDP("UnityHeartBeat");
        }
    }

    //DEBUG
    private IEnumerator WaitForQuit(int reason) 
    {
        print("Quiting" + reason.ToString());
        if(reason == 0)
            sendUDP("UnityQuitClean");
        else if (reason == 1)
            sendUDP("UnityQuitCleanNoRestart");
        else if(reason == 2)
            sendUDP("UnityQuitCleanQuitYippStarter");
        else if (reason == 3)
            sendUDP("UnityQuitBlackQuickRestart");

        yield return new WaitForSeconds(waitTillQuit);

        Application.Quit();
    }
    
    //sending
    private void sendUDP(string message)
    {
        byte[] messagebuf = Encoding.ASCII.GetBytes(message);
        IPEndPoint endPoint = new IPEndPoint(localhost, portSend);
        UDPClient.Send(messagebuf, messagebuf.Length, endPoint);
    }
    
}