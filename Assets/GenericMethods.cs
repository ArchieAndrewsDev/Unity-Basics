namespace Basics
{
    public static class GenericMethods
    {
        public static int GetAdjacentId(int curId, int size, int direction)
        {
            int newId = curId += direction;

            if (newId >= size)
                newId = 0;

            if (newId < 0)
                newId = size - 1;

            return newId;
        }
    }
}
