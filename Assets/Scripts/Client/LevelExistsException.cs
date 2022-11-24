using System;

namespace Editor
{
    public class LevelExistsException : Exception
    {
        public LevelExistsException(string level) : base(message: $"Level with id {level} already exists")
        {
        }
    }
}