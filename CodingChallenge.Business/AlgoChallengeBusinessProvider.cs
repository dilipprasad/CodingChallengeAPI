using AutoMapper.Execution;
using CodingChallenge.Business.Interfaces;
using CodingChallenge.Models;
using System.Collections;
using System.IO.IsolatedStorage;

namespace CodingChallenge.Business
{
    public class AlgoChallengeBusinessProvider : IAlgoChallengeBusinessProvider
    {

        private object GetElementAtIndex(Hashtable hashTbl, int index)
        {
            int i = 0;
            foreach (var item in hashTbl)
            {
                if (i == index)
                {
                    return item;
                }
                i++;
            }
            return new object();
        }

        //private object GetFrequencyByColor(Hashtable hashTbl, string color)
        //{

        //    foreach (DictionaryEntry item in hashTbl)
        //    {
        //        if(item)
        //        return ((DictionaryEntry)item).Value;
        //    }
        //    return 0;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalNumberOfRecords">Total number of records from CSV</param>
        /// <param name="shapeObj">Collection of the Records from CSV File</param>
        /// <param name="ColorWithCounter">keyparis = Key -Color, value -count</param>
        /// <returns></returns>
        public Task<ShapeObjects[]> SolveChallenge(int totalNumberOfRecords, ArrayList shapeObj, Hashtable ColorWithCounter)
        {
            int distinctCount = ColorWithCounter.Count;

            //Build the Heap
            BuildHeap(totalNumberOfRecords, shapeObj, ColorWithCounter);

            ShapeObjects[] shapeObjNew = new ShapeObjects[totalNumberOfRecords];
            //for (int idx = 0; idx < distinctCount; idx++)
            //{
            //    var maxColor = ExtractMax(shapeObj, ColorWithCounter, totalNumberOfRecords - idx);

            //}
            //Validate max heap

            for (int idx = 0; idx < distinctCount; idx++)
            {
                var maxColor = ExtractMax(shapeObj, ColorWithCounter, totalNumberOfRecords - idx);

                var p = idx;
                while (shapeObjNew[p] != null)
                    p += 1;

                //var obj = shapeObj[idx] as ShapeObjects;
                //var obj = (DictionaryEntry)GetElementAtIndex(ColorWithCounter, idx);
                
                var freq = Convert.ToInt32(ColorWithCounter[maxColor.Color]);
                //var freq = Convert.ToInt32(ColorWithCounter[obj.Color]); //Get the frequency of the word
                for (int k = 0; k < freq; k++)
                {
                    if (p + distinctCount * k >= totalNumberOfRecords)
                        break;//Problem cannot
                    //
                    shapeObjNew[p + distinctCount * k] = maxColor;
                    //shapeObjNew.Insert(p + distinctCount * k, maxColor);
                }

            }

            return Task.FromResult(shapeObjNew);
        }

        private void BuildHeap(int totalNumberOfRecords, ArrayList shapeObj, Hashtable ColorWithCounter)
        {
            //Build Heap
            var centerPos = (int)Math.Floor((float)(totalNumberOfRecords) / 2);
            while (centerPos >= 0)
            {
                BuildMaxHeap(shapeObj, ColorWithCounter, centerPos, totalNumberOfRecords);
                centerPos -= 1; //Decrement by 1
            }


        }

        private int GetFrequency(ArrayList shapeObj, Hashtable ColorWithCounter, int index)
        {
            var colorObj = shapeObj[index] as ShapeObjects;

            return Convert.ToInt32(ColorWithCounter[colorObj.Color]); //Get frequency
        }

        private int GetCount(Dictionary<string, int> ColorWithCounter, int index)
        {
            return GetKeyPairObj(ColorWithCounter, index).Value;
        }

        private KeyValuePair<string, int> GetKeyPairObj(Dictionary<string, int> ColorWithCounter, int index)
        {
            int i = 0;
            foreach (var item in ColorWithCounter)
            {
                if (i == index)
                {
                    return item;
                }
                i++;
            }
            return new KeyValuePair<string, int>();
        }

        private void BuildMaxHeap(ArrayList shapeObj, Hashtable ColorWithCounter, int position, int recordsCount)
        {
            var leftPos = position * 2 + 1;
            var rightPos = position * 2 + 2;
            var largest = position; //Largest at the center

            if (leftPos < recordsCount && GetFrequency(shapeObj, ColorWithCounter, leftPos) > GetFrequency(shapeObj, ColorWithCounter, position))
                largest = leftPos;
            if (rightPos < recordsCount && GetFrequency(shapeObj, ColorWithCounter, rightPos) > GetFrequency(shapeObj, ColorWithCounter, largest))
                largest = rightPos;

            if (largest != position)
            {
                SwapPosition(shapeObj, position, largest);
                BuildMaxHeap(shapeObj, ColorWithCounter, largest, recordsCount);

            }

        }


        private void SwapPosition(ArrayList shapeObj, int position, int largest)
        {
            //Get values to temp variable
            var tempObj = shapeObj[position];

            //Now swap data
            shapeObj[position] = shapeObj[largest];

            shapeObj[largest] = tempObj;
        }

        private ShapeObjects ExtractMax(ArrayList shapeObj, Hashtable ColorWithCounter, int recordsCount)
        {
            var maxColor = shapeObj[0] as ShapeObjects;
            if (recordsCount > 1)
            {
                shapeObj[0] = shapeObj[recordsCount - 1];
                BuildMaxHeap(shapeObj, ColorWithCounter, 0, recordsCount - 1);
            }
            return maxColor;
        }


    }
}
