namespace Evoo
{
    public static class Program
    {
        /// <summary>
        /// Application main entry point.
        /// </summary>
        public static void Main(string[] Args)
        {
            Window w = new Window();

            // This is how you fix the fps if anyone's wonder, however, it creates fps's so high, nobody
            // would notice, takes much more cpu, and stresses the graphics card... I'll just leave this
            // here anyway.
            w.VSync = OpenTK.VSyncMode.Off;


            w.Run();
        }
    }
}