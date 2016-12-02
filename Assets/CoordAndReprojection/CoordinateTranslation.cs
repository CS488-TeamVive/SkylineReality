using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class CoordinateTranslation : MonoBehaviour {

    [SerializeField]
    public int UTMNorthing;
    [SerializeField]
    public int UTMEasting;
    [SerializeField]
    public string UTMZone;
    [SerializeField]
    public int HeightmapNSLength;
    [SerializeField]
    public int HeightmapEWLength;
    [SerializeField]
    public int MinHeightActual = 0;
    [SerializeField]
    public float HeightScale = 1;
    [SerializeField]
    public float MetersPerPixel = 1;

    public void SetCenter(double latitude, double longitude)
    {
        LatLongtoUTM(latitude, longitude, out UTMNorthing, out UTMEasting, out UTMZone);
    }

    public void SetCenter(int UTMNorthing, int UTMEasting, string UTMZone)
    {
        this.UTMNorthing = UTMNorthing;
        this.UTMEasting = UTMEasting;
        this.UTMZone = UTMZone;
    }

    public float ElevationOffsetAtCoordinate(int UTMNorthing, int UTMEasting)
    {
        Terrain t = gameObject.GetComponent<Terrain>();
        TerrainData td = t.terrainData;

        //     elevation at coordinate 
        return (td.GetHeight(UTMEasting - this.UTMEasting - HeightmapEWLength / 2,
            UTMNorthing - this.UTMNorthing + HeightmapNSLength / 2))
        //  divided by heightscale
            * td.size.y;
            ; 

    }

    /**
     * This method assumes the given coordinates are in the same zone as this terrain.
     */
    public Vector3 RelativePosition(int UTMNorthing, int UTMEasting, int elevation)
    {
        return new Vector3(
            (float)(UTMEasting - this.UTMEasting - 0.5f * HeightmapEWLength),
            elevation - MinHeightActual,
            (float)(UTMNorthing - this.UTMNorthing + 0.5f * HeightmapNSLength)
            );
    }
    public Vector2 RelativePosition(int UTMNorthing, int UTMEasting)
    {
        return new Vector2(
            (float)(UTMEasting - this.UTMEasting - 0.5f * HeightmapEWLength),
            (float)(UTMNorthing - this.UTMNorthing + 0.5f * HeightmapNSLength)
            );
    }
    // does not consider zone
    public bool WithinExtent(double UTMNorthing, double UTMEasting)
    {
        Terrain t = gameObject.GetComponent<Terrain>();
        TerrainData td = t.terrainData;

        int meterWidth, meterLength;
        double boundLowerNorth, boundUpperNorth, boundLefterEast, boundRighterEast;

        meterWidth = td.heightmapResolution * (int)MetersPerPixel;
        meterLength = td.heightmapResolution * (int)MetersPerPixel;

        boundLowerNorth = this.UTMNorthing - HeightmapNSLength / 2;
        boundUpperNorth = this.UTMNorthing + HeightmapNSLength / 2;
        boundLefterEast = this.UTMEasting - HeightmapEWLength / 2;
        boundRighterEast = this.UTMEasting + HeightmapEWLength / 2;

        bool result = UTMNorthing > boundLowerNorth &&
            UTMNorthing < boundUpperNorth &&
            UTMEasting > boundLefterEast &&
            UTMEasting < boundRighterEast;

        Debug.Log(string.Format("Testing {0:N}, {1:N} within {2:N}-{3:N}, {4:N}-{5:N}\n => {6}",
            UTMNorthing,
            UTMEasting,
            boundLowerNorth,
            boundUpperNorth,
            boundLefterEast,
            boundRighterEast,
            result));

        return result;

    }

    /* constants adapted from http://forum.worldwindcentral.com/showthread.php?9863-C-code-to-convert-DD-to-UTM-here-it-is-!! */
    public const double deg2rad = 0.01745329251994329576923690768489;
    public const double latlong_a = 6378137;
    public const double eccSquared = 0.00669438;
    public const double k0 = 0.9996;

    /* method adapted from http://forum.worldwindcentral.com/showthread.php?9863-C-code-to-convert-DD-to-UTM-here-it-is-!! */
    public static void LatLongtoUTM(
        double Lat,
        double Long,
        out int UTMNorthing,
        out int UTMEasting,
        out string Zone)
    {
        double LongOrigin;
        double eccPrimeSquared;
        double N, T, C, A, M;

        //Make sure the longitude is between -180.00 .. 179.9
        double LongTemp = (Long + 180) - ((int)((Long + 180) / 360)) * 360 - 180; // -180.00 .. 179.9;

        double LatRad = Lat * deg2rad;
        double LongRad = LongTemp * deg2rad;
        double LongOriginRad;
        int ZoneNumber;

        ZoneNumber = ((int)((LongTemp + 180) / 6)) + 1;

        if (Lat >= 56.0 && Lat < 64.0 && LongTemp >= 3.0 && LongTemp < 12.0)
            ZoneNumber = 32;

        // Special zones for Svalbard
        if (Lat >= 72.0 && Lat < 84.0)
        {
            if (LongTemp >= 0.0 && LongTemp < 9.0) ZoneNumber = 31;
            else if (LongTemp >= 9.0 && LongTemp < 21.0) ZoneNumber = 33;
            else if (LongTemp >= 21.0 && LongTemp < 33.0) ZoneNumber = 35;
            else if (LongTemp >= 33.0 && LongTemp < 42.0) ZoneNumber = 37;
        }
        LongOrigin = (ZoneNumber - 1) * 6 - 180 + 3; //+3 puts origin in middle of zone
        LongOriginRad = LongOrigin * deg2rad;

        //compute the UTM Zone from the latitude and longitude
        Zone = ZoneNumber.ToString() + UTMLetterDesignator(Lat);

        eccPrimeSquared = (eccSquared) / (1 - eccSquared);

        N = latlong_a / Math.Sqrt(1 - eccSquared * Math.Sin(LatRad) * Math.Sin(LatRad));
        T = Math.Tan(LatRad) * Math.Tan(LatRad);
        C = eccPrimeSquared * Math.Cos(LatRad) * Math.Cos(LatRad);
        A = Math.Cos(LatRad) * (LongRad - LongOriginRad);

        M = latlong_a * ((1 - eccSquared / 4 - 3 * eccSquared * eccSquared / 64 - 5 * eccSquared * eccSquared * eccSquared / 256) * LatRad
        - (3 * eccSquared / 8 + 3 * eccSquared * eccSquared / 32 + 45 * eccSquared * eccSquared * eccSquared / 1024) * Math.Sin(2 * LatRad)
        + (15 * eccSquared * eccSquared / 256 + 45 * eccSquared * eccSquared * eccSquared / 1024) * Math.Sin(4 * LatRad)
        - (35 * eccSquared * eccSquared * eccSquared / 3072) * Math.Sin(6 * LatRad));

        UTMEasting = (int)((double)(k0 * N * (A + (1 - T + C) * A * A * A / 6
        + (5 - 18 * T + T * T + 72 * C - 58 * eccPrimeSquared) * A * A * A * A * A / 120)
        + 500000.0));

        UTMNorthing = (int)((double)(k0 * (M + N * Math.Tan(LatRad) * (A * A / 2 + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24
        + (61 - 58 * T + T * T + 600 * C - 330 * eccPrimeSquared) * A * A * A * A * A * A / 720))) + 1);
        if (Lat < 0)
            UTMNorthing += 10000000; //10000000 meter offset for southern hemisphere
    }

    /* method adapted from http://forum.worldwindcentral.com/showthread.php?9863-C-code-to-convert-DD-to-UTM-here-it-is-!! */
    private static char UTMLetterDesignator(double Lat)
    {
        char LetterDesignator;

        if ((84 >= Lat) && (Lat >= 72)) LetterDesignator = 'X';
        else if ((72 > Lat) && (Lat >= 64)) LetterDesignator = 'W';
        else if ((64 > Lat) && (Lat >= 56)) LetterDesignator = 'V';
        else if ((56 > Lat) && (Lat >= 48)) LetterDesignator = 'U';
        else if ((48 > Lat) && (Lat >= 40)) LetterDesignator = 'T';
        else if ((40 > Lat) && (Lat >= 32)) LetterDesignator = 'S';
        else if ((32 > Lat) && (Lat >= 24)) LetterDesignator = 'R';
        else if ((24 > Lat) && (Lat >= 16)) LetterDesignator = 'Q';
        else if ((16 > Lat) && (Lat >= 8)) LetterDesignator = 'P';
        else if ((8 > Lat) && (Lat >= 0)) LetterDesignator = 'N';
        else if ((0 > Lat) && (Lat >= -8)) LetterDesignator = 'M';
        else if ((-8 > Lat) && (Lat >= -16)) LetterDesignator = 'L';
        else if ((-16 > Lat) && (Lat >= -24)) LetterDesignator = 'K';
        else if ((-24 > Lat) && (Lat >= -32)) LetterDesignator = 'J';
        else if ((-32 > Lat) && (Lat >= -40)) LetterDesignator = 'H';
        else if ((-40 > Lat) && (Lat >= -48)) LetterDesignator = 'G';
        else if ((-48 > Lat) && (Lat >= -56)) LetterDesignator = 'F';
        else if ((-56 > Lat) && (Lat >= -64)) LetterDesignator = 'E';
        else if ((-64 > Lat) && (Lat >= -72)) LetterDesignator = 'D';
        else if ((-72 > Lat) && (Lat >= -80)) LetterDesignator = 'C';
        else LetterDesignator = 'Z'; //Latitude is outside the UTM limits
        return LetterDesignator;
    }
    

    class Ellipsoid
    {
        //Attributes
        public string ellipsoidName;
        public double EquatorialRadius;
        public double eccentricitySquared;

        public Ellipsoid(string name, double radius, double ecc)
        {
            ellipsoidName = name;
            EquatorialRadius = radius;
            eccentricitySquared = ecc;
        }
    };
}
