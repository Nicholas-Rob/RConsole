using System;
using System.Collections.Generic;
using System.Linq;

namespace RConsole
{
    class BStack
    {
        public Type stackType;
        private object[] objects;
        private int limit = 0;
        private bool cycle = false;
        public BStack()
        {
            objects = new object[1];
            
        }
        public BStack(int limit)
        {
            objects = new object[1];
            this.limit = limit;
        }

        public BStack(int limit, bool cycle)
        {
            objects = new object[1];
            this.limit = limit;
            this.cycle = cycle;
        }

        public bool Add(object obj)
        {
            if(stackType == null)
            {
                objects[0] = obj;
                stackType = obj.GetType();

                return true;
            }
            else
            {
                if(obj.GetType() == stackType)
                {
                    
                    if (limit == 0 || objects.Count() < limit)
                    {
                        objects = objects.Append(obj).ToArray();
                        return true;
                    }
                    else if (cycle)
                    {
                        objects = objects.Skip(1).Append(obj).ToArray();
                        return true;
                    }
                    else
                    {
                        
                        throw new LimitReachedException(limit);
                    }
                }
                else
                {
                    throw new DifferentTypeException(stackType, obj.GetType());
                }
            }

            return false;
        }

        public object Pop()
        {
            if (objects.Count() > 0)
            {
                object obj = objects.Last();
                objects = objects.SkipLast(1).ToArray();

                return obj;
            }

            return null;
        }

        public object Peek(int count)
        {
            if (objects.Count() > count)
            {
                return objects.SkipLast(count).Last();
            }

            return null;
        }

        public void Display()
        {
            int count = 0;
            foreach(object obj in objects)
            {
                Console.WriteLine(count + ": " + objects[count]);
                count++;
            }
        }
    }

    class DifferentTypeException : Exception
    {
        public DifferentTypeException(Type stackType, Type objType)
            : base(String.Format("Invalid object type: Stack Type = {0} , Object Type = {1}", stackType.Name, objType.Name))
        {

        }
    }

    class LimitReachedException : Exception
    {
        public LimitReachedException(int limit)
            : base(String.Format("Stack limit reached: Limit = {0}", limit))
        {

        }
    }
}
