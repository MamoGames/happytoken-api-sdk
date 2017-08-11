namespace HappyTokenApi.Debugging
{
    public abstract class DebugWindow
    {
        public int Id { get; }

        public string Title { get; }

        public abstract void Draw();

        protected DebugWindow(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}