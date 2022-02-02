using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using UnscentedKalmanFilter;
using ZedGraph;

namespace Example;

internal class Program
{
	private static void Main()
	{
		var filter = new UKF();
		var measurements = new List<double>();
		var states = new List<double>();
		var rnd = new Random();
		for (var k = 0; k < 100; k++)
		{
			var measurement = Math.Sin(k * 3.14 * 5 / 180) + (double) rnd.Next(50) / 100;
			measurements.Add(measurement);
			filter.Update(new[] {measurement});
			states.Add(filter.getState()[0]);
		}
		var myPane = new GraphPane(new RectangleF(0, 0, 3200, 2400), "Unscented Kalman Filter",
			"number", "measurement");
		var measurementsPairs = new PointPairList();
		var statesPairs = new PointPairList();
		for (var i = 0; i < measurements.Count; i++)
		{
			measurementsPairs.Add(i, measurements[i]);
			statesPairs.Add(i, states[i]);
		}
		myPane.AddCurve("measurement", measurementsPairs, Color.Red, SymbolType.Circle);
		myPane.AddCurve("estimate", statesPairs, Color.Green, SymbolType.XCross);
		var bm = new Bitmap(200, 200);
		var g = Graphics.FromImage(bm);
		myPane.AxisChange(g);
		Image im = myPane.GetImage();
		im.Save("../../../../Data/result.png", ImageFormat.Png);
	}
}