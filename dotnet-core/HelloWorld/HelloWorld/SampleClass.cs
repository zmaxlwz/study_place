
namespace HelloWorld
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SampleClass : SampleInterface
    {
        public int Age { get; set; } = 5;

        public void PrintName(int age, string name)
        {
            this.Age = age;
            Console.WriteLine("My name is: {0}", name);
        }
    }
}
