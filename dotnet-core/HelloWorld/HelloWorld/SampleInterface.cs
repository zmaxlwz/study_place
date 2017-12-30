
namespace HelloWorld
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface SampleInterface
    {
        int Age { get; }

        void PrintName(int age, string name = "default name");
    }
}
