using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace Paint.GLSL
{
    class DataReader
    {
        public List<DrawingData> ReadFile(string fileName)
        {
            try
            {
                return File.ReadAllLines(fileName)
                    .Where(n => new string(n.Take(2).ToArray()) != "//")
                    .Select(n =>
                    {
                        var m = n.Split(';').ToArray();
                        return new DrawingData
                        {
                            NameOfBrush = m[0],
                            SizeOfBrush = StrTofloat(m[1]),
                            Position = new Vector2f(StrTofloat(m[2]), StrTofloat(m[3])),
                            Color = new Color(Convert.ToByte(m[4]), Convert.ToByte(m[5]), Convert.ToByte(m[6])),
                        };
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid data format " +ex.Message);
            }
            
        }


        float StrTofloat(string str)
        {
            float value;
            return float.TryParse(str.Replace('.', ','), out value)
                ? value : float.Parse(str.Replace(',', '.'));
        }

    }
}
