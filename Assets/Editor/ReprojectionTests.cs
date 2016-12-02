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
        public void FargoLatLon_ToUTMUp()
        {
            double latitude = 47.022678;
            double longitude = -96.8155631226544;
            double expectedUTMNorthing = 5210000;
            double expectedUTMEasting = 666000;



            int UTMNorthing;
            int UTMEasting;
            string UTMZone;

            CoordinateTranslation.LatLongtoUTM(latitude, longitude, out UTMNorthing, out UTMEasting, out UTMZone);

            Assert.AreEqual(
                new Vector2((int)expectedUTMNorthing, (int)expectedUTMEasting),
                new Vector2((int)UTMNorthing, (int)UTMEasting)
                );
        }
        [Test]
        public void FargoLatLon_ToUTMMid()
        {
            double latitude = 46.932755;
            double longitude = -96.81922393958703;
            int expectedUTMNorthing = 5200000;
            int expectedUTMEasting = 666000;



            int UTMNorthing;
            int UTMEasting;
            string UTMZone;

            CoordinateTranslation.LatLongtoUTM(latitude, longitude, out UTMNorthing, out UTMEasting, out UTMZone);

            Assert.AreEqual(
                new Vector2((int)expectedUTMNorthing, (int)expectedUTMEasting),
                new Vector2((int)UTMNorthing, (int)UTMEasting)
                );
        }

        [Test]
        public void FargoLatLon_ToUTMDown()
        {
            double latitude = 46.842831;
            double longitude = -96.82286717333807;
            int expectedUTMNorthing = 5190000;
            int expectedUTMEasting = 666000;



            int UTMNorthing;
            int UTMEasting;
            string UTMZone;

            CoordinateTranslation.LatLongtoUTM(latitude, longitude, out UTMNorthing, out UTMEasting, out UTMZone);

            Assert.AreEqual(
                new Vector2((int)expectedUTMNorthing, (int)expectedUTMEasting),
                new Vector2((int)UTMNorthing, (int)UTMEasting)
                );
        }

        [Test]
        public void FargoLatLon_ToUTM3()
        {
            double latitude = 46.930178;
            double longitude = -96.68795823923735;
            int expectedUTMNorthing = 5200000;
            int expectedUTMEasting = 676000;


            int UTMNorthing;
            int UTMEasting;
            string UTMZone;

            CoordinateTranslation.LatLongtoUTM(latitude, longitude, out UTMNorthing, out UTMEasting, out UTMZone);

            Assert.AreEqual(
                new Vector2((int)expectedUTMNorthing, (int)expectedUTMEasting),
                new Vector2((int)UTMNorthing, (int)UTMEasting)
                );
        }
    }
}
#endif