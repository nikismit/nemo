using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class GeneralHelper : MonoBehaviour
{
    //----------------------------------------------------------------------//
    // ******************        STRIPPED VERSION        ****************** //
    //----------------------------------------------------------------------//
    public static GeneralHelper Instance { get; private set; }
    [SerializeField] string ProductName;
    private WindowHandler w;
    //----------------------------------------------------------------------//

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        initStayOnTop();
    }

    public IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    private void initStayOnTop()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            w = new WindowHandler(ProductName, 0);
            InvokeRepeating("repeatBringToFront", 1f, 7f);
        }
    }
    private void OnApplicationFocus(bool _focusStatus)
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer && !_focusStatus)
        {
            StartCoroutine(bringToFront());
        }
    }
    private IEnumerator bringToFront()
    {
        yield return StartCoroutine(WaitForRealSeconds(0.75f));       // not sure if this is needed

        if (w != null)
            w.SwitchToWindow();//
    }
    private void repeatBringToFront()
    {
        StartCoroutine(bringToFront());
    }
}

public class WindowHandler
{
    private string WINDOW_NAME = "";            //name of the window
    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const UInt32 SWP_NOSIZE = 0x0001;
    const UInt32 SWP_NOMOVE = 0x0002;
    const UInt32 SWP_SHOWWINDOW = 0x0040;
    private int X_OFFSET = 0;

    //Import find window function
    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("User32.dll", SetLastError = true)]
    static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    public WindowHandler(string _windowName, int _xOffset)
    {
        WINDOW_NAME = _windowName;
        X_OFFSET = _xOffset;
    }
    public void SwitchToWindow()
    {
        IntPtr window = FindWindowByCaption(IntPtr.Zero, WINDOW_NAME);

        if (window == IntPtr.Zero)
            return;

        SwitchToThisWindow(window, true);
        //SetWindowPos( window, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW );               // always centered in middle
        SetWindowPos(window, HWND_TOPMOST, X_OFFSET, 0, 0, 0, SWP_NOSIZE | SWP_SHOWWINDOW);                        // always topleft corner
    }
}