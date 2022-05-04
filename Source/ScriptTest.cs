using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Urbrural.Source
{
    public class Point
    {
        public int X { get; set; }
        public int Y;
    }


    public class ScriptTest
    {
        public static async void Test()
        {
            var point = new Point {X = 3, Y = 5};

            var result = await CSharpScript.EvaluateAsync<int>("public class Abc { public int Value {get;set;}=120; }; new Abc().Value");

            var now = await CSharpScript.EvaluateAsync("System.DateTime.Now");
            Console.WriteLine(now);

            var ps = await CSharpScript.EvaluateAsync<int>("X*2 + Y*2", globals: point);
            Console.WriteLine(ps);
        }
    }
}