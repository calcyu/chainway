using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace Urbrural
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

            var scripta = CSharpScript.Create<Type>("public class Abc : Urbrural.Point { public int Z => 120; }; return typeof(Abc);", ScriptOptions.Default.WithReferences("Urbrural"));
            scripta.Compile();
            var state = (await scripta.RunAsync());
            var clazz = state.ReturnValue;
            var assembly = clazz.Assembly.GetName().Name;

            var obj = Activator.CreateInstance(clazz);


            var scriptb = CSharpScript.Create<int>("Z",ScriptOptions.Default.AddReferences(assembly), globalsType: clazz);
            scriptb.Compile();
            var ret = (await scriptb.RunAsync(globals: obj)).ReturnValue;


            Console.WriteLine("");
        }
    }
}