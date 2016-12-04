using UnityEngine;
using System.Collections;
using ETC.Platforms;

/// <summary>
/// An example lighting manager using the DMX class to interface with an Enttec DMX USB Pro.
/// Requires one RGB 3-channel fixture connected as device 1 (i.e. channels 1 through 3).
/// </summary>
/// <remarks>
/// Author: Bryan Maher (bm3n@andrew.cmu.edu) 26-Jan-2015
/// 
/// Feel free to use this example code as starting point for your own project.
/// </remarks>
public class DMXController : MonoBehaviour {
	/// <summary>
	/// Set this Editor property to the value of the DMX controller's COM port.
	/// <example>COM22</example>
	/// </summary>
    ///
	static string ComPort = "COM3";
	/// <summary>
	/// Instance of the DMX class used to control the lights.
	/// </summary>
	private static DMX dmx;

	// Use this for initialization
	static void Start () {
		dmx = new DMX(ComPort);
		// Cycle through the color wheel
	}

	/// <summary>
	/// Sets the lighting fixture to given color.
	/// </summary>
	/// <param name="color">Desired color.</param>
	public static void SetFan(bool state)
	{
        int deviceID = 1;


        if (state)
        {
            dmx.Channels[deviceID + 0] = (byte)(255);
            dmx.Channels[deviceID + 1] = (byte)(255);
        }
        else
        {
            dmx.Channels[deviceID + 0] = (byte)(0);
            dmx.Channels[deviceID + 1] = (byte)(0);
        }

		dmx.Send();
	}
}
