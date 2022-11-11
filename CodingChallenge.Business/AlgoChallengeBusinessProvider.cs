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
        /// <param name="shapeAndColor">Collection of the Records from CSV File</param>
        /// <param name="ColorWithCounter">keyparis = Key -Color, value -count</param>
        /// <returns></returns>
        public Task<ArrayList> SolveChallenge(int totalNumberOfRecords, ShapeAndColor shapeAndColorObj)
        {
            int distinctColorCount = shapeAndColorObj.GetDistinctColorCount();

            //Build the Heap
            //BuildHeap(totalNumberOfRecords, shapeAndColorObj);
            BuildHeap(distinctColorCount, shapeAndColorObj);

            ArrayList shapeObjNew = new ArrayList();
            
            for (int idx = 0; idx < distinctColorCount; idx++)
            {
                //var maxColor = ExtractMax(shapeAndColorObj,  totalNumberOfRecords - idx);
                var maxColor = ExtractMax(shapeAndColorObj, distinctColorCount - idx);

                var p = idx;
                //while (shapeObjNew[p] != null)
                while (shapeObjNew.IndexOf(p) != -1)
                    p += 1;

                
                var freq = maxColor.Count;
                for (int k = 0; k < freq; k++)
                {
                    if (p + distinctColorCount * k >= totalNumberOfRecords)
                        break;//Problem cannot
                    
                    //shapeObjNew[p + distinctColorCount * k] = maxColor[k];
                    shapeObjNew.Insert(p + distinctColorCount * k, maxColor[k]);
                }

            }

            return Task.FromResult(shapeObjNew);
        }

        private void BuildHeap(int totalNumberOfRecords, ShapeAndColor shapeAndColorObj)
        {
            //Build Heap
            var centerPos = (int)Math.Floor((float)(totalNumberOfRecords) / 2);
            while (centerPos >= 0)
            {
                BuildMaxHeap(shapeAndColorObj,  centerPos, totalNumberOfRecords);
                centerPos -= 1; //Decrement by 1
            }


        }

        private int GetFrequency(ShapeAndColor shapeAndColorObj, int index)
        {
            var colorObj = shapeAndColorObj.GetColorObjectByIndex(index);

            return Convert.ToInt32(colorObj.Count); //Get frequency
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

        private void BuildMaxHeap(ShapeAndColor shapeAndColorObj, int position, int recordsCount)
        {
            var leftPos = position * 2 + 1;
            var rightPos = position * 2 + 2;
            var largest = position; //Largest at the center

            if (leftPos < recordsCount && GetFrequency(shapeAndColorObj,   leftPos) > GetFrequency(shapeAndColorObj,   position))
                largest = leftPos;
            if (rightPos < recordsCount && GetFrequency(shapeAndColorObj,   rightPos) > GetFrequency(shapeAndColorObj,  largest))
                largest = rightPos;

            if (largest != position)
            {
                shapeAndColorObj.SwapPosition(position, largest);
                BuildMaxHeap(shapeAndColorObj,   largest, recordsCount);

            }

        }


        private ColorObject ExtractMax(ShapeAndColor shapeAndColorObj, int recordsCount)
        {
            var maxColor = shapeAndColorObj.GetColorObjectByIndex(0) as ColorObject;
            if (recordsCount > 1)
            {
                //shapeObj[0] = shapeObj[recordsCount - 1];
                shapeAndColorObj.SetColorObjectByIndex(shapeAndColorObj.GetColorObjectByIndex(recordsCount - 1),0);
                BuildMaxHeap(shapeAndColorObj,  0, recordsCount - 1);
            }
            return maxColor;
        }


    }
}
