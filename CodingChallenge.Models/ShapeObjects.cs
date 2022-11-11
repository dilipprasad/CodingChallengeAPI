using System;
using System.Collections;

namespace CodingChallenge.Models
{
    public class ColorObject
    {
        /// <summary>
        /// Color Property
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Dynamically Calculates the Sum of the Color based on the sub objets (shapes)
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                for (int i = 0; i < ShapeObj.Count; i++)
                {
                    count += (ShapeObj[i] as ShapeObjects)?.Count ?? 0;//Each Color has shapes obj as collection
                }
                return count;
            }
        }

        /// <summary>
        /// Arraylist of ShapeObjects
        /// </summary>
        public ArrayList ShapeObj { get; set; }

        public ColorObject(string color, string shape)
        {
            Color = color;
            ShapeObj = new ArrayList();
            ShapeObj.Add(new ShapeObjects()
            {
                Shape = shape,
                Count = 1

            });
        }

        private int FindIndexByShape(string shape)
        {
            for (int i = 0; i < ShapeObj.Count; i++)
            {
                if ((ShapeObj[i] as ShapeObjects)?.Shape == shape)
                {
                    return i;
                }
            }
            return -1;//Not found
        }


        public void UpdateCount(string shape,int count=1)
        {
            var index= FindIndexByShape(shape);
            if(index >-1)
            {
                (ShapeObj[index] as ShapeObjects).Count += count; 
            }
            else
            {
                ShapeObj.Add(new ShapeObjects()
                {
                    Count = count,
                    Shape=shape
                });
                //throw new Exception("Shape not initialized, Something went wrong");
            }
        }

        public ColorObject this[int index] => Pop(index);

        public ColorObject Pop(int index)
        {
            var shape = ShapeObj[index] as ShapeObjects;
            return new ColorObject(this.Color,shape.Shape);
        }
    }
    public class ShapeObjects
    {

        /// <summary>
        /// Shape - Part of the color
        /// </summary>
        public string Shape { get; set; }

        /// <summary>
        /// we are maintaining count for each shape part of the color
        /// </summary>
        public int Count { get; set; }

    }

    /// <summary>
    /// Parent Object for the shape and color
    /// </summary>
    public class ShapeAndColor
    {
        //ColorObjects
        private ArrayList _colorObjects { get; set; }

        public ShapeAndColor()
        {
            _colorObjects = new ArrayList();
        }

        public int this[string color] => FindIndexByColor(color);

        private int FindIndexByColor(string color)
        {
            for (int i = 0; i < _colorObjects.Count; i++)
            {
                if ((_colorObjects[i] as ColorObject)?.Color == color)
                {
                    return i;
                }
            }
            return -1;//Not found
        }

        public void Add(string color, string shape)
        {
            int index = FindIndexByColor(color);
            if (index == -1)
            {
                var obj = new ColorObject(color,shape);
                _colorObjects.Add(obj);
            }
            else
            {
                var colorObj = _colorObjects[index] as ColorObject;
                colorObj.UpdateCount(shape);
            }
        }

        public int GetDistinctColorCount()
        {
           return _colorObjects.Count;
        }

        public ColorObject GetColorObjectByIndex(int index)
        {
            return _colorObjects[index] as ColorObject;
        }

        public void SwapPosition(int fromIdx,int toIdx)
        {
            //Get values to temp variable
            var tempObj = _colorObjects[fromIdx];

            _colorObjects[fromIdx] = _colorObjects[toIdx];

            _colorObjects[toIdx] = tempObj;
        }

        public void SetColorObjectByIndex(ColorObject colorObject, int index)
        {
            _colorObjects[index]=colorObject;
            
        }
    }
}
