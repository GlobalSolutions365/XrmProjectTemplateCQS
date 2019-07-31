using System;

namespace FlowVisualizer
{
    public class Color : IDisposable
    {
        public Color(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public void Dispose()
        {
            Console.ResetColor();
        }
    }
}
