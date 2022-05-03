using Imagin.Common;
using Imagin.Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <source>
    /// https://softwarebydefault.com/2013/06/30/stained-glass-image-filter/
    /// </source>
    [Category(ImageEffectCategory.Sketch), DisplayName("Stained glass")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class StainedGlassEffect : ImageEffect
    {
        public StainedGlassEffect() : base() { }

        public override Color Apply(Color color, double opacity = 1) => color;

        public class Pixel
        {
            private int xOffset = 0;
            public int XOffset
            {
                get { return xOffset; }
                set { xOffset = value; }
            }

            private int yOffset = 0;
            public int YOffset
            {
                get { return yOffset; }
                set { yOffset = value; }
            }

            private byte blue = 0;
            public byte Blue
            {
                get { return blue; }
                set { blue = value; }
            }

            private byte green = 0;
            public byte Green
            {
                get { return green; }
                set { green = value; }
            }

            private byte red = 0;
            public byte Red
            {
                get { return red; }
                set { red = value; }
            }
        }

        public class VoronoiPoint
        {
            private int xOffset = 0;
            public int XOffset
            {
                get { return xOffset; }
                set { xOffset = value; }
            }


            private int yOffset = 0;
            public int YOffset
            {
                get { return yOffset; }
                set { yOffset = value; }
            }


            private int blueTotal = 0;
            public int BlueTotal
            {
                get { return blueTotal; }
                set { blueTotal = value; }
            }


            private int greenTotal = 0;
            public int GreenTotal
            {
                get { return greenTotal; }
                set { greenTotal = value; }
            }


            private int redTotal = 0;
            public int RedTotal
            {
                get { return redTotal; }
                set { redTotal = value; }
            }


            public void CalculateAverages()
            {
                if (pixelCollection.Count > 0)
                {
                    blueAverage = blueTotal / pixelCollection.Count;
                    greenAverage = greenTotal / pixelCollection.Count;
                    redAverage = redTotal / pixelCollection.Count;
                }
            }


            private int blueAverage = 0;
            public int BlueAverage
            {
                get { return blueAverage; }
            }


            private int greenAverage = 0;
            public int GreenAverage
            {
                get { return greenAverage; }
            }


            private int redAverage = 0;
            public int RedAverage
            {
                get { return redAverage; }
            }


            private List<Pixel> pixelCollection = new List<Pixel>();
            public List<Pixel> PixelCollection
            {
                get { return pixelCollection; }
            }


            public void AddPixel(Pixel pixel)
            {
                blueTotal += pixel.Blue;
                greenTotal += pixel.Green;
                redTotal += pixel.Red;


                pixelCollection.Add(pixel);
            }
        }

        public enum DistanceFormulaType
        {
            Chebyshev,
            Euclidean,
            Manhattan
        }

        private static Dictionary<int, int> squareRoots = new Dictionary<int, int>();

        private static int GetDistance(DistanceFormulaType type, int x1, int x2, int y1, int y2)
        {
            switch (type)
            {
                case DistanceFormulaType.Chebyshev:
                    return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));

                case DistanceFormulaType.Euclidean:
                    int square = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
                    if (squareRoots.ContainsKey(square) == false)
                        squareRoots.Add(square, (int)Math.Sqrt(square));

                    return squareRoots[square];

                case DistanceFormulaType.Manhattan:
                    return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            }
            return default;
        }

        /*
        public static void Render(int blockSize, double blockFactor, DistanceFormulaType distanceType, bool highlightEdges, byte edgeThreshold, System.Drawing.Color edgeColor)
        {
            int neighbourHoodTotal = 0;

            int sourceOffset = 0;
            int resultOffset = 0;

            int currentPixelDistance = 0;
            int nearestPixelDistance = 0;

            int nearesttPointIndex = 0;

            int Width = input.Width, Height = input.Height;

            Random randomizer = new Random();
            List<VoronoiPoint> randomPointList = new List<VoronoiPoint>();

            Parallel.For(0, Height - blockSize, row =>
            {
                for (int col = 0; col < Width - blockSize; col += blockSize)
                {
                    sourceOffset = row * sourceData.Stride + col * 4;

                    neighbourHoodTotal = 0;

                    for (int y = 0; y < blockSize; y++)
                    {
                        for (int x = 0; x < blockSize; x++)
                        {
                            resultOffset = sourceOffset + y * sourceData.Stride + x * 4;
                            neighbourHoodTotal += pixelBuffer[resultOffset];
                            neighbourHoodTotal += pixelBuffer[resultOffset + 1];
                            neighbourHoodTotal += pixelBuffer[resultOffset + 2];
                        }
                    }

                    randomizer = new Random(neighbourHoodTotal);

                    var randomPoint = new VoronoiPoint();
                    randomPoint.XOffset = randomizer.Next(0, blockSize) + col;
                    randomPoint.YOffset = randomizer.Next(0, blockSize) + row;
                    randomPointList.Add(randomPoint);

                    row += blockSize - 1;
                }
            });

            int rowOffset = 0;
            int colOffset = 0;
            for (int bufferOffset = 0; bufferOffset < pixelBuffer.Length - 4; bufferOffset += 4)
            {
                rowOffset = bufferOffset / sourceData.Stride;
                colOffset = (bufferOffset % sourceData.Stride) / 4;

                currentPixelDistance = 0;
                nearestPixelDistance = blockSize * 4;
                nearesttPointIndex = 0;

                List<VoronoiPoint> pointSubset = new List<VoronoiPoint>();

                pointSubset.AddRange(from t in randomPointList where rowOffset >= t.YOffset - blockSize * 2 && rowOffset <= t.YOffset + blockSize * 2 select t);
                for (int k = 0; k < pointSubset.Count; k++)
                {
                    if (distanceType == DistanceFormulaType.Euclidean)
                        currentPixelDistance = GetDistance(DistanceFormulaType.Euclidean, pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);

                    else if (distanceType == DistanceFormulaType.Manhattan)
                        currentPixelDistance = GetDistance(DistanceFormulaType.Manhattan, pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);

                    else if (distanceType == DistanceFormulaType.Chebyshev)
                        currentPixelDistance = GetDistance(DistanceFormulaType.Chebyshev, pointSubset[k].XOffset, colOffset, pointSubset[k].YOffset, rowOffset);

                    if (currentPixelDistance <= nearestPixelDistance)
                    {
                        nearestPixelDistance = currentPixelDistance;
                        nearesttPointIndex = k;

                        if (nearestPixelDistance <= blockSize / blockFactor)
                        {
                            break;
                        }
                    }
                }
                Pixel tmpPixel = new Pixel();
                tmpPixel.XOffset = colOffset;
                tmpPixel.YOffset = rowOffset;
                tmpPixel.Blue = pixelBuffer[bufferOffset];
                tmpPixel.Green = pixelBuffer[bufferOffset + 1];
                tmpPixel.Red = pixelBuffer[bufferOffset + 2];
                pointSubset[nearesttPointIndex].AddPixel(tmpPixel);
            }

            Parallel.For(0, randomPointList.Count, k =>
            {
                randomPointList[k].CalculateAverages();
                for (int i = 0; i < randomPointList[k].PixelCollection.Count; i++)
                {
                    resultOffset = randomPointList[k].PixelCollection[i].YOffset * sourceData.Stride + randomPointList[k].PixelCollection[i].XOffset * 4;
                    resultBuffer[resultOffset] = (byte)randomPointList[k].BlueAverage;
                    resultBuffer[resultOffset + 1] = (byte)randomPointList[k].GreenAverage;
                    resultBuffer[resultOffset + 2] = (byte)randomPointList[k].RedAverage;
                    resultBuffer[resultOffset + 3] = 255;
                }
            });
        }
        */

        public override ImageEffect Copy() => new StainedGlassEffect();
    }
}