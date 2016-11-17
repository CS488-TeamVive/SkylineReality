#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using NUnit.Framework;

namespace Reprojection.Tests
{
    public class ReprojectionTests
    {

        [Test]
        public void FargoLatLon_ToUTM()
        {
            double latitude = 46.8961201;
            double longitude = -96.8083852;
            double UTMNorthing;
            double UTMEasting;
            string UTMZone;

            CoordinateTranslation.LatLongtoUTM(latitude, longitude, out UTMNorthing, out UTMEasting, out UTMZone);

            //double UTMNorthingExpected = 

            //Assert.AreEqual();
        }
    }
}
#endif